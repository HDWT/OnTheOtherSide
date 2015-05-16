using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LineOfSight : MonoBehaviour
{
	[System.Serializable]
	private class Settings
	{
		public float NearWidth	= 1;
		public float FarWidth	= 2;
		public float Length		= 3;
		public float Height		= 1;

		public bool Equal(Settings other)
		{
			return Mathf.Approximately(NearWidth, other.NearWidth) &&
				Mathf.Approximately(FarWidth, other.FarWidth) &&
				Mathf.Approximately(Length, other.Length) &&
				Mathf.Approximately(Height, other.Height);
		}

		public void Copy(Settings other)
		{
			NearWidth	= other.NearWidth;
			FarWidth	= other.FarWidth;
			Length		= other.Length;
			Height		= other.Height;
		}
	}

	#region --- Serialized Fields ---

	[SerializeField]
	private Settings m_settings = new Settings();

	[SerializeField, HideInInspector]
	private MeshCollider m_meshCollider = null;

	[SerializeField, HideInInspector]
	private MeshFilter m_meshFilter = null;

	[SerializeField, HideInInspector]
	private MeshRenderer m_meshRenderer = null;

	#endregion

	private Transform m_transform = null;
	private Settings m_lastSettings = new Settings();

	public System.Action<Collider> OnEnter = null;
	public System.Action<Collider> OnExit = null;

	void OnEnable()
	{
		Init();
	}

	void OnDestroy()
	{
		OnEnter = null;
		OnExit = null;
	}

	private void Init()
	{
		if (m_transform == null)
			m_transform = GetComponent<Transform>();

		//
		if (m_meshCollider == null)
			m_meshCollider = GameObjectUtils.GetOrAddComponent<MeshCollider>(this.gameObject);

		//
		if (m_meshFilter == null)
			m_meshFilter = GameObjectUtils.GetOrAddComponent<MeshFilter>(this.gameObject);

		//
		if (m_meshRenderer == null)
			m_meshRenderer = GameObjectUtils.GetOrAddComponent<MeshRenderer>(this.gameObject);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider && (OnEnter != null))
			OnEnter(collider);
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider && (OnExit != null))
			OnExit(collider);
	}

	void Update()
	{
		UpdateGeneratedMeshes();
	}

	private void UpdateGeneratedMeshes()
	{
		if (!m_lastSettings.Equal(m_settings))
		{
			m_lastSettings.Copy(m_settings);

			UpdateMeshForRenderer();
			UpdateMeshForCollider();
		}
		else
		{
			if (m_meshFilter && m_meshFilter.sharedMesh == null)
				UpdateMeshForRenderer();

			if (m_meshCollider && m_meshCollider.sharedMesh == null)
				UpdateMeshForCollider();
		}
	}

	//
	private void UpdateMeshForRenderer()
	{
		if (m_meshFilter == null)
			return;

		if (m_meshFilter.sharedMesh == null)
			m_meshFilter.sharedMesh = GenerateMeshForRender();
		else
			m_meshFilter.sharedMesh.vertices = GetVerticesForRender();

		if (m_meshRenderer != null)
		{
			m_meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			m_meshRenderer.receiveShadows = false;
			m_meshRenderer.useLightProbes = false;
			m_meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		}
	}

	private Vector3[] GetVerticesForRender()
	{
		Vector3 nearLeft	= -m_transform.right * m_settings.NearWidth;
		Vector3 nearRight	= m_transform.right * m_settings.NearWidth;

		Vector3 farLeft		= -m_transform.right * m_settings.FarWidth + m_transform.forward * m_settings.Length;
		Vector3 farCenter	= m_transform.forward * (m_settings.Length + m_settings.FarWidth / 4.0f);
		Vector3 farRight	= m_transform.right * m_settings.FarWidth + m_transform.forward * m_settings.Length;

		return new Vector3[] { nearLeft, nearRight, farLeft, farCenter, farRight };
	}

	private Mesh GenerateMeshForRender()
	{
		Mesh mesh = new Mesh();

		mesh.name = "generated_mesh";
		mesh.vertices = GetVerticesForRender();

		mesh.triangles = new int[]
		{
			0, 2, 3, // left top
			0, 3, 2, // left bottom

			0, 3, 1, // center top
			1, 3, 0, // center bottom

			1, 3, 4, // right top
			1, 4, 3, // right bottom
		};

		return mesh;
	}

	//
	private void UpdateMeshForCollider()
	{
		if (m_meshCollider == null)
			return;

		m_meshCollider.isTrigger = false;
		m_meshCollider.convex = false;

		if (m_meshCollider.sharedMesh == null)
			m_meshCollider.sharedMesh = GenerateMeshForCollider();
		else
			m_meshCollider.sharedMesh.vertices = GetVerticesForCollider();

		m_meshCollider.convex = true;
		m_meshCollider.isTrigger = true;
	}

	private Vector3[] GetVerticesForCollider()
	{
		Vector3 nearLeft		= -m_transform.right * m_settings.NearWidth;
		Vector3 nearRight		= m_transform.right * m_settings.NearWidth;

		Vector3 farLeftTop		= -m_transform.right * m_settings.FarWidth + m_transform.forward * m_settings.Length + m_transform.up * m_settings.Height;
		Vector3 farCenterTop	= m_transform.forward * (m_settings.Length + m_settings.FarWidth / 4.0f) + m_transform.up * m_settings.Height;
		Vector3 farRightTop		= m_transform.right * m_settings.FarWidth + m_transform.forward * m_settings.Length + m_transform.up * m_settings.Height;

		Vector3 farLeftBottom	= -m_transform.right * m_settings.FarWidth + m_transform.forward * m_settings.Length - m_transform.up * m_settings.Height;
		Vector3 farCenterBottom = m_transform.forward * (m_settings.Length + m_settings.FarWidth / 4.0f) - m_transform.up * m_settings.Height;
		Vector3 farRightBottom	= m_transform.right * m_settings.FarWidth + m_transform.forward * m_settings.Length - m_transform.up * m_settings.Height;

		return new Vector3[] { nearLeft, nearRight, farLeftTop, farCenterTop, farRightTop, farLeftBottom, farCenterBottom, farRightBottom };
	}

	private Mesh GenerateMeshForCollider()
	{
		Mesh mesh = new Mesh();

		mesh.name = "generated_mesh";
		mesh.vertices = GetVerticesForCollider();

		mesh.triangles = new int[]
		{
			0, 5, 2, // left
			2, 5, 3, // left farTop
			5, 6, 3, // left farBottom
			0, 2, 3, // left top
			0, 6, 5, // left bottom

			0, 3, 1, // center top
			0, 1, 6, // center bottom

			1, 4, 7, // right
			4, 3, 7, // right farTop
			3, 6, 7, // right farBottom
			1, 3, 4, // right top
			1, 7, 6, // right bottom
		};

		return mesh;
	}
}

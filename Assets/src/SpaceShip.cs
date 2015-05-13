using UnityEngine;
using System.Collections;

public enum SpaceShipState
{
	Idle,
	Moving,
}

public class SpaceShip : MonoBehaviour
{
	#region --- Serialized Fields ---

	[SerializeField]
	private GravitySense m_gravitySense = null;

	[SerializeField]
	private float m_launchSpeed = 10;

	[SerializeField]
	private float m_drag = 0.1f;

	[SerializeField]
	private Transform m_muzzle = null;

	[SerializeField]
	private float m_bulletMaxAngle = 5;

	[SerializeField]
	private float m_maxNonVisibleTime = 0.2f;

	#endregion

	private Transform m_transform = null;
	private Rigidbody m_rigidbody = null;
	private Renderer[] m_meshRenderers = null;

	private Camera m_camera = null;
	private Plane m_ground = new Plane(Vector3.up, Vector3.zero);

	private SpaceShipState m_state = SpaceShipState.Idle;
	private float m_currentSpeed = 0;
	private bool m_destroyed = false;

	private float m_notVisibleTime = 0;

	public delegate void OnDestroyEventHandler();
	public OnDestroyEventHandler OnDestroy = null;
	public System.Action OnGoesAway = null;

	void Awake()
	{
		m_transform = GetComponent<Transform>();
		m_rigidbody = GetComponent<Rigidbody>();
		m_meshRenderers = GetComponentsInChildren<Renderer>();
	}

	public void OnLaunch()
	{
		m_currentSpeed = m_launchSpeed;
		m_state = SpaceShipState.Moving;
		m_rigidbody.isKinematic = false;
	}

	void Update()
	{
		if (m_destroyed)
			return;

		if (m_state == SpaceShipState.Idle)
		{

		}
		else if (m_state == SpaceShipState.Moving)
		{
			foreach (var renderer in m_meshRenderers)
			{
				if (renderer == null)
					continue;

				m_notVisibleTime = (renderer.isVisible) ? (0) : (m_notVisibleTime + Time.deltaTime);
				break;
			}

			if ((m_notVisibleTime > m_maxNonVisibleTime) && (OnGoesAway != null))
				OnGoesAway();

			if (m_currentSpeed > 0)
			{
				Vector3 gravity = Vector3.zero;

				if (m_gravitySense != null)
				{
					gravity = m_gravitySense.GetPower();
					gravity.y = 0;

					if (gravity != Vector3.zero)
					{
						Quaternion lookRotation = Quaternion.LookRotation(m_transform.forward + Vector3.Normalize(gravity));
						m_transform.rotation = Quaternion.Lerp(m_transform.rotation, lookRotation, Time.deltaTime * Vector3.Magnitude(gravity));

						Debug.DrawLine(m_transform.position, m_transform.position + m_transform.forward + gravity, Color.green);
					}
				}

				m_transform.Translate(Vector3.forward * m_currentSpeed * Time.deltaTime);

				if (m_drag > 0)
					m_currentSpeed -= m_drag * Time.deltaTime * 50;
			}

			if (m_currentSpeed <= 0)
			{
				m_currentSpeed = 0;
				m_state = SpaceShipState.Idle;
			}
			else
			{
				Vector3 hitPoint = Vector3.zero;

				if ((Input.GetMouseButtonDown(0) && GroundRaycast(out hitPoint)) || Input.GetKeyDown(KeyCode.Space))
				{
					Bullet bullet = GameObject.Instantiate(CommonSettings.Instance.Bullet) as Bullet;

					bullet.transform.position = m_muzzle.position;
					bullet.transform.rotation = m_muzzle.rotation;

					float rotation = Random.Range(-m_bulletMaxAngle, m_bulletMaxAngle);

					bullet.transform.Rotate(Vector3.up, rotation);

					SceneFolder.Instance.Add(bullet.gameObject, "Bullets");
				}
			}
		}
	}

	IEnumerator OnCollisionEnter(Collision collision)
	{
		m_currentSpeed = 0;
		m_rigidbody.isKinematic = true;
		m_state = SpaceShipState.Idle;
		m_destroyed = true;

		foreach (var renderer in m_meshRenderers)
		{
			if (renderer)
				renderer.enabled = false;
		}

		GameObject explosion = GameObject.Instantiate(CommonSettings.Instance.ExplosionEffect) as GameObject;
		explosion.transform.position = m_transform.position;
		explosion.transform.rotation = m_transform.rotation;

		yield return new WaitForSeconds(1);

		if (OnDestroy != null)
			OnDestroy();
	}

	void OnGUI()
	{
		//if (Application.isEditor)
		//{
		//	GUILayout.Label("State: " + m_state);
		//	GUILayout.Label("Speed: " + m_currentSpeed);
		//}
	}

	private bool GroundRaycast(out Vector3 point)
	{
		if (m_camera == null)
			m_camera = Camera.main;

		point = Vector3.zero;

		Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
		float distance = 0;

		if (!m_ground.Raycast(ray, out distance))
			return false;

		point = ray.GetPoint(distance);
		return true;
	}
}

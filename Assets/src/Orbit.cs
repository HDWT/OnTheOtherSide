using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
	#region --- Serialized Fields ---

	[SerializeField]
	private float m_width = 1.5f;

	[SerializeField]
	private float m_height = 2.1f;

	[SerializeField]
	private Transform m_object = null;

	[SerializeField]
	private float m_speed = 15;

	[SerializeField]
	private float m_angle = 0;

	[SerializeField]
	private bool m_lookAtPath = true;

	[SerializeField]
	private bool m_moveForward = true;

	#endregion

	private Transform m_transform = null;

	void Awake()
	{
		m_transform = GetComponent<Transform>();
	}

	void Update()
	{
		if (m_object == null)
			return;

		m_angle += m_speed * Time.deltaTime;
		m_angle %= 360;

		UpdateObjectPosition();
	}

	private void UpdateObjectPosition()
	{
		if (m_object == null)
			return;

		float x = m_width * Mathf.Cos(Mathf.Deg2Rad * m_angle);
		float z = m_height * Mathf.Sin(Mathf.Deg2Rad * m_angle);

		Vector3 currPos = m_moveForward
			? m_transform.right * (x + m_transform.localPosition.x) + m_transform.up * m_transform.localPosition.y + m_transform.forward * (z + m_transform.localPosition.z)
			: m_transform.right * (x + m_transform.localPosition.x) + m_transform.up * m_transform.localPosition.y - m_transform.forward * (z + m_transform.localPosition.z);

		if (m_lookAtPath)
			m_object.LookAt(currPos);

		m_object.localPosition = currPos;
	}

	#if UNITY_EDITOR

	void OnDrawGizmos()
	{
		//if (UnityEditor.Selection.activeGameObject != this.gameObject)
		//	return;

		if (m_transform == null)
			m_transform = GetComponent<Transform>();

		if (!Application.isPlaying)
		{
			UpdateObjectPosition();
			m_angle %= 360;
		}

		Vector3 firstPos = Vector3.zero;
		Vector3 prevPos = Vector3.zero;

		for (int angle = 0; angle < 360; angle += 10)
		{
			float x = m_width * Mathf.Cos(Mathf.Deg2Rad * angle);
			float z = m_height * Mathf.Sin(Mathf.Deg2Rad * angle);

			if (firstPos == Vector3.zero)
			{
				firstPos = m_transform.right * (x + m_transform.localPosition.x) + m_transform.up * m_transform.localPosition.y + m_transform.forward * (z + m_transform.localPosition.z);
				firstPos = m_transform.TransformPoint(firstPos);

				prevPos = firstPos;
				continue;
			}

			Vector3 currPos = m_transform.right * (x + m_transform.localPosition.x) + m_transform.up * m_transform.localPosition.y + m_transform.forward * (z + m_transform.localPosition.z);
			currPos = m_transform.TransformPoint(currPos);

			Gizmos.DrawLine(prevPos, currPos);
			prevPos = currPos;
		}

		Gizmos.DrawLine(prevPos, firstPos);
	}

	#endif
}


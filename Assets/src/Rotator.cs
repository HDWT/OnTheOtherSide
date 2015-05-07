using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
	[SerializeField]
	private Vector3 m_velocity = Vector3.zero;

	private Transform m_transform = null;

	void Update()
	{
		if (m_transform == null)
			m_transform = GetComponent<Transform>();

		if (m_velocity.x != 0)
			m_transform.Rotate(Vector3.right * m_velocity.x * Time.deltaTime);

		if (m_velocity.y != 0)
			m_transform.Rotate(Vector3.up * m_velocity.y * Time.deltaTime);

		if (m_velocity.z != 0)
			m_transform.Rotate(Vector3.forward * m_velocity.z * Time.deltaTime);
	}
}

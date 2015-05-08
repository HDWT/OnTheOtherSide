using UnityEngine;
using System.Collections;

public enum LayerIndex
{
	Player = 8,
	Portal = 9,
	Bullet = 11,
}

public class Mine : MonoBehaviour
{
	[SerializeField]
	private float m_speed = 10;

	private Transform m_transform = null;
	private Transform m_target = null;

	void Awake()
	{
		m_transform = GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
		if (m_target != null)
		{
			float distance = Vector3.Distance(m_transform.position, m_target.position);
			//Quaternion.

			m_transform.Translate(Vector3.Normalize(m_target.position - m_transform.position) * m_speed * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider && collider.gameObject.layer == (int)LayerIndex.Player)
			m_target = collider.transform;
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider && collider.gameObject.layer == (int)LayerIndex.Player)
			m_target = null;
	}

	void OnGUI()
	{
		if (m_target != null)
			GUILayout.Label("Mine distance: " + Vector3.Distance(m_transform.position, m_target.position));
	}
}

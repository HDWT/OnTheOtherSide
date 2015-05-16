using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour
{
	[SerializeField]
	private float m_speed = 10;

	[SerializeField]
	private float m_drag = 0.1f;

	[SerializeField]
	private MeshRenderer m_meshRenderer = null;

	private Transform m_transform = null;
	private Transform m_target = null;

	private float m_currentSpeed = 0;
	private Vector3 m_lastDirection = Vector3.zero;

	void Awake()
	{
		m_transform = GetComponent<Transform>();
	}

	void Start()
	{
		if (m_meshRenderer != null)
			m_meshRenderer.transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
	}

	void Update()
	{
		if (m_target != null)
		{
			m_lastDirection = Vector3.Normalize(m_target.position - m_transform.position);
			m_transform.Translate(m_lastDirection * m_speed * Time.deltaTime);
		}
		else if (m_currentSpeed > 0)
		{
			m_transform.Translate(m_lastDirection * m_currentSpeed * Time.deltaTime);
			m_currentSpeed -= m_drag * Time.deltaTime * 50;
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider && LayerUtils.Is(collider.gameObject, LayerType.Player))
		{
			m_target = collider.transform;
			m_currentSpeed = 0;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider && LayerUtils.Is(collider.gameObject, LayerType.Player))
		{
			m_target = null;
			m_currentSpeed = m_speed;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		GameObject.Destroy(this.gameObject);
	}
}

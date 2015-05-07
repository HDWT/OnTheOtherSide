using UnityEngine;
using System.Collections;

public enum ShipMode
{
	Idle,
	RouteSelection,
	Moving,
}

public class ShipController : MonoBehaviour
{
	const float RaycastDistance = 100.0f;

	[SerializeField]
	private Camera m_camera = null;

	[SerializeField]
	private Collider m_collider = null;

	private Transform m_transform = null;

	private ShipMode m_mode = ShipMode.Idle;

	private float m_currentSpeed = 0;

	[SerializeField]
	private float m_initialSpeed = 10;

	[SerializeField]
	private float m_slowingPerFrame = 0.1f;

	void Awake()
	{
		if (m_camera == null)
			m_camera = Camera.main;

		m_transform = GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
		if (m_mode == ShipMode.Idle)
		{
			if (Input.GetMouseButton(0))
			{

				if (Raycast())
				{
					m_mode = ShipMode.RouteSelection;
				}
			}
		}
		else if (m_mode == ShipMode.RouteSelection)
		{
			if (Input.GetMouseButton(0))
			{
				if (Raycast())
				{
					m_mode = ShipMode.Moving;

					m_currentSpeed = m_initialSpeed;
				}
			}
		}
		else if (m_mode == ShipMode.Moving)
		{
			if (m_currentSpeed > 0)
			{
				m_transform.Translate(Vector3.forward * m_currentSpeed * Time.deltaTime);

				m_currentSpeed -= m_slowingPerFrame * Time.deltaTime;
			}
			
			if (m_currentSpeed <= 0)
			{
				m_currentSpeed = 0;
				m_mode = ShipMode.Idle;
			}
		}

	}

	void OnGUI()
	{
		GUILayout.Label("Mode: " + m_mode);
	}

	private bool Raycast()
	{
		Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		return m_collider.Raycast(ray, out hitInfo, RaycastDistance);
	}
}

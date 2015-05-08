using UnityEngine;
using System.Collections;

public class LaunchZone : MonoBehaviour
{
	const float RaycastDistance = 300.0f;

	private enum State
	{
		Idle,
		DirSelection,
	}

	public delegate void LaunchEventHandler(Vector3 start, Vector3 end);

	#region --- Serialized Fields ---

	[SerializeField]
	private Collider m_inputCollider = null;

	[SerializeField]
	private LineRenderer m_lineRenderer = null;

	#endregion

	private State	m_state			= State.Idle;
	private Camera	m_camera		= null;
	private Vector3 m_startPoint	= Vector3.zero;
	private Plane	m_ground		= new Plane(Vector3.up, Vector3.zero);

	public LaunchEventHandler OnLaunch = null;

	void OnEnable()
	{
		m_lineRenderer.gameObject.SetActive(m_state == State.DirSelection);
	}

	void Update()
	{
		switch (m_state)
		{
			case State.Idle:
				{
					if (Input.GetMouseButtonDown(0))
					{
						Vector3 hitPoint = Vector3.zero;

						if (ZoneRaycast(out hitPoint))
						{
							hitPoint.y = 0;
							m_startPoint = hitPoint;
							
							m_lineRenderer.gameObject.SetActive(true);
							m_lineRenderer.SetPosition(0, m_startPoint);
							m_lineRenderer.SetPosition(1, m_startPoint);

							m_state = State.DirSelection;
						}
					}
				}

				break;

			case State.DirSelection:
				{
					if (Input.GetMouseButtonUp(0))
					{
						Vector3 hitPoint = Vector3.zero;
						bool launch = !ZoneRaycast(out hitPoint);

						m_lineRenderer.gameObject.SetActive(false);
						m_state = State.Idle;

						if (launch && GroundRaycast(out hitPoint))
						{
							hitPoint.y = 0;

							if (OnLaunch != null)
								OnLaunch(m_startPoint, hitPoint);
						}
					}
					else
					{
						Vector3 hitPoint = Vector3.zero;

						if (GroundRaycast(out hitPoint))
						{
							hitPoint.y = 0;
							m_lineRenderer.SetPosition(1, hitPoint);
						}
					}
				}
				break;

			default:
				Debug.LogError(string.Format("Case for {0} not implemented", m_state));
				break;
		}
	}

	private bool GroundRaycast(out Vector3 point)
	{
		point = Vector3.zero;

		Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
		float distance = 0;

		if (!m_ground.Raycast(ray, out distance))
			return false;

		point = ray.GetPoint(distance);
		return true;
	}

	private bool ZoneRaycast(out Vector3 point)
	{
		if (m_camera == null)
			m_camera = Camera.main;

		if (m_camera == null)
		{
			point = Vector3.zero;
			return false;
		}

		Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		bool result = m_inputCollider.Raycast(ray, out hitInfo, RaycastDistance);
		point = hitInfo.point;

		return result;
	}
}

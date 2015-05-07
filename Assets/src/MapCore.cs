using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MapState
{
	Idle,
	Launch,
	Play,
}

public class MapCore : MonoBehaviour
{
	#region --- Serialized Fields ---

	[SerializeField]
	private List<LaunchZone> m_launchZones = new List<LaunchZone>();

	#endregion

	private MapState m_state = MapState.Idle;
	private SpaceShip m_shipController = null;

	public MapState State
	{
		get { return m_state; }
		private set
		{
			if (m_state != value)
			{
				m_state = value;
				OnStateChanged();
			}
		}
	}

	IEnumerator Start()
	{
		foreach (LaunchZone zone in m_launchZones)
		{
			if (zone != null)
				zone.OnLaunch = OnLaunch;
		}

		yield return null;

		State = MapState.Launch;
	}

	private void SetLaunchZoneActive(bool active)
	{
		foreach (LaunchZone zone in m_launchZones)
		{
			if (zone != null)
				zone.gameObject.SetActive(active);
		}
	}

	private void Restart()
	{
		State = MapState.Launch;
	}

	private void OnLaunch(Vector3 start, Vector3 end)
	{
		if (m_state != MapState.Launch)
		{
			Debug.LogError(string.Format("Launch failed. Current state {0}. Expected state {1}", m_state, MapState.Launch));
			return;
		}

		if (m_shipController != null)
			GameObject.Destroy(m_shipController.gameObject);

		m_shipController = GameObject.Instantiate(CommonSettings.Instance.ShipController);
		m_shipController.transform.position = start;
		m_shipController.transform.rotation = Quaternion.LookRotation(end - start);

		m_shipController.OnLaunch();

		State = MapState.Play;
	}

	private void OnStateChanged()
	{
		switch (m_state)
		{
			case MapState.Idle:
				break;

			case MapState.Launch:
				{
					SetLaunchZoneActive(true);

					//
					if (m_shipController != null)
						GameObject.Destroy(m_shipController.gameObject);

					m_shipController = null;
				}
				break;

			case MapState.Play:
				SetLaunchZoneActive(false);
				break;

			default:
				Debug.LogError(string.Format("Case for {0} not implemented", m_state));
				break;
		}
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 20), "Restart"))
			Restart();
	}
}

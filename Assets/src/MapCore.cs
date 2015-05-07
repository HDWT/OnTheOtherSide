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
	[SerializeField]
	private List<LaunchZone> m_launchZones = new List<LaunchZone>();

	[SerializeField]
	private ShipController m_shipController = null;

	private MapState m_state = MapState.Idle;

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





	// Use this for initialization
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
		m_shipController.transform.rotation = Quaternion.FromToRotation(start, end);

		State = MapState.Play;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void SetLaunchZoneActive(bool active)
	{
		foreach (LaunchZone zone in m_launchZones)
		{
			if (zone != null)
				zone.gameObject.SetActive(active);
		}
	}

	private void OnStateChanged()
	{
		switch (m_state)
		{
			case MapState.Idle:
				break;

			case MapState.Launch:
				SetLaunchZoneActive(true);
				break;

			case MapState.Play:
				SetLaunchZoneActive(false);
				break;

			default:
				Debug.LogError(string.Format("Case for {0} not implemented", m_state));
				break;
		}
	}
}

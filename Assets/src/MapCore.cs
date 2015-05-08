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
	private string m_nextMap = string.Empty;

	[SerializeField]
	private List<LaunchZone> m_launchZones = new List<LaunchZone>();

	[SerializeField]
	private List<ExitPortal> m_exitPortals = new List<ExitPortal>();

	#endregion

	private MapState m_state = MapState.Idle;
	private SpaceShip m_spaceship = null;

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

		foreach (ExitPortal exitPortal in m_exitPortals)
		{
			if (exitPortal != null)
				exitPortal.OnEnter = OnExitPortal;
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
		Application.LoadLevel(Application.loadedLevelName);
	}

	private void OnLaunch(Vector3 start, Vector3 end)
	{
		if (m_state != MapState.Launch)
		{
			Debug.LogError(string.Format("Launch failed. Current state {0}. Expected state {1}", m_state, MapState.Launch));
			return;
		}

		if (m_spaceship != null)
			GameObject.Destroy(m_spaceship.gameObject);

		m_spaceship = GameObject.Instantiate(CommonSettings.Instance.ShipController);
		m_spaceship.transform.position = start;
		m_spaceship.transform.rotation = Quaternion.LookRotation(end - start);

		m_spaceship.OnDestroy = OnSpaceshipDestroyed;
		m_spaceship.OnLaunch();

		State = MapState.Play;
	}

	private void OnSpaceshipDestroyed()
	{
		if (m_spaceship != null)
			GameObject.Destroy(m_spaceship.gameObject);

		m_spaceship = null;

		Debug.Log("RESTART - Space ship destroyed");
		Restart();
	}

	private void OnExitPortal(GameObject go)
	{
		if ((m_spaceship == null) || (go == null))
			return;

		if (go == m_spaceship.gameObject)
		{
			Debug.Log("EXIT Portal. Next level " + m_nextMap);

			if (!string.IsNullOrEmpty(m_nextMap))
				Application.LoadLevel(m_nextMap);
		}
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
					if (m_spaceship != null)
						GameObject.Destroy(m_spaceship.gameObject);

					m_spaceship = null;
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
		if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 30), "Restart"))
			Restart();

		bool guiEnabled = GUI.enabled;
		GUI.enabled = !string.IsNullOrEmpty(m_nextMap);

		if (GUI.Button(new Rect(Screen.width - 100, 30, 100, 30), "Next"))
		{
			if (!string.IsNullOrEmpty(m_nextMap))
				Application.LoadLevel(m_nextMap);
		}

		GUI.enabled = guiEnabled;

		if (GUI.Button(new Rect(Screen.width - 100, 60, 100, 30), "Quit"))
			Application.Quit();
	}
}

﻿using UnityEngine;
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
	private float m_launchSpeed = 10;

	[SerializeField]
	private float m_drag = 0.1f;

	#endregion

	private Transform m_transform = null;

	private SpaceShipState m_state = SpaceShipState.Idle;
	private float m_currentSpeed = 0;

	void Awake()
	{
		m_transform = GetComponent<Transform>();
	}

	public void OnLaunch()
	{
		m_currentSpeed = m_launchSpeed;
		m_state = SpaceShipState.Moving;
	}

	void Update()
	{
		if (m_state == SpaceShipState.Idle)
		{

		}
		else if (m_state == SpaceShipState.Moving)
		{
			if (m_currentSpeed > 0)
			{
				m_transform.Translate(Vector3.forward * m_currentSpeed * Time.deltaTime);

				m_currentSpeed -= m_drag * Time.deltaTime * 50;
			}
			
			if (m_currentSpeed <= 0)
			{
				m_currentSpeed = 0;
				m_state = SpaceShipState.Idle;
			}
		}
	}

	void OnGUI()
	{
		GUILayout.Label("State: " + m_state);
		GUILayout.Label("Speed: " + m_currentSpeed);
	}
}

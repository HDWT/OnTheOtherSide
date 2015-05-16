using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
	#region --- Serialized Fields ---

	[SerializeField]
	private List<Transform> m_muzzles = new List<Transform>();

	[SerializeField]
	private LineOfSight m_lineOfSight = null;

	[SerializeField]
	private float m_shootsPerSecond = 2;

	[SerializeField]
	private float m_bulletMaxAngle = 2;

	[SerializeField]
	private bool m_oneShot = false;

	#endregion

	private Transform m_transform = null;
	private int m_muzzleIndex = 0;
	private float m_timeLeft = 0;
	private bool m_canShoot = true;

	void Awake()
	{
		m_transform = GetComponent<Transform>();
		m_timeLeft = 1.0f / m_shootsPerSecond;
		
		if (m_lineOfSight != null)
		{
			m_lineOfSight.OnEnter = EnterLineOfSight;
			m_lineOfSight.OnExit = ExitLineOfSighe;
			m_canShoot = false;
		}
	}

	void Update()
	{
		if (m_muzzles.Count == 0)
			return;

		m_timeLeft -= Time.deltaTime;

		if (!m_canShoot || m_timeLeft > 0)
			return;

		if (m_oneShot)
		{
			for (int i = 0; i < m_muzzles.Count; ++i)
				Shoot(m_muzzles[i]);
		}
		else
		{
			Transform muzzle = m_muzzles[m_muzzleIndex];

			Shoot(muzzle);

			if (++m_muzzleIndex >= m_muzzles.Count)
				m_muzzleIndex = 0;
		}

		m_timeLeft = 1.0f / m_shootsPerSecond;
	}

	private void EnterLineOfSight(Collider collider)
	{
		if (collider && LayerUtils.Is(collider.gameObject, LayerType.Player))
			m_canShoot = true;
	}

	private void ExitLineOfSighe(Collider collider)
	{
		if (collider && LayerUtils.Is(collider.gameObject, LayerType.Player))
			m_canShoot = false;
	}

	private void Shoot(Transform muzzle)
	{
		Bullet bullet = GameObject.Instantiate(CommonSettings.Instance.Bullet) as Bullet;

		bullet.transform.position = muzzle.position;
		bullet.transform.rotation = muzzle.rotation;

		float rotation = Random.Range(-m_bulletMaxAngle, m_bulletMaxAngle);

		bullet.transform.Rotate(Vector3.up, rotation);

		SceneFolder.Instance.Add(bullet.gameObject, "Bullets");
	}

	void OnCollisionEnter(Collision collision)
	{
		if (LayerUtils.Is(collision.gameObject, LayerType.Bullet))
		{
			GameObject explosion = GameObject.Instantiate(CommonSettings.Instance.ExplosionEffect) as GameObject;
			explosion.transform.position = m_transform.position;
			explosion.transform.rotation = m_transform.rotation;

			GameObject.Destroy(this.gameObject);
		}
	}
}

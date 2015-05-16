using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	#region --- Serialized Fields ---

	[SerializeField]
	private float m_damage = 1;

	[SerializeField]
	private float m_speed = 10;

	[SerializeField]
	private float m_lifeTime = 2;

	[SerializeField]
	private GravitySense m_gravitySense = null;

	#endregion

	private Transform m_transform = null;
	private Collider m_collider = null;
	private float m_time = 0;
	private float m_disabledColliderTime = 0;

	public float Damage { get { return m_damage; } }

	void Awake()
	{
		m_transform = GetComponent<Transform>();
		m_collider = GetComponent<Collider>();

		DisableCollider(0.1f);
	}

	void Update()
	{
		if (m_collider && !m_collider.enabled && m_disabledColliderTime > 0)
		{
			m_disabledColliderTime -= Time.deltaTime;

			if (m_disabledColliderTime < 0)
				m_collider.enabled = true;
		}

		if (m_gravitySense != null)
			m_gravitySense.Apply(m_transform);

		//
		m_transform.Translate(Vector3.forward * m_speed * Time.deltaTime);

		m_time += Time.deltaTime;

		if (m_time > m_lifeTime)
			GameObject.Destroy(this.gameObject);
	}

	void OnTeleport()
	{

	}

	void OnCollisionEnter(Collision collision)
	{
		GameObject.Destroy(this.gameObject);
	}

	private void DisableCollider(float time)
	{
		if (m_collider != null)
		{
			m_collider.enabled = false;
			m_disabledColliderTime = time;
		}
	}
}

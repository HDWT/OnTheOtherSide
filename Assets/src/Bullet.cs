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

	#endregion

	private Transform m_transform = null;
	private float m_time = 0;

	public float Damage { get { return m_damage; } }

	void Awake()
	{
		m_transform = GetComponent<Transform>();
	}

	void Update()
	{
		m_transform.Translate(Vector3.forward * m_speed * Time.deltaTime);

		m_time += Time.deltaTime;

		if (m_time > m_lifeTime)
			GameObject.Destroy(this.gameObject);
	}

	void OnCollisionEnter(Collision collision)
	{
		GameObject.Destroy(this.gameObject);
	}
}

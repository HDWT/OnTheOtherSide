using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class GravityField : MonoBehaviour
{
	[SerializeField]
	private float m_power = 1;

	private Transform m_transform = null;
	private SphereCollider m_sphereCollider = null;

	public float Radius { get { return m_sphereCollider.radius * m_transform.localScale.x; } }
	public float Power  { get { return m_power; } }

	public Transform cachedTransform
	{
		get
		{
			if (m_transform == null)
				m_transform = GetComponent<Transform>();

			return m_transform;
		}
	}

	void Awake()
	{
		m_transform = GetComponent<Transform>();

		m_sphereCollider = GetComponent<SphereCollider>();
		m_sphereCollider.isTrigger = true;

		LayerUtils.CheckWarning(this.gameObject, LayerType.GravityField);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider)
			collider.SendMessage("OnGravityFieldEnter", this, SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider)
			collider.SendMessage("OnGravityFieldExit", this, SendMessageOptions.DontRequireReceiver);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravitySense : MonoBehaviour
{
	private HashSet<GravityField> m_fields = new HashSet<GravityField>();
	private Transform m_transform = null;

	void Awake()
	{
		m_transform = GetComponent<Transform>();
	}

	void OnDestroy()
	{
		m_fields.Clear();
	}

	public Vector3 GetPower()
	{
		if (m_fields.Count == 0)
			return Vector3.zero;

		Vector3 power = Vector3.zero;

		foreach (var field in m_fields)
		{
			if (field == null)
				continue;

			float distance = field.Radius - Vector3.Distance(m_transform.position, field.cachedTransform.position);

			if (distance < 0)
				continue;

			float v = field.Power * Mathf.Sqrt(distance);
			Vector3 direction = Vector3.Normalize(field.cachedTransform.position - m_transform.position);

			Debug.DrawLine(m_transform.position, m_transform.position + direction * v);

			power += direction * v;
		}

		return power;
	}

	public void Apply(Transform tr)
	{
		if (tr == null)
			return;

		Vector3 gravity = GetPower();
		gravity.y = 0;

		if (gravity != Vector3.zero)
		{
			Quaternion lookRotation = Quaternion.LookRotation(tr.forward + Vector3.Normalize(gravity));
			tr.rotation = Quaternion.Lerp(tr.rotation, lookRotation, Time.deltaTime * Vector3.Magnitude(gravity));

			Debug.DrawLine(tr.position, tr.position + tr.forward + Vector3.Normalize(gravity), Color.green);
		}
	}

	void Update()
	{
		Vector3 power = GetPower();

		Debug.DrawLine(m_transform.position, m_transform.position + power, Color.red);
	}

	private void OnGravityFieldEnter(object field)
	{
		GravityField gravityField = field as GravityField;

		if (gravityField == null)
			return;

		m_fields.Add(gravityField);
	}

	private void OnGravityFieldExit(object field)
	{
		GravityField gravityField = field as GravityField;

		if (gravityField == null)
			return;

		m_fields.Remove(gravityField);
	}
}

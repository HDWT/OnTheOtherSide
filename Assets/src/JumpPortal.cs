using UnityEngine;
using System.Collections;

public class JumpPortal : MonoBehaviour
{
	#region --- Serialized Fields ---

	[SerializeField]
	private JumpPortal m_targetPortal = null;

	[SerializeField]
	private Transform m_exitPoint = null;

	[SerializeField]
	private float m_exitDelay = 0.3f;

	#endregion

	private Transform m_transform = null;

	public bool IsValid { get { return m_targetPortal != null; } }

	void Awake()
	{
		m_transform = GetComponent<Transform>();
	}

	IEnumerator OnTriggerEnter(Collider collider)
	{
		if (!IsValid)
			yield break;

		yield return new WaitForSeconds(m_exitDelay);

		if (collider)
		{
			float angle = 180 + collider.transform.rotation.eulerAngles.y - m_transform.rotation.eulerAngles.y;

			collider.transform.position = m_targetPortal.m_exitPoint.position;
			collider.transform.rotation = m_targetPortal.transform.rotation;
			collider.transform.Rotate(Vector3.up, angle);
		}

		//Debug.Log("On enter " + collider.name + " " + this.name);
	}

	void OnTriggerExit(Collider collider)
	{
		if (!IsValid)
			return;

		//Debug.Log("On exit " + collider.name + " " + this.name);
	}

	#if UNITY_EDITOR

	void OnDrawGizmos()
	{
		if (!IsValid)
			return;

		if (m_transform == null)
			m_transform = GetComponent<Transform>();

		if (UnityEditor.Selection.activeGameObject != this.gameObject)
			return;

		Color gizmosColor = Gizmos.color;
		Gizmos.color = Color.green;

		Gizmos.DrawLine(m_transform.position, m_targetPortal.m_exitPoint.position);
		Gizmos.color = gizmosColor;
	}

	#endif
}

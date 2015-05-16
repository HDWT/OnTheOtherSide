using UnityEngine;
using System.Collections;

public class JumpPortal : MonoBehaviour
{
	#region --- Serialized Fields ---

	[SerializeField]
	private JumpPortal m_targetPortal = null;

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

		if (m_exitDelay > 0)
			yield return new WaitForSeconds(m_exitDelay);

		if (collider)
		{
			bool moveInPortal = Vector3.Dot(collider.transform.forward, -transform.forward) > 0;

			if (moveInPortal)
			{
				Transform objTransform = collider.GetComponent<Transform>();

				float angle = 180 + objTransform.rotation.eulerAngles.y - m_transform.rotation.eulerAngles.y;

				objTransform.rotation = m_targetPortal.transform.rotation;
				objTransform.Rotate(Vector3.up, angle);

				//
				Vector3 exitPoint = m_transform.InverseTransformVector(objTransform.position - m_transform.position);

				objTransform.position = m_targetPortal.m_transform.position + m_targetPortal.m_transform.forward * exitPoint.z + m_targetPortal.transform.right * exitPoint.x;

				collider.SendMessage("OnTeleport", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (!IsValid)
			return;
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

		Gizmos.DrawLine(m_transform.position, m_targetPortal.transform.position);
		Gizmos.color = gizmosColor;
	}

	#endif
}

using UnityEngine;
using System.Collections;

public class ExitPortal : MonoBehaviour
{
	[SerializeField]
	private float m_exitDelay = 0.3f;

	public delegate void OnEnterHandler(GameObject go);
	public OnEnterHandler OnEnter = null;

	IEnumerator OnTriggerEnter(Collider collider)
	{
		yield return new WaitForSeconds(m_exitDelay);

		if (OnEnter != null)
			OnEnter(collider.gameObject);
	}

	void OnTriggerExit(Collider collider)
	{

	}
}

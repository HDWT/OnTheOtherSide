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
		if (collider && LayerUtils.Is(collider.gameObject, LayerType.Player))
			collider.enabled = false;

		yield return new WaitForSeconds(m_exitDelay);

		if ((OnEnter != null) && (collider != null))
		{
			OnEnter(collider.gameObject);
		}
	}

	void OnTriggerExit(Collider collider)
	{

	}
}

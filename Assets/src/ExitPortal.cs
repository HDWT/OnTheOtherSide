using UnityEngine;
using System.Collections;

public class ExitPortal : MonoBehaviour
{
	public delegate void OnEnterHandler(GameObject go);
	public OnEnterHandler OnEnter = null;

	void OnTriggerEnter(Collider collider)
	{
		if (OnEnter != null)
			OnEnter(collider.gameObject);
	}

	void OnTriggerExit(Collider collider)
	{

	}
}

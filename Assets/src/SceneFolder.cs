using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneFolder : MonoBehaviour
{
	private static SceneFolder m_instance = null;
	public static SceneFolder Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = new GameObject("Scene Folder").AddComponent<SceneFolder>();
				m_instance.transform.position = Vector3.zero;
				m_instance.transform.rotation = Quaternion.identity;
			}

			return m_instance;
		}
	}

	private Dictionary<string, Transform> m_folders = new Dictionary<string, Transform>();

	public void Add(GameObject go, string name)
	{
		if (go == null || string.IsNullOrEmpty(name))
			return;

		Transform folder = null;

		if (m_folders.TryGetValue(name, out folder))
		{
			go.transform.parent = folder;
		}
		else
		{
			folder = new GameObject(name).GetComponent<Transform>();
			m_folders[name] = folder;

			go.transform.parent = folder;
		}
	}

	public void DestroyFolder(string name)
	{
		Transform folder = null;

		if (m_folders.TryGetValue(name, out folder))
		{
			GameObject.Destroy(folder.gameObject);
			m_folders.Remove(name);
		}
	}
}

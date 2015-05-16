using UnityEngine;

public static class GameObjectUtils
{
	public static T GetOrAddComponent<T>(GameObject go) where T : Component
	{
		if (go == null)
			return null;

		T component = go.GetComponent<T>();

		if (component == null)
			component = go.AddComponent<T>();

		return component;
	}
}

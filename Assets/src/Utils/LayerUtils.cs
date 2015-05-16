using UnityEngine;

public enum LayerType
{
	Player = 8,
	Portal = 9,
	Bullet = 11,
	GravityField = 13,
	GravitySense = 14,
}

public static class LayerUtils
{
	public static bool Is(GameObject go, LayerType layer)
	{
		return (go != null) ? (go.layer == (int)layer) : (false);
	}

	public static void CheckWarning(GameObject go, LayerType layer)
	{
		if ((go == null) || (go.layer == (int)layer))
			return;

		string fullName = string.Empty;
		Transform tr = go.transform;

		while (tr != null)
		{
			fullName = (fullName == string.Empty) ? go.name : string.Format("{0}/{1}", tr.name, fullName);
			tr = tr.parent;
		}

		Debug.LogWarning(string.Format("[{0}] has layer '{1}'. Expected layer '{2}'", fullName, LayerMask.LayerToName(go.layer), LayerMask.LayerToName((int)layer)));
	}
}

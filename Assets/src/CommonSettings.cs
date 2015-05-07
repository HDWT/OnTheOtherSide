using UnityEngine;
using System.Collections;

public class CommonSettings : ScriptableObject
{
	private static CommonSettings m_instance = null;
	public static CommonSettings Instance
	{
		get
		{
			if (m_instance == null)
			{
				if (Resources.Load("serialized/common_settings") == null)
					Debug.LogError("Load 'common_settings' failed");
			}

			return m_instance;
		}
	}

	#region --- Serialized Fields ---

	[SerializeField]
	private ShipController m_shipController = null;

	#endregion

	public ShipController ShipController { get { return m_shipController; } }

	void OnEnable()
	{
		m_instance = this;
	}

	#if UNITY_EDITOR

	[UnityEditor.MenuItem("Assets/Create/CommonSettings")]
	public static void CreateAsset()
	{
		CommonSettings asset = ScriptableObject.CreateInstance<CommonSettings>();

		string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/resources/serialized/common_settings.asset");

		UnityEditor.AssetDatabase.CreateAsset(asset, assetPathAndName);
		UnityEditor.AssetDatabase.SaveAssets();
		UnityEditor.AssetDatabase.Refresh();
	}

	#endif
}

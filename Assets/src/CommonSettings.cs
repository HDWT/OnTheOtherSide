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
	private SpaceShip m_shipController = null;

	[SerializeField]
	private Bullet m_bullet = null;

	[SerializeField]
	private GameObject m_explosionEffect = null;

	#endregion

	public SpaceShip	ShipController	{ get { return m_shipController; } }
	public Bullet		Bullet			{ get { return m_bullet; } }
	public GameObject	ExplosionEffect { get { return m_explosionEffect; } }

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

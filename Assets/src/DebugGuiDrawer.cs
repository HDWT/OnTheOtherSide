using UnityEngine;

public static class DebugGuiDrawer
{
	public const int DefaultWidth = 960;
	public const int DefaultHeight = 540;

	private const float MinRatio = 0.5f;
	private const float MaxRatio = 3.0f;

	static bool s_heightRatioOnly = true;
	static float s_customScale = 1;

	#region --- GuiStyle Settings ---

	// Box style
	static readonly GUIStyle BoxStyle = new GUIStyle(GUI.skin.box);
	const int DefaultBoxFontSize = 14;

	private static float ScaledBoxFontSize
	{
		get { return DefaultBoxFontSize * ScaleRatio; }
	}

	// Button style
	static readonly GUIStyle ButtonStyle = new GUIStyle(GUI.skin.button);
	const int DefaultButtonFontSize = 14;

	private static float ScaledButtonFontSize
	{
		get { return DefaultButtonFontSize * ScaleRatio; }
	}

	// Label style
	public static readonly GUIStyle LabelStyle = new GUIStyle(GUI.skin.label);
	const int DefaultLabelFontSize = 14;

	private static float ScaledLabelFontSize
	{
		get { return DefaultLabelFontSize * ScaleRatio; }
	}

	// TextArea style
	static readonly GUIStyle TextAreaStyle = new GUIStyle(GUI.skin.textArea);
	const int DefaultTextAreaFontSize = 14;

	private static float ScaledTextAreaFontSize
	{
		get { return DefaultTextAreaFontSize * ScaleRatio; }
	}

	// TextField style
	static readonly GUIStyle TextFieldStyle = new GUIStyle(GUI.skin.textField);
	const int DefaultTextFieldFontSize = 14;

	private static float ScaledTextFieldFontSize
	{
		get { return DefaultTextFieldFontSize * ScaleRatio; }
	}

	#endregion

	public static float Scale
	{
		get { return s_customScale; }
		set { s_customScale = Mathf.Clamp(value, 0, 10); }
	}

	public static float ScaleRatio
	{
		get
		{
			float widthRatio = (float)Screen.width / (float)DefaultWidth;
			float heightRatio = (float)Screen.height / (float)DefaultHeight;
			float ratio = (s_heightRatioOnly || heightRatio > widthRatio) ? heightRatio : widthRatio;

			return Mathf.Clamp(ratio, MinRatio, MaxRatio);
		}
	}

	public static void Box(Rect position, string text, TextAnchor anchor = TextAnchor.UpperLeft)
	{
		Box(position, text, string.Empty, anchor);
	}

	/// <summary> </summary>
	public static void Box(Rect position, string text, string tooltip, TextAnchor anchor = TextAnchor.UpperLeft)
	{
		ScalePosition(ref position, anchor);
		ApplyCustomScale(ref position, s_customScale);
		BoxStyle.fontSize = Mathf.FloorToInt(ScaledBoxFontSize * s_customScale);

		if ((position.width < 1) || (position.height < 1))
			return;

		if (string.IsNullOrEmpty(tooltip))
			GUI.Box(position, text, BoxStyle);
		else
			GUI.Box(position, new GUIContent(text, tooltip), BoxStyle);
	}

	public static bool Button(Rect position, string text, TextAnchor anchor = TextAnchor.UpperLeft)
	{
		return Button(position, text, string.Empty, anchor);
	}

	/// <summary> </summary>
	public static bool Button(Rect position, string text, string tooltip, TextAnchor anchor = TextAnchor.UpperLeft)
	{
		ScalePosition(ref position, anchor);
		ApplyCustomScale(ref position, s_customScale);
		ButtonStyle.fontSize = Mathf.FloorToInt(ScaledButtonFontSize * s_customScale);

		if ((position.width < 1) || (position.height < 1))
			return false;

		if (string.IsNullOrEmpty(tooltip))
			return GUI.Button(position, text, ButtonStyle);
		else
			return GUI.Button(position, new GUIContent(text, tooltip), ButtonStyle);
	}

	/*public static void Box(Rect position, string text, string tooltip, TextAnchor anchor = TextAnchor.UpperLeft)
	{
		ScalePosition(ref position, anchor);
		ApplyCustomScale(ref position, s_customScale);
		ButtonStyle.fontSize = Mathf.FloorToInt(ScaledButtonFontSize * s_customScale);

		if ((position.width < 1) || (position.height < 1))
			return;

		if (string.IsNullOrEmpty(tooltip))
			GUI.Box(position, text, ButtonStyle);
		else
			GUI.Box(position, new GUIContent(text, tooltip), ButtonStyle);
	}*/

	public static void Label(Rect position, string text, TextAnchor anchor = TextAnchor.UpperLeft, TextAnchor pivot = TextAnchor.UpperLeft)
	{
		Label(position, text, string.Empty, anchor, pivot);
	}

	/// <summary> </summary>
	public static void Label(Rect position, string text, string tooltip, TextAnchor anchor = TextAnchor.UpperLeft, TextAnchor pivot = TextAnchor.UpperLeft)
	{
		ScalePosition(ref position, anchor);
		ApplyCustomScale(ref position, s_customScale); 
		LabelStyle.fontSize = Mathf.FloorToInt(ScaledLabelFontSize * s_customScale);
		LabelStyle.alignment = pivot;

		if ((position.width < 1) || (position.height < 1))
			return;
		
		if (string.IsNullOrEmpty(tooltip))
			GUI.Label(position, text, LabelStyle);
		else
			GUI.Label(position, new GUIContent(text, tooltip), LabelStyle);
	}

	/// <summary> </summary>
	public static string TextArea(Rect position, string text, TextAnchor anchor = TextAnchor.UpperLeft)
	{
		return TextArea(position, text, -1, anchor);
	}

	public static string TextArea(Rect position, string text, int maxLength, TextAnchor anchor = TextAnchor.UpperLeft)
	{
		ScalePosition(ref position, anchor);
		ApplyCustomScale(ref position, s_customScale);
		TextAreaStyle.fontSize =  Mathf.FloorToInt(ScaledTextAreaFontSize * s_customScale);

		if ((position.width < 1) || (position.height < 1))
			return text;

		if (maxLength < 0)
			return GUI.TextArea(position, text, TextAreaStyle);
		else
			return GUI.TextArea(position, text, maxLength, TextAreaStyle);
	}

	public static string TextField(Rect position, string text, TextAnchor anchor = TextAnchor.UpperLeft)
	{
		return TextField(position, text, -1, anchor);
	}

	/// <summary> </summary>
	public static string TextField(Rect position, string text, int maxLength, TextAnchor anchor = TextAnchor.UpperLeft)
	{
		ScalePosition(ref position, anchor);
		ApplyCustomScale(ref position, s_customScale);
		TextFieldStyle.fontSize = Mathf.FloorToInt(ScaledTextFieldFontSize * s_customScale);

		if ((position.width < 1) || (position.height < 1))
			return text;

		if (maxLength < 0)
			return GUI.TextField(position, text, TextFieldStyle);
		else
			return GUI.TextField(position, text, maxLength, TextFieldStyle);
	}

	private static void ScalePosition(ref Rect position, TextAnchor anchor)
	{
		switch (anchor)
		{
			case TextAnchor.UpperLeft:
				position.x = position.x * ScaleRatio;
				position.y = position.y * ScaleRatio;
				break;

			case TextAnchor.UpperCenter:
				position.x = Screen.width / 2.0f - (position.width * ScaleRatio) / 2.0f  + position.x * ScaleRatio;
				position.y = position.y * ScaleRatio;
				break;

			case TextAnchor.UpperRight:
				position.x = Screen.width - position.x * ScaleRatio - position.width * ScaleRatio;
				position.y = position.y * ScaleRatio;
				break;

			case TextAnchor.LowerLeft:
				position.x = position.x * ScaleRatio;
				position.y = Screen.height - position.y * ScaleRatio - position.height * ScaleRatio;
				break;

			case TextAnchor.LowerCenter:
				position.x = Screen.width / 2.0f - (position.width * ScaleRatio) / 2.0f  + position.x * ScaleRatio;
				position.y = Screen.height - position.y * ScaleRatio - position.height * ScaleRatio;
				break;

			case TextAnchor.LowerRight:
				position.x = Screen.width - position.x * ScaleRatio - position.width * ScaleRatio;
				position.y = Screen.height - position.y * ScaleRatio - position.height * ScaleRatio;
				break;

			case TextAnchor.MiddleLeft:
				position.x = position.x * ScaleRatio;
				position.y = Screen.height / 2.0f - (position.height * ScaleRatio) / 2.0f + position.y * ScaleRatio;
				break;

			case TextAnchor.MiddleCenter:
				position.x = Screen.width / 2.0f - (position.width * ScaleRatio) / 2.0f + position.x * ScaleRatio;
				position.y = Screen.height / 2.0f - (position.height * ScaleRatio) / 2.0f + position.y * ScaleRatio;
				break;

			case TextAnchor.MiddleRight:
				position.x = Screen.width - position.x * ScaleRatio - position.width * ScaleRatio;
				position.y = Screen.height / 2.0f - (position.height * ScaleRatio) / 2.0f + position.y * ScaleRatio;
				break;
		}

		position.width *= ScaleRatio;
		position.height *= ScaleRatio;
	}

	private static void ApplyCustomScale(ref Rect position, float scale)
	{
		if (scale < 0.99999f || scale > 1.00001f)
		{
			float dWidth = position.width - position.width * scale;
			float dHeight = position.height - position.height * scale;

			position.x += dWidth / 2.0f;
			position.y += dHeight / 2.0f;
			position.width -= dWidth;
			position.height -= dHeight;
		}
	}
}


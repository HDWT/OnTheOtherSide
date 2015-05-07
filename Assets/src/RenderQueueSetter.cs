using UnityEngine;
using System.Collections;

public class RenderQueueSetter : MonoBehaviour
{
	[SerializeField]
	private int m_value = 3000;

	private Renderer m_renderer = null;
	private int m_lastValue = 0;

	void OnEnable()
	{
		if (m_renderer == null)
			m_renderer = GetComponent<Renderer>();

		if ((m_renderer != null) && (m_renderer.sharedMaterial != null))
			m_lastValue = m_renderer.sharedMaterial.renderQueue;
	}

	void Update()
	{
		if ((m_renderer != null) && (m_renderer.sharedMaterial != null))
		{
			if (m_value != m_lastValue)
				m_renderer.sharedMaterial.renderQueue = m_value;

			m_lastValue = m_value;
		}
	}
}


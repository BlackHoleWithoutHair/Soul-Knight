using UnityEngine;
public class PanelGemStore1 : IPanelGemStore
{
    public PanelGemStore1(IPanel parent) : base(parent)
    {
        m_GameObject = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPanel("PanelGemStore"));
        m_GameObject.name = GetType().Name;
        m_GameObject.transform.SetParent(UnityTool.Instance.GetMainCanvas().transform);
        m_GameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        m_GameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        m_GameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        m_GameObject.SetActive(false);
    }
}

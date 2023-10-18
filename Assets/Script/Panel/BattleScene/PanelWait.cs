using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelWait : IPanel
{
    private TextMeshProUGUI Tip;
    private List<string> tips;
    public PanelWait(IPanel panel) : base(panel)
    {
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
    }
    protected override void OnInit()
    {
        base.OnInit();
        Tip = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "Text");
        tips = ProxyResourceFactory.Instance.Factory.GetScriptableObject<TipScriptableObject>().tips;
        EventCenter.Instance.RegisterObserver(EventType.OnFinishRoomGenerate, () =>
        {
            OnExit();
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        Tip.text = tips[Random.Range(0, tips.Count)];
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

}

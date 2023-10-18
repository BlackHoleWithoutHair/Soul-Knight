using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IPanelGemStore : IPanel
{
    private Button ButtonBack;
    public IPanelGemStore(IPanel parent) : base(parent)
    {
        isShowPanelAfterExit = true;
    }
    protected override void OnInit()
    {
        base.OnInit();
        ButtonBack = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonBack");
        m_GameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1080);
        ButtonBack.onClick.AddListener(() =>
        {
            OnExit();
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        m_GameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 0.3f);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
        m_GameObject.GetComponent<RectTransform>().DOAnchorPosY(1080, 0.3f);
    }
}

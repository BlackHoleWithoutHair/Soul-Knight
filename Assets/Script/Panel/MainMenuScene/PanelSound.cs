using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelSound : IPanel
{
    private GameObject Title;
    private GameObject DivContent;
    private Button ButtonClose;
    private Toggle ToggleMute;
    public PanelSound(IPanel parent) : base(parent)
    {
        isShowPanelAfterExit = true;
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
    }
    protected override void OnInit()
    {
        base.OnInit();
        Title = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "Title");
        DivContent = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivContent");
        ButtonClose = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonClose");
        ToggleMute = UnityTool.Instance.GetComponentFromChild<Toggle>(m_GameObject, "ToggleMute");
        DivContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(1920, 0);
        ButtonClose.onClick.AddListener(() =>
        {
            OnExit();
        });

    }
    protected override void OnEnter()
    {
        base.OnEnter();
        Title.SetActive(true);
        ButtonClose.gameObject.SetActive(true);
        ToggleMute.Select();
        DivContent.GetComponent<RectTransform>().DOAnchorPosX(0, 0.3f);
    }
    public override void OnExit()
    {
        base.OnExit();
        Title.SetActive(false);
        ButtonClose.gameObject.SetActive(false);
        DivContent.GetComponent<RectTransform>().DOAnchorPosX(1920, 0.3f);
    }


}

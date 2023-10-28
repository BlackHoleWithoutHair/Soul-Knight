using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelLuckyCat : IPanel
{
    public PanelLuckyCat(IPanel parent) : base(parent)
    {
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        isShowPanelAfterExit = true;
    }
    protected override void OnInit()
    {
        base.OnInit();
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1080);
        UnityTool.Instance.GetComponentFromChild<Button>(gameObject, "ButtonBack").onClick.AddListener(() =>
        {
            OnExit();
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        AppearAnim();
    }
    public override void OnExit()
    {
        base.OnExit();
        DisAppearAnim();
    }
    private void AppearAnim()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 0.3f);
    }
    private void DisAppearAnim()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(1080, 0.3f);
    }

}

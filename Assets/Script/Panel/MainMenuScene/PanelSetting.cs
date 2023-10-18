using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PanelSetting : IPanel
{
    private GameObject Title;
    private GameObject DivContent;
    private Button ButtonClose;
    private Button ButtonKey;
    private Button ButtonVideo;
    private Button ButtonSound;
    public PanelSetting(IPanel parent) : base(parent)
    {
        isShowPanelAfterExit = true;
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        children.Add(new PanelSound(this));
    }
    protected override void OnInit()
    {
        base.OnInit();
        Title = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "Title");
        DivContent = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivContent");
        ButtonClose = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonClose");
        ButtonKey = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonKey");
        ButtonVideo = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonVideo");
        ButtonSound = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonSound");
        m_GameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1080);
        ButtonSound.onClick.AddListener(() =>
        {
            EnterPanel(typeof(PanelSound));
        });
        ButtonClose.onClick.AddListener(() =>
        {
            OnExit();
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        ButtonKey.Select();
        m_GameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 0.3f);
        DivContent.GetComponent<RectTransform>().DOAnchorPosX(0, 0.3f);
        Title.SetActive(true);
        ButtonClose.gameObject.SetActive(true);
    }
    public override void OnExit()
    {
        base.OnExit();
        m_GameObject.GetComponent<RectTransform>().DOAnchorPosY(1080, 0.3f);
    }
    protected override void EnterPanel(Type type)
    {
        base.EnterPanel(type);
        Title.SetActive(false);
        ButtonClose.gameObject.SetActive(false);
        DivContent.GetComponent<RectTransform>().DOAnchorPosX(-1920, 0.3f);
    }
}

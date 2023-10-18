using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelOnlineAlert : IPanel
{
    private Button ButtonClose;
    private Button ButtonCreateRoom;
    private Button ButtonJoinRoom;
    private CanvasGroup m_CanvasGroup;
    public PanelOnlineAlert(IPanel parent) : base(parent)
    {
        isShowPanelAfterExit = true;
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        children.Add(new PanelRoomList(this));
        children.Add(new PanelCreateRoom(this));
    }
    protected override void OnInit()
    {
        base.OnInit();
        m_CanvasGroup = m_GameObject.GetComponent<CanvasGroup>();
        m_CanvasGroup.alpha = 0f;
        ButtonClose = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonClose");
        ButtonCreateRoom = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonCreateRoom");
        ButtonJoinRoom = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonJoinRoom");
        ButtonClose.onClick.AddListener(() =>
        {
            OnExit();
        });
        ButtonCreateRoom.onClick.AddListener(() =>
        {
            EnterPanel(typeof(PanelCreateRoom));
        });
        ButtonJoinRoom.onClick.AddListener(() =>
        {
            EnterPanel(typeof(PanelRoomList));
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        m_CanvasGroup.DOFade(1, 0.3f);
    }
    public override void OnExit()
    {
        base.OnExit();
        m_CanvasGroup.DOFade(0, 0.3f).OnComplete(() =>
        {
            m_GameObject.SetActive(false);
        });
    }
}

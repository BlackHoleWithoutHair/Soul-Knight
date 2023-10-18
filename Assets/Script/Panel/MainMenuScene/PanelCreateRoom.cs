using SoulKnightProtocol;
using TMPro;
using UnityEngine.UI;

public class PanelCreateRoom : IPanel
{
    private TMP_InputField InputName;
    private TMP_InputField InputMaxNum;
    private Button ButtonStart;
    private Button ButtonBack;
    private bool isCreateRoomResponse;
    public PanelCreateRoom(IPanel panel) : base(panel)
    {
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
    }
    protected override void OnInit()
    {
        base.OnInit();
        InputName = UnityTool.Instance.GetComponentFromChild<TMP_InputField>(m_GameObject, "InputFieldRoomName");
        InputMaxNum = UnityTool.Instance.GetComponentFromChild<TMP_InputField>(m_GameObject, "InputFieldMaxNum");
        ButtonStart = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonStart");
        ButtonBack = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonBack");
        ButtonBack.onClick.AddListener(() =>
        {
            OnExit();
        });
        ButtonStart.onClick.AddListener(() =>
        {
            if (InputName.text.Length != 0 && InputMaxNum.text.Length != 0)
            {
                (ClientFacade.Instance.GetRequest(ActionCode.CreateRoom) as RequestCreateRoom).SendRequest(InputName.text, int.Parse(InputMaxNum.text), (pack) =>
                {
                    if (pack.ReturnCode == ReturnCode.Fail)
                    {
                        EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "创建房间失败");
                    }
                    if (pack.ReturnCode == ReturnCode.Success)
                    {
                        EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "创建房间成功");
                        isCreateRoomResponse = true;
                    }
                });
            }
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (isCreateRoomResponse)
        {
            isCreateRoomResponse = false;
            MemoryModelCommand.Instance.EnterOnlineMode();
        }
    }
}

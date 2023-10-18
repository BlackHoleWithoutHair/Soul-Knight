using SoulKnightProtocol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelUserList : IPanel
{
    private Button ButtonBack;
    private Button ButtonExit;
    private Button ButtonStart;
    private GameObject UserItem;
    private MainPack ResponsePack;
    private bool isFindPlayerResponse;
    private bool isExitRoomResponse;
    public PanelUserList(IPanel parent) : base(parent)
    {
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        EventCenter.Instance.RegisterObserver<MainPack>(EventType.OnFindPlayerResponse, (pack) =>
        {
            isFindPlayerResponse = true;
            ResponsePack = pack;
        });
    }
    protected override void OnInit()
    {
        base.OnInit();
        UserItem = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "UserItem");
        ButtonExit = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonExit");
        ButtonBack = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonBack");
        ButtonStart = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonStart");
        ButtonBack.onClick.AddListener(() =>
        {
            OnExit();
        });
        ButtonStart.onClick.AddListener(() =>
        {
            MemoryModelCommand.Instance.EnterOnlineMode();
        });
        ButtonExit.onClick.AddListener(() =>
        {
            (ClientFacade.Instance.GetRequest(ActionCode.ExitRoom) as RequestExitRoom).SendRequest((pack) =>
            {
                if (pack.ReturnCode == ReturnCode.Success)
                {
                    isExitRoomResponse = true;
                }
                if (pack.ReturnCode == ReturnCode.Fail)
                {
                    EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "ÍË³ö·¿¼äÊ§°Ü");
                }
            });
        });
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (isFindPlayerResponse)
        {
            isFindPlayerResponse = false;
            UserItem.SetActive(false);
            UnityTool.Instance.DestroyChildrenExceptFirstChild(UserItem.transform.parent);
            for (int i = 0; i < ResponsePack.RoomPacks[0].PlayerPacks.Count; i++)
            {
                if (i == 0)
                {
                    SetUserItemInfo(UserItem, ResponsePack.RoomPacks[0].PlayerPacks[i]);
                    UserItem.SetActive(true);
                }
                else
                {
                    GameObject obj = Object.Instantiate(UserItem, UserItem.transform.parent);
                    SetUserItemInfo(obj, ResponsePack.RoomPacks[0].PlayerPacks[i]);
                }
            }
        }
        if (isExitRoomResponse)
        {
            isExitRoomResponse = false;
            OnExit();
        }
    }
    private void SetUserItemInfo(GameObject obj, PlayerPack pack)
    {
        obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pack.PlayerName;
    }
}

using DG.Tweening;
using SoulKnightProtocol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelRoomList : IPanel
{
    private Button ButtonRoomItem;
    private Button ButtonBack;
    private Button ButtonSearch;
    private bool isFindRoomResponse;
    private bool isJoinRoomResponse;
    private MainPack pack;
    public PanelRoomList(IPanel parent) : base(parent)
    {
        isShowPanelAfterExit = true;
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        children.Add(new PanelUserList(this));
    }
    protected override void OnInit()
    {
        base.OnInit();
        m_GameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1080);
        ButtonRoomItem = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "RoomItem");
        ButtonSearch = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonSearch");
        ButtonBack = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonBack");
        ButtonBack.onClick.AddListener(() =>
        {
            OnExit();
        });
        ButtonSearch.onClick.AddListener(() =>
        {
            (ClientFacade.Instance.GetRequest(ActionCode.FindRoom) as RequestFindRoom).SendRequest((pack) =>
            {
                if (pack.ReturnCode == ReturnCode.Fail)
                {
                    EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "查询房间失败");
                }
                if (pack.ReturnCode == ReturnCode.NoRoom)
                {
                    EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "当前没有房间");
                }
                if (pack.ReturnCode == ReturnCode.Success)
                {
                    isFindRoomResponse = true;
                    this.pack = pack;
                }
            });
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
        if (isFindRoomResponse)
        {
            UnityTool.Instance.DestroyChildrenExceptFirstChild(ButtonRoomItem.transform.parent);
            if (pack.RoomPacks.Count == 0)
            {
                ButtonRoomItem.gameObject.SetActive(false);
            }
            for (int i = 0; i < pack.RoomPacks.Count; i++)
            {
                string RoomName = pack.RoomPacks[i].RoomName;
                if (i == 0)
                {
                    SetRoomItemInfo(ButtonRoomItem.gameObject, pack.RoomPacks[i]);
                    ButtonRoomItem.gameObject.SetActive(true);
                    ButtonRoomItem.onClick.AddListener(() =>
                    {
                        if (UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(ButtonRoomItem.gameObject, "TextState").text.CompareTo("等待加入") == 0)
                        {
                            (ClientFacade.Instance.GetRequest(ActionCode.JoinRoom) as RequestJoinRoom).SendRequest(RoomName, (pack) =>
                            {
                                if (pack.ReturnCode == ReturnCode.Success)
                                {
                                    isJoinRoomResponse = true;
                                }
                            });
                        }
                    });
                }
                else
                {
                    GameObject obj = Object.Instantiate(ButtonRoomItem.gameObject, ButtonRoomItem.transform.parent);
                    SetRoomItemInfo(ButtonRoomItem.gameObject, pack.RoomPacks[i]);
                    obj.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        if (UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(obj, "TextState").text.CompareTo("等待加入") == 0)
                        {
                            (ClientFacade.Instance.GetRequest(ActionCode.JoinRoom) as RequestJoinRoom).SendRequest(RoomName, (pack) =>
                            {
                                if (pack.ReturnCode == ReturnCode.Success)
                                {
                                    isJoinRoomResponse = true;
                                }

                            });
                        }
                    });
                }
            }
            isFindRoomResponse = false;
        }
        if (isJoinRoomResponse)
        {
            isJoinRoomResponse = false;
            MemoryModelCommand.Instance.EnterOnlineMode();
        }
    }
    public override void OnExit()
    {
        base.OnExit();
        m_GameObject.GetComponent<RectTransform>().DOAnchorPosY(1080, 0.3f);
    }

    private void SetRoomItemInfo(GameObject obj, RoomPack pack)
    {
        string text = "";

        obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pack.RoomName;
        UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(obj, "TextPlayerNum").text = pack.CurrentNum.ToString() + "/" + pack.MaxNum.ToString();
        switch (pack.RoomCode)
        {
            case RoomCode.WaitForJoin:
                text = "等待加入";
                break;
            case RoomCode.Playing:
                text = "游戏中";
                break;
            case RoomCode.Full:
                text = "房间已满";
                break;
        }
        UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(obj, "TextState").text = text;
    }
}

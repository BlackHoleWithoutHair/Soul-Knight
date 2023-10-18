using SoulKnightProtocol;
using UnityEngine;
using UnityEngine.Events;

public class RequestJoinRoom : BaseRequest
{
    public RequestJoinRoom(RequestManager manager) : base(manager)
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
    }
    public override void OnResponse(MainPack pack)
    {
        if (pack.ReturnCode == ReturnCode.Success)
        {
            Debug.Log("成功加入房间");
        }
        if (pack.ReturnCode == ReturnCode.Fail)
        {
            Debug.Log("加入房间失败");
        }
        ReceiveAction?.Invoke(pack);
    }
    public void SendRequest(string roomName, UnityAction<MainPack> receiveAction)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        RoomPack roomPack = new RoomPack();
        roomPack.RoomName = roomName;
        pack.RoomPacks.Add(roomPack);
        base.SendRequest(pack);
        ReceiveAction = receiveAction;
    }
}

using SoulKnightProtocol;
using UnityEngine;
using UnityEngine.Events;

public class RequestCreateRoom : BaseRequest
{

    public RequestCreateRoom(RequestManager manager) : base(manager)
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
    }
    public override void OnResponse(MainPack pack)
    {
        if (pack.ReturnCode == ReturnCode.Success)
        {
            Debug.Log("成功创建房间");
        }
        if (pack.ReturnCode == ReturnCode.Fail)
        {
            Debug.Log("创建房间失败");
        }
        ReceiveAction?.Invoke(pack);
        ReceiveAction = null;
    }
    public void SendRequest(string name, int maxNum, UnityAction<MainPack> receiveAction = null)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        RoomPack roomPack = new RoomPack();
        roomPack.RoomName = name;
        roomPack.MaxNum = maxNum;
        pack.RoomPacks.Add(roomPack);
        base.SendRequest(pack);
        ReceiveAction = receiveAction;
    }
}

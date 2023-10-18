using SoulKnightProtocol;
using UnityEngine;
using UnityEngine.Events;

public class RequestFindRoom : BaseRequest
{

    public RequestFindRoom(RequestManager manager) : base(manager)
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.FindRoom;
    }
    public override void OnResponse(MainPack pack)
    {
        if (pack.ReturnCode == ReturnCode.Success)
        {
            Debug.Log("成功查询房间");
        }
        if (pack.ReturnCode == ReturnCode.Fail)
        {
            Debug.Log("查询房间失败");
        }
        if (pack.ReturnCode == ReturnCode.NoRoom)
        {
            Debug.Log("没有查询到房间");
        }
        ReceiveAction?.Invoke(pack);
        ReceiveAction = null;
    }
    public void SendRequest(UnityAction<MainPack> receiveAction = null)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        base.SendRequest(pack);
        ReceiveAction = receiveAction;
    }
}

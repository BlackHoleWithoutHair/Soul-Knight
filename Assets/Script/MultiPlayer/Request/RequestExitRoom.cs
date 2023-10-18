using SoulKnightProtocol;
using UnityEngine;
using UnityEngine.Events;

public class RequestExitRoom : BaseRequest
{
    public RequestExitRoom(RequestManager manager) : base(manager)
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ExitRoom;
    }
    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
        if (pack.ReturnCode == ReturnCode.Success)
        {
            Debug.Log("成功退出房间");

        }
        if (pack.ReturnCode == ReturnCode.Fail)
        {
            Debug.Log("退出房间失败");
        }
        (ClientFacade.Instance.GetRequest(ActionCode.FindRoom) as RequestFindRoom).SendRequest();
        ReceiveAction?.Invoke(pack);
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

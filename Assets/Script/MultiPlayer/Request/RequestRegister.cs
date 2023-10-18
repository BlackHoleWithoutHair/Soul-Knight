using SoulKnightProtocol;
using UnityEngine;
using UnityEngine.Events;

public class RequestRegister : BaseRequest
{
    public RequestRegister(RequestManager manager) : base(manager)
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;
    }
    public override void OnResponse(MainPack pack)
    {
        if (pack.ReturnCode == ReturnCode.Success)
        {
            Debug.Log("注册成功");
        }
        else if (pack.ReturnCode == ReturnCode.Fail)
        {
            Debug.Log("注册失败");
        }
        ReceiveAction?.Invoke(pack);
        ReceiveAction = null;
    }
    public void SendRequest(string username, string password, UnityAction<MainPack> receiveAction)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        LoginPack loginPack = new LoginPack();
        loginPack.UserName = username;
        loginPack.Password = password;
        pack.LoginPack = loginPack;
        base.SendRequest(pack);
        ReceiveAction = receiveAction;
    }
}

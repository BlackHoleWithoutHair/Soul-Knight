using SoulKnightProtocol;
using UnityEngine;
using UnityEngine.Events;

public class RequestLogin : BaseRequest
{
    public RequestLogin(RequestManager manager) : base(manager)
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
    }
    public void SendRequest(string username, string password, UnityAction<MainPack> receiveAction)
    {
        MainPack pack = new MainPack();
        LoginPack loginPack = new LoginPack();
        loginPack.UserName = username;
        loginPack.Password = password;
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.LoginPack = loginPack;
        base.SendRequest(pack);
        ReceiveAction = receiveAction;
    }
    public override void OnResponse(MainPack pack)
    {
        if (pack.ReturnCode == ReturnCode.Success)
        {
            Debug.Log("登录成功");
            ModelContainer.Instance.GetModel<MemoryModel>().UserName = pack.LoginPack.UserName;
        }
        else if (pack.ReturnCode == ReturnCode.Fail)
        {
            Debug.Log("登录失败");
        }
        ReceiveAction?.Invoke(pack);
        ReceiveAction = null;
    }
}
using SoulKnightProtocol;
using UnityEngine.Events;

public class BaseRequest
{
    protected RequestCode requestCode;
    protected ActionCode actionCode;
    public ActionCode m_ActionCode => actionCode;
    private RequestManager requestManager;
    protected UnityAction<MainPack> ReceiveAction;
    public BaseRequest(RequestManager manager)
    {
        requestManager = manager;
    }
    public virtual void OnDestroy()
    {
        requestManager.RemoveRequest(actionCode);
    }
    public virtual void OnResponse(MainPack pack)
    {

    }
    public virtual void SendRequest(MainPack pack)
    {
        requestManager.m_ClientManager.Send(pack);
    }
    public virtual void SendRequestUdp(MainPack pack)
    {
        LoginPack loginPack = new LoginPack();
        loginPack.UserName = ModelContainer.Instance.GetModel<MemoryModel>().UserName;
        pack.LoginPack = loginPack;
        requestManager.m_ClientManager.SendTo(pack);
    }
}

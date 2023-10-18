using SoulKnightProtocol;
using System.Collections.Generic;

public class RequestManager
{
    private ClientManager clientManager;
    public ClientManager m_ClientManager => clientManager;
    private Dictionary<ActionCode, BaseRequest> requestDic = new Dictionary<ActionCode, BaseRequest>();
    public RequestManager(ClientManager client)
    {
        clientManager = client;
        requestDic.Add(ActionCode.Register, new RequestRegister(this));
        requestDic.Add(ActionCode.Login, new RequestLogin(this));
        requestDic.Add(ActionCode.FindRoom, new RequestFindRoom(this));
        requestDic.Add(ActionCode.CreateRoom, new RequestCreateRoom(this));
        requestDic.Add(ActionCode.JoinRoom, new RequestJoinRoom(this));
        requestDic.Add(ActionCode.ExitRoom, new RequestExitRoom(this));
        requestDic.Add(ActionCode.FindPlayer, new RequestFindPlayer(this));
        requestDic.Add(ActionCode.EnterOnlineStartRoom, new RequestEnterOnlineStartRoom(this));
        requestDic.Add(ActionCode.UpdatePlayerState, new RequestUpdatePlayerState(this));
    }
    public void AddRequest(BaseRequest request)
    {
        requestDic.Add(request.m_ActionCode, request);
    }
    public BaseRequest GetRequest(ActionCode code)
    {
        if (requestDic.ContainsKey(code))
        {
            return requestDic[code];
        }
        return null;
    }
    public void RemoveRequest(ActionCode code)
    {
        requestDic.Remove(code);
    }
    public void HandleResponse(MainPack pack)
    {
        if (requestDic.TryGetValue(pack.ActionCode, out BaseRequest request))
        {
            request.OnResponse(pack);
        }
    }
}
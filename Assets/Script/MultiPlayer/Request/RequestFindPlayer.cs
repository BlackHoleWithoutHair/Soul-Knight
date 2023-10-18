using SoulKnightProtocol;

public class RequestFindPlayer : BaseRequest
{
    public RequestFindPlayer(RequestManager manager) : base(manager)
    {
        actionCode = ActionCode.FindPlayer;
    }
    public override void OnResponse(MainPack pack)
    {
        EventCenter.Instance.NotisfyObserver(EventType.OnFindPlayerResponse, pack);
    }
}

using SoulKnightProtocol;
using UnityEngine;

public class RequestEnterOnlineStartRoom : BaseRequest
{
    public RequestEnterOnlineStartRoom(RequestManager manager) : base(manager)
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.EnterOnlineStartRoom;
    }
    public override void OnResponse(MainPack pack)
    {
        if (pack.ReturnCode == ReturnCode.Success)
        {
            Debug.Log("成功加入联机初始房间");
        }
        if (pack.ReturnCode == ReturnCode.Fail)
        {
            Debug.Log("加入联机初始房间失败");
        }
        EventCenter.Instance.NotisfyObserver(EventType.OnEnterOnlineStartRoomResponse, pack);
    }
    public void SendRequest(PlayerAttribute attr)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        CharacterPack characterPack = new CharacterPack();
        characterPack.PlayerType = attr.m_ShareAttr.Type.ToString();
        pack.CharacterPacks.Add(characterPack);
        base.SendRequest(pack);
    }
}

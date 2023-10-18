using SoulKnightProtocol;
using UnityEngine.Events;

public class RequestUpdatePlayerState : BaseRequest
{
    public RequestUpdatePlayerState(RequestManager manager) : base(manager)
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdatePlayerState;
    }
    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
        ReceiveAction?.Invoke(pack);
    }
    public void SendRequest(PlayerControlInput input, UnityAction<MainPack> receiveAction = null)
    {
        MainPack pack = new MainPack();
        CharacterPack characterPack = new CharacterPack();
        InputPack inputPack = new InputPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        inputPack.Horizontal = input.Horizontal;
        inputPack.Vertical = input.Vertical;
        inputPack.MousePosX = input.MouseWorldPos.x;
        inputPack.MousePosY = input.MouseWorldPos.y;
        inputPack.CharacterPosX = input.CharacterPos.x;
        inputPack.CharacterPosY = input.CharacterPos.y;
        inputPack.IsAttackKeyDown = input.isAttackKeyDown;
        characterPack.InputPack = inputPack;
        pack.CharacterPacks.Add(characterPack);
        base.SendRequestUdp(pack);
        ReceiveAction = receiveAction;
    }
}

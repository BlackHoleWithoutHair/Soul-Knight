using KnightServer;
using SoulKnightProtocol;

public class GameController : BaseController
{
    public GameController(ControllerManager manager) : base(manager)
    {
        requestCode = RequestCode.Game;
    }
    public MainPack UpdatePlayerState(Client client, MainPack pack)
    {
        Room room = client.Room;
        if (room != null)
        {
            pack.CharacterPacks[0].CharacterName = client.userName;
            pack.IsBroadcastMessage = true;
            pack.ReturnCode = ReturnCode.Success;
            room.BroadcastTo(client, pack);
            pack.IsBroadcastMessage = false;
        }
        else
        {
            pack.ReturnCode = ReturnCode.Fail;
        }
        return pack;
    }
}

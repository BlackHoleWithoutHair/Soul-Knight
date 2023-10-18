
using SoulKnightProtocol;
namespace KnightServer
{
    public class RoomController : BaseController
    {
        public RoomController(ControllerManager manager) : base(manager)
        {
            requestCode = RequestCode.Room;
        }
        public MainPack CreateRoom(Client client, MainPack pack)
        {
            try
            {
                Room room = new Room(client, pack.RoomPacks[0]);
                client.Server.Rooms.Add(room);
                pack.ReturnCode = ReturnCode.Success;
                pack.ActionCode = ActionCode.CreateRoom;
            }
            catch
            (Exception)
            {
                Console.WriteLine("创建房间失败");
                pack.ReturnCode = ReturnCode.Fail;
            }
            return pack;
        }
        public MainPack FindRoom(Client client, MainPack pack)
        {
            pack.ActionCode = ActionCode.FindRoom;
            try
            {
                if (client.Server.Rooms.Count == 0 || !client.Server.Rooms.Any(room => room.hostCode != HostCode.SelectCharacter))
                {
                    pack.ReturnCode = ReturnCode.NoRoom;
                }
                else
                {
                    foreach (Room room in client.Server.Rooms)
                    {
                        if (room.hostCode != HostCode.SelectCharacter)
                        {
                            pack.RoomPacks.Add(room.GetRoomPack());
                        }
                    }
                    pack.ReturnCode = ReturnCode.Success;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("查询房间失败");
                pack.ReturnCode = ReturnCode.Fail;
            }
            return pack;
        }
        public MainPack JoinRoom(Client client, MainPack pack)
        {
            bool isFind = false;
            Room findRoom = null;
            foreach (Room room in client.Server.Rooms)
            {
                if (room.m_RoomName == pack.RoomPacks[0].RoomName)
                {
                    isFind = true;
                    findRoom = room;
                    break;
                }
            }
            if (isFind)
            {
                Console.WriteLine("加入的房间被找到");
                pack.ReturnCode = findRoom.AddPlayer(client);
                if (pack.ReturnCode == ReturnCode.Success)
                {
                    pack.RoomPacks.Clear();
                    pack.RoomPacks.Add(findRoom.GetRoomPack());
                    pack.ActionCode = ActionCode.FindPlayer;
                    findRoom.Broadcast(pack);
                    pack.ActionCode = ActionCode.JoinRoom;
                }
            }
            else
            {
                pack.ReturnCode = ReturnCode.Fail;
            }
            return pack;
        }
        public MainPack ExitRoom(Client client, MainPack pack)
        {
            Room room = client.Room;
            if (client.Room != null)
            {
                pack.ReturnCode = client.Room.RemovePlayer(client);
                if (pack.ReturnCode == ReturnCode.Success)
                {
                    pack.RoomPacks.Clear();
                    pack.RoomPacks.Add(room.GetRoomPack());
                    pack.ActionCode = ActionCode.FindPlayer;
                    room.Broadcast(pack);
                    pack.ActionCode = ActionCode.ExitRoom;
                }
            }
            else
            {
                pack.ReturnCode = ReturnCode.Fail;
            }
            return pack;
        }
        public MainPack EnterOnlineStartRoom(Client client, MainPack pack)
        {
            client.PlayerType = pack.CharacterPacks[0].PlayerType;
            Room room = client.Room;
            if (room != null)
            {
                if (client.userName == room.Clients[0].userName)
                {
                    room.hostCode = HostCode.WaitForStartGame;
                }
                pack.ReturnCode = ReturnCode.Success;
                pack.IsBroadcastMessage = true;
                pack.CharacterPacks[0].CharacterName = client.userName;
                room.Broadcast(client, pack);
                pack.IsBroadcastMessage = false;
                pack.CharacterPacks.Clear();
                foreach (Client c in room.Clients)
                {
                    if (c.PlayerType != null && c.PlayerType.Length != 0)
                    {
                        CharacterPack p = new CharacterPack();
                        p.CharacterName = c.userName;
                        p.PlayerType = c.PlayerType;
                        pack.CharacterPacks.Add(p);
                    }
                }
            }
            else
            {
                pack.ReturnCode = ReturnCode.Fail;
            }
            return pack;
        }
    }
}


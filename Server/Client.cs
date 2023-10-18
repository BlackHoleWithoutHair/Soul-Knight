using SoulKnightProtocol;
using System.Net;
using System.Net.Sockets;
namespace KnightServer
{
    public class Client
    {
        private Socket socket;
        private Server server;
        public Server Server => server;
        private Message message;
        private UserDao userDao;
        public UserDao m_UserDao => userDao;
        private Room? room = null;
        public Room Room => room;
        public EndPoint remoteEp;
        public string userName;
        public string PlayerType;
        public Client(Socket socket, Server server)
        {
            this.socket = socket;
            this.server = server;
            userDao = new UserDao();
            message = new Message();
            BeginReceive();
        }
        private void BeginReceive()
        {
            socket.BeginReceive(message.Buffer, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                if (socket == null || socket.Connected == false) return;
                int len = socket.EndReceive(result);
                if (len == 0)
                {
                    Close();
                    return;
                }
                message.ReadBuffer(len, (pack) =>
                {
                    server.m_ControllerManager.HandleRequest(pack, this);//处理接收数据
                });
                BeginReceive();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Close();
            }
        }
        public void Send(MainPack pack)
        {
            if (pack != null)
            {
                socket.Send(Message.ConvertToByteArray(pack));//报错点
            }
        }
        public void SendTo(MainPack pack)
        {
            if (remoteEp == null) return;
            byte[] buff = Message.PackDataUDP(pack);
            server.UDPServer.socket.SendTo(buff, buff.Length, SocketFlags.None, remoteEp);
        }
        private void Close()
        {
            if (room != null)
            {
                room.RemovePlayer(this);
            }
            server.RemoveClient(this);
            socket.Close();
            userDao.CloseConn();
            Console.WriteLine("关闭了一个客户端");
        }
        public void SetRoom(Room room)
        {
            this.room = room!;
        }
    }
}

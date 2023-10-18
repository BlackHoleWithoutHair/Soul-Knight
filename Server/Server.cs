using System.Net;
using System.Net.Sockets;
namespace KnightServer
{
    public class Server
    {
        private Socket socket;
        private UDPServer us;
        public UDPServer UDPServer => us;
        private Thread ausThread;
        private List<Client> clients = new List<Client>();
        private List<Room> rooms = new List<Room>();
        public List<Room> Rooms => rooms;
        private ControllerManager controllerManager;
        public ControllerManager m_ControllerManager => controllerManager;
        public Server(int port)
        {
            controllerManager = new ControllerManager(this);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(0);
            BeginAccept();
            us = new UDPServer(9998, this, controllerManager);

        }
        ~Server()
        {
            if (ausThread != null)
            {
                ausThread.Abort();
                ausThread = null;
            }
        }
        private void BeginAccept()
        {
            socket.BeginAccept(AcceptCallback, null);
        }
        private void AcceptCallback(IAsyncResult result)
        {
            Socket client = socket.EndAccept(result);
            Console.WriteLine("增加了一个客户端");
            clients.Add(new Client(client, this));
            BeginAccept();
        }
        public void RemoveClient(Client client)
        {
            clients.Remove(client);
        }
        public Client GetClientByUserName(string user)
        {
            foreach (Client client in clients)
            {
                if (client.userName == user)
                {
                    return client;
                }
            }
            return null;
        }
    }
}


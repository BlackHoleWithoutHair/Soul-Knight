using KnightServer;
using SoulKnightProtocol;
using System.Net;
using System.Net.Sockets;

public class UDPServer
{
    Socket udpServer;
    public Socket socket => udpServer;
    IPEndPoint bindEP;//本地ip
    EndPoint remoteEP;//远程ip
    Server server;
    ControllerManager controllerManager;
    Byte[] buffer = new byte[1024];
    Thread receiveThread;
    public UDPServer(int port, Server server, ControllerManager manager)
    {
        this.server = server;
        this.controllerManager = manager;
        udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        bindEP = new IPEndPoint(IPAddress.Any, port);
        remoteEP = bindEP;
        udpServer.Bind(bindEP);
        receiveThread = new Thread(ReceiveMsg);
        receiveThread.Start();
        Console.WriteLine("UDP服务器已启动");
    }
    ~UDPServer()
    {
        if (receiveThread != null)
        {
            receiveThread.Abort();
            receiveThread = null;
        }
    }
    public void ReceiveMsg()
    {
        while (true)
        {
            int len = udpServer.ReceiveFrom(buffer, ref remoteEP);
            MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 0, len);
            Handlerequest(pack, remoteEP);

        }
    }
    private void Handlerequest(MainPack pack, EndPoint remoteEp)
    {
        Client client = server.GetClientByUserName(pack.LoginPack.UserName);
        if (client.remoteEp == null)
        {
            client.remoteEp = remoteEp;
        }
        controllerManager.HandleRequest(pack, client, true);
    }

}
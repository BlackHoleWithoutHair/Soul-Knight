using SoulKnightProtocol;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ClientManager : BaseManager
{
    private Socket socket;
    private Socket udpClient;
    private IPEndPoint ipEndPoint;
    private EndPoint endPoint;
    private Byte[] buffer = new Byte[1024];
    private Thread aucThread;
    private Message message;
    private RequestManager requestManager;
    public RequestManager m_RequestManager => requestManager;
    public ClientManager(ClientFacade facade) : base(facade)
    {
        requestManager = new RequestManager(this);
    }
    public override void OnInit()
    {
        base.OnInit();
        message = new Message();
        InitSocket();
        InitUDP();
        BeginReceive();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        message = null;
        CloseSocket();
    }
    private void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect("127.0.0.1", 9999);
        }
        catch
        {
            UnityEngine.Debug.Log("连接服务器失败");
        }

    }
    private void CloseSocket()
    {
        if (socket.Connected && socket != null)
        {
            socket.Close();
        }
    }
    private void BeginReceive()
    {
        socket.BeginReceive(message.Buffer, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }
    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int len = socket.EndReceive(result);
            if (len == 0)
            {
                CloseSocket();
                return;
            }
            message.ReadBuffer(len, (pack) =>
            {
                requestManager.HandleResponse(pack);
            });
            BeginReceive();
        }
        catch
        {
            Debug.Log("服务器连接失败");
            return;
        }

    }


    private void InitUDP()
    {
        udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);
        endPoint = ipEndPoint;
        try
        {
            udpClient.Connect(endPoint);
        }
        catch
        {
            Debug.Log("UDP连接失败");
        }
        aucThread = new Thread(ReceiveMsg);
        aucThread.Start();
    }
    private void ReceiveMsg()
    {
        Debug.Log("UDP开始接收");
        while (true)
        {
            int len = udpClient.ReceiveFrom(buffer, ref endPoint);
            MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 0, len);
            //Debug.Log("")
            requestManager.HandleResponse(pack);
        }
    }
    public void Send(MainPack pack)
    {
        if (socket.Connected)
        {
            socket.Send(Message.ConvertToByteArray(pack));
        }
    }
    public void SendTo(MainPack pack)
    {
        pack.LoginPack.UserName = ModelContainer.Instance.GetModel<MemoryModel>().UserName;
        Byte[] sendBuff = Message.PackDataUDP(pack);
        udpClient.Send(sendBuff, sendBuff.Length, SocketFlags.None);
    }
}

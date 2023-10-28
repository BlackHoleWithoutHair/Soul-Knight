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
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            IAsyncResult connResult = socket.BeginConnect(ep, null, null);
            connResult.AsyncWaitHandle.WaitOne(2);
            if (connResult.IsCompleted)
            {
                BeginReceive();
            }
            else
            {
                UnityEngine.Debug.Log("连接TCP服务器超时");
            }
        }
        catch
        {
            UnityEngine.Debug.Log("连接TCP服务器失败");
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
            Debug.Log("TCP服务器连接失败");
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
            IAsyncResult udpResult = udpClient.BeginConnect(endPoint, null, null);
            udpResult.AsyncWaitHandle.WaitOne(2);
            if (udpResult.IsCompleted)
            {
                aucThread = new Thread(ReceiveMsg);
                aucThread.Start();
            }
            else
            {
                UnityEngine.Debug.Log("UDP服务器连接超时");
            }
        }
        catch
        {
            Debug.Log("UDP连接失败");
        }
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

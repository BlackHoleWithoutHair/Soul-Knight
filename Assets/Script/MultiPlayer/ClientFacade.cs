using SoulKnightProtocol;

public class ClientFacade
{
    private ClientManager clientManager;
    public ClientManager m_ClientManager => clientManager;
    private static ClientFacade instance;
    public static ClientFacade Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ClientFacade();
            }
            return instance;
        }
    }
    public ClientFacade()
    {
        clientManager = new ClientManager(this);
        clientManager.OnInit();
    }
    public BaseRequest GetRequest(ActionCode code)
    {
        return clientManager.m_RequestManager.GetRequest(code);
    }
}

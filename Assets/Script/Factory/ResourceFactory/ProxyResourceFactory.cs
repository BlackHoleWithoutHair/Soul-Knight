public class ProxyResourceFactory
{
    private static ProxyResourceFactory instance;
    public static ProxyResourceFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ProxyResourceFactory();
            }
            return instance;
        }
    }
    private IResourceFactory factory;
    public IResourceFactory Factory => factory;
    private ProxyResourceFactory()
    {
        factory = new ResourcesFactory();
    }

}

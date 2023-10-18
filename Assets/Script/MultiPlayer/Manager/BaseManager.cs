public class BaseManager
{
    protected ClientFacade facade;
    public BaseManager(ClientFacade facade)
    {
        this.facade = facade;
    }
    public virtual void OnInit() { }
    public virtual void OnDestroy() { }
}

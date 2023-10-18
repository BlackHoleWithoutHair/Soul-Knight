using KnightServer;
using SoulKnightProtocol;
public abstract class BaseController
{
    protected RequestCode requestCode = RequestCode.RequestNone;
    public RequestCode m_RequestCode => requestCode;
    protected ControllerManager controllerManager;
    public BaseController(ControllerManager manager)
    {
        controllerManager = manager;
    }
}

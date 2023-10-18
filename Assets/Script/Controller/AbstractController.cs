public abstract class AbstractController
{
    private bool isInit;
    private bool isAfterRunInit;
    private bool isControllerRun;
    public AbstractController() { }
    protected virtual void Init() { }
    protected virtual void AlwaysUpdate() { }
    protected virtual void OnBeforeRunUpdate()
    {
        if (!isInit)
        {
            isInit = true;
            Init();
        }
    }
    protected virtual void OnAfterRunInit() { }
    protected virtual void OnAfterRunUpdate()
    {
        if (!isAfterRunInit)
        {
            isAfterRunInit = true;
            OnAfterRunInit();
        }
    }
    public virtual void GameUpdate()
    {
        if (isControllerRun)
        {
            OnAfterRunUpdate();
        }
        else
        {
            OnBeforeRunUpdate();
        }
        AlwaysUpdate();
    }
    public void TurnOnController()
    {
        isControllerRun = true;
    }
    public void TurnOffController()
    {
        isControllerRun = false;
    }
}
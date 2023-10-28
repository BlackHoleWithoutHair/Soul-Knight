public abstract class IState
{
    public IStateController m_Controller;
    private bool isInit;
    private bool isStart;
    public IState(IStateController controller)
    {
        m_Controller = controller;
    }
    public virtual void GameUpdate()
    {
        StateAnyUpdate();
        StateUpdate();
    }
    protected virtual void StateInit() { }
    protected virtual void StateStart()
    {
        if (!isInit)
        {
            isInit = true;
            StateInit();
        }
    }
    protected virtual void StateUpdate()
    {
        if (!isStart)
        {
            isStart = true;
            StateStart();
        }
    }
    protected virtual void StateEnd()
    {
        isStart = false;
    }
    protected virtual void StateAnyUpdate() { }
    public void OnExit()
    {
        StateEnd();
    }
}


public abstract class IState
{
    private bool isInit;
    public IStateController m_Controller;
    public IState(IStateController controller)
    {
        m_Controller = controller;
    }
    public virtual void GameStart()
    {
        if (!isInit)
        {
            isInit = true;
            StateInit();
        }
        StateStart();
    }
    public virtual void GameUpdate()
    {
        StateAnyUpdate();
        StateUpdate();
    }
    public virtual void GameExit()
    {
        StateEnd();
    }
    protected virtual void StateInit() { }
    protected virtual void StateStart() { }
    protected virtual void StateUpdate() { }
    protected virtual void StateEnd() { }
    protected virtual void StateAnyUpdate() { }
}


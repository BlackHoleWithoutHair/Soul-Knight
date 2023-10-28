public abstract class IGameFacade
{
    protected bool isGameStart;
    public bool IsGameStart => isGameStart;
    private bool isInit;
    private SceneController m_SceneController;
    public SceneController SceneController => m_SceneController;
    private InputController m_InputController;
    public InputController InputController => m_InputController;
    public IGameFacade() { }
    protected virtual void Init()
    {
        m_SceneController = new SceneController();
        m_InputController = new InputController();
    }
    public virtual void GameUpdate()
    {
        if (!isInit)
        {
            isInit = true;
            Init();
        }
        m_InputController.GameUpdate();
        m_SceneController.GameUpdate();
    }
}

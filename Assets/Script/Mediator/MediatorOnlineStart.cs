public class MediatorOnlineStart : IGameFacade
{
    private ItemController m_ItemController;
    private PlayerController m_PlayerSystem;
    public PlayerController PlayerSystem => m_PlayerSystem;
    public MediatorOnlineStart()
    {
        m_ItemController = new ItemController();
        m_PlayerSystem = new PlayerController();

    }
    protected override void Init()
    {
        base.Init();
        m_ItemController.TurnOnController();
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        m_ItemController.GameUpdate();
        m_PlayerSystem.GameUpdate();
    }
}


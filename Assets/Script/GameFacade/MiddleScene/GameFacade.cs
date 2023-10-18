namespace MiddleScene
{
    public class GameFacade : IGameFacade
    {
        private PlayerController m_PlayerController;
        private UIController m_UIController;
        private ItemController m_ItemController;
        private EnemyController m_EnemyController;
        private BuffController m_BuffController;
        public GameMediator m_Mediator { get; private set; }
        public GameFacade() : base()
        {

            m_PlayerController = new PlayerController();
            m_EnemyController = new EnemyController();
            m_UIController = new UIController();
            m_ItemController = new ItemController();
            m_BuffController = new BuffController();
            m_ItemController.TurnOnController();

            GameMediator.Instance.Register(m_PlayerController);
            GameMediator.Instance.Register(m_EnemyController);
            GameMediator.Instance.Register(m_UIController);
            GameMediator.Instance.Register(m_ItemController);
            GameMediator.Instance.Register(m_BuffController);

            EventCenter.Instance.RegisterObserver(EventType.OnFinishSelectPlayer, () =>
            {
                m_BuffController.TurnOffController();
                m_PlayerController.TurnOnController();
                m_EnemyController.TurnOnController();
            });
        }
        protected override void Init()
        {
            base.Init();
        }
        public override void GameUpdate()
        {
            base.GameUpdate();

            m_BuffController.GameUpdate();
            m_UIController.GameUpdate();
            m_ItemController.GameUpdate();
            m_PlayerController.GameUpdate();
            m_EnemyController.GameUpdate();
        }

    }
}


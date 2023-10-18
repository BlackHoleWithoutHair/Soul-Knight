namespace BattleScene
{
    public class GameFacade : IGameFacade
    {
        private PlayerController m_PlayerController;
        private UIController m_UIController;
        private ItemController m_ItemController;
        private EnemyController m_EnemyController;
        private RoomController m_RoomController;
        private BuffController m_BuffController;
        public GameFacade()
        {

            m_ItemController = new ItemController();
            m_UIController = new UIController();
            m_RoomController = new RoomController();
            m_PlayerController = new PlayerController();
            m_EnemyController = new EnemyController();
            m_BuffController = new BuffController();


            GameMediator.Instance.Register(m_ItemController);
            GameMediator.Instance.Register(m_UIController);
            GameMediator.Instance.Register(m_RoomController);
            GameMediator.Instance.Register(m_PlayerController);
            GameMediator.Instance.Register(m_EnemyController);
            GameMediator.Instance.Register(m_BuffController);

            EventCenter.Instance.RegisterObserver(EventType.OnFinishRoomGenerate, () =>
            {
                m_ItemController.TurnOnController();
                m_UIController.TurnOnController();
                m_RoomController.TurnOnController();
                m_BuffController.TurnOnController();
                m_EnemyController.TurnOnController();
                m_PlayerController.TurnOnController();
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
            m_ItemController.GameUpdate();
            m_UIController.GameUpdate();
            m_RoomController.GameUpdate();
            m_EnemyController.GameUpdate();
            m_PlayerController.GameUpdate();
        }
    }

}

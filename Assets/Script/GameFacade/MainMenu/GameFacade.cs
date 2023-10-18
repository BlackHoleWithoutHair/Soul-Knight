
namespace MainMenuScene
{
    public class GameFacade : IGameFacade
    {
        public UIController m_UISystem { get; private set; }
        public GameFacade()
        {

            m_UISystem = new UIController();

            GameMediator.Instance.Register(m_UISystem);
        }
        protected override void Init() { }
        public override void GameUpdate()
        {
            m_UISystem.GameUpdate();
        }
    }
}


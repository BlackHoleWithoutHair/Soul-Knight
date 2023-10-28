
namespace MainMenuScene
{
    public class GameFacade : IGameFacade
    {
        public UIController m_UIController { get; private set; }
        public GameFacade()
        {

            m_UIController = new UIController();

            GameMediator.Instance.RegisterController(m_UIController);
            GameMediator.Instance.RegisterSystem(new AudioSystem());
        }
        protected override void Init() { }
        public override void GameUpdate()
        {
            m_UIController.GameUpdate();
        }
    }
}


public class SceneController : AbstractController
{
    private SceneModel m_SceneModel;
    private MemoryModel m_MemoryModel;
    public SceneController()
    {
        EventCenter.Instance.RegisterObserver(EventType.OnSceneChangeComplete, OnSceneChange);
    }
    protected override void Init()
    {
        base.Init();
        m_SceneModel = ModelContainer.Instance.GetModel<SceneModel>();
        m_MemoryModel = ModelContainer.Instance.GetModel<MemoryModel>();
        m_MemoryModel.isOnlineMode.Register((val) =>
        {
            SceneModelCommand.Instance.LoadScene(SceneName.MiddleScene);
        });
    }
    private void OnSceneChange()
    {
        if (m_SceneModel.sceneName == SceneName.MiddleScene
            || m_SceneModel.sceneName == SceneName.MainMenuScene)
        {
            MemoryModelCommand.Instance.InitMemoryModel();
        }
        if (m_SceneModel.sceneName == SceneName.MainMenuScene)
        {
            MemoryModelCommand.Instance.ExitOnlineMode();
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneModelCommand : Singleton<SceneModelCommand>
{
    private SceneModel sceneModel;
    private SceneName name;
    private SceneModelCommand()
    {
        sceneModel = ModelContainer.Instance.GetModel<SceneModel>();
    }
    public int GetActiveSceneIndex()
    {
        return sceneModel.SceneIndex;
    }
    public SceneName GetActiveSceneName()
    {
        return sceneModel.sceneName;
    }
    public AsyncOperation LoadScene(SceneName name)
    {
        this.name = name;
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)name);
        operation.completed += OnSceneChange;
        return operation;
    }
    public AsyncOperation ReloadActiveScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneModel.SceneIndex);
        operation.completed += OnSceneChange;
        return operation;
    }
    private void OnSceneChange(AsyncOperation operation)
    {
        sceneModel.SceneIndex = (int)name;
        SetSceneType((int)name);
        EventCenter.Instance.NotisfyObserver(EventType.OnSceneChangeComplete);
        EventCenter.Instance.ClearObserver();
    }
    private void SetSceneType(int index)
    {
        switch (index)
        {
            case 0:
                sceneModel.sceneName = SceneName.MainMenuScene;
                sceneModel.sceneType = SceneType.Normal; break;
            case 1:
                sceneModel.sceneName = SceneName.MiddleScene;
                sceneModel.sceneType = SceneType.Normal; break;
            case 2:
                sceneModel.sceneName = SceneName.BattleScene;
                sceneModel.sceneType = SceneType.Battle; break;
            case 3:
                sceneModel.sceneName = SceneName.OnlineStartScene;
                sceneModel.sceneType = SceneType.Battle; break;
        }
    }
}
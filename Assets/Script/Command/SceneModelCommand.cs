using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneModelCommand : Singleton<SceneModelCommand>
{
    private SceneModel sceneModel;
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
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)name);
        operation.completed += OnSceneChange;
        return operation;
    }
    public AsyncOperation ReloadActiveScene()
    {
        Debug.Log(sceneModel.SceneIndex);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneModel.SceneIndex);
        operation.completed += OnSceneChange;
        return operation;
    }
    private void OnSceneChange(AsyncOperation operation)
    {
        sceneModel.SetValue();
        EventCenter.Instance.NotisfyObserver(EventType.OnSceneChangeComplete);
        EventCenter.Instance.ClearObserver();
    }
}
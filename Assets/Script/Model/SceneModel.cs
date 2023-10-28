using UnityEngine.SceneManagement;

public class SceneModel : AbstractModel
{
    public int SceneIndex;
    public SceneName sceneName;
    public SceneType sceneType;
    protected override void OnInit()
    {
        SetValue();
    }
    private int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public void SetValue()
    {
        SceneIndex = GetActiveSceneIndex();
        switch (SceneIndex)
        {
            case 0:
                sceneName = SceneName.MainMenuScene;
                sceneType = SceneType.Normal; break;
            case 1:
                sceneName = SceneName.MiddleScene;
                sceneType = SceneType.Normal; break;
            case 2:
                sceneName = SceneName.BattleScene;
                sceneType = SceneType.Battle; break;
            case 3:
                sceneName = SceneName.OnlineStartScene;
                sceneType = SceneType.Battle; break;
        }
    }
}
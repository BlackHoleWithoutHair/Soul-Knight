using DG.Tweening;
using System.Collections;
using UnityEngine;
public class UIController : AbstractController
{
    private IPanel rootPanel;
    public IPanel RootPanel => rootPanel;
    private GameObject Notice;
    private Coroutine coroutine;
    public UIController()
    {
        switch (SceneModelCommand.Instance.GetActiveSceneName())
        {
            case SceneName.MainMenuScene:
                rootPanel = new MainMenuScene.PanelRoot();
                break;
            case SceneName.MiddleScene:
                rootPanel = new MiddleScene.PanelRoot();
                break;
            case SceneName.BattleScene:
                rootPanel = new BattleScene.PanelRoot();
                break;
        }
        Notice = UnityTool.Instance.GetGameObjectFromCanvas("Notice");
        if (Notice != null)
        {
            Notice.SetActive(true);
            Notice.GetComponent<CanvasGroup>().alpha = 0;
            EventCenter.Instance.RegisterObserver<string>(EventType.OnWantShowNotice, (s) =>
            {
                Notice.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
                if (coroutine != null)
                {
                    CoroutinePool.Instance.StopCoroutine(coroutine);
                }
                coroutine = CoroutinePool.Instance.StartCoroutine(WaitForCloseNotice());
            });
        }
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        rootPanel.GameUpdate();
    }
    private IEnumerator WaitForCloseNotice()
    {
        yield return new WaitForSeconds(2);
        Notice.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
        coroutine = null;
    }
}

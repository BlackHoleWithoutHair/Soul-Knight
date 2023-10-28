
using UnityEngine;

public class PanelBossCutScene : IPanel
{
    private Animator m_Animator;
    public PanelBossCutScene(IPanel parent) : base(parent)
    {
        children.Add(new PanelBoss(this));
    }
    protected override void OnInit()
    {
        base.OnInit();
        m_Animator = gameObject.GetComponent<Animator>();
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        m_Animator.SetBool("isEnter", true);
        CoroutinePool.Instance.StartAnimatorCallback(m_Animator, "Enter", () =>
        {
            CoroutinePool.Instance.DelayInvoke(() =>
            {
                m_Animator.SetBool("isExit", true);
                CoroutinePool.Instance.StartAnimatorCallback(m_Animator, "Exit", () =>
                {
                    EnterPanel(typeof(PanelBoss));
                    EventCenter.Instance.NotisfyObserver(EventType.OnResume);
                    EventCenter.Instance.NotisfyObserver(EventType.OnBossCutSceneFinish);
                });
            }, 1);
        });
    }
}

using UnityEngine;

public class Pane : Item
{
    private Animator m_Animator;
    private Animator m_AppearLightAnim;
    private AnimatorStateInfo info;
    private AnimatorStateInfo LightInfo;
    private float Timer;
    public Pane(GameObject obj) : base(obj)
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_AppearLightAnim = gameObject.transform.Find("AppearLight").GetComponent<Animator>();
    }
    protected override void Init()
    {
        base.Init();
        gameObject.transform.position = position;
        Timer = 0;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        Timer += Time.deltaTime;
        info = m_Animator.GetCurrentAnimatorStateInfo(0);
        LightInfo = m_AppearLightAnim.GetCurrentAnimatorStateInfo(0);
        if (LightInfo.normalizedTime > 1)
        {
            m_AppearLightAnim.gameObject.SetActive(false);
        }
        if (Timer > 0.66f)
        {
            m_AppearLightAnim.enabled = true;
        }
        if (info.normalizedTime > 1)
        {
            Remove();
        }
    }
}

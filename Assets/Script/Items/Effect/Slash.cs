using UnityEngine;

public class Slash : Item
{
    private Animator m_Animator;
    private AnimatorStateInfo info;
    public Slash(GameObject obj) : base(obj)
    {
        m_Animator = gameObject.GetComponent<Animator>();

    }
    protected override void Init()
    {
        base.Init();
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        info = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime > 1)
        {
            Remove();
        }
    }
}

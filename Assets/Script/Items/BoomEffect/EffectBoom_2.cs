using System.Collections;
using UnityEngine;

public class EffectBoom_2 : IEffectBoom
{
    private ParticleSystem m_Fog;
    private Animator m_Boom1Animator;
    private AnimatorStateInfo info;
    private bool isFogActive;
    public EffectBoom_2(GameObject obj) : base(obj)
    {
        type = EffectBoomType.EffectBoom_2;
        m_Fog = gameObject.transform.Find("Fog").GetComponent<ParticleSystem>();
        m_Boom1Animator = gameObject.transform.Find("Boom1").GetComponent<Animator>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        isFogActive = false;
        m_Boom1Animator.gameObject.SetActive(true);
        m_Boom1Animator.Play("Boom1", 0, 0);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        info = m_Boom1Animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime > 1 && !isFogActive)
        {
            isFogActive = true;
            m_Boom1Animator.gameObject.SetActive(false);
            m_Fog.gameObject.SetActive(true);
            CoroutinePool.Instance.StartCoroutine(WaitForDestroy());
        }
    }
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Remove();
    }
    protected override void OnExit()
    {
        base.OnExit();
        m_Fog.gameObject.SetActive(false);
    }
}

using UnityEngine;

public class IEffectBoom : Item
{
    protected EffectBoomType type;
    protected Animator m_BoomAnimator;
    protected ParticleSystem m_Particle;
    protected ParticleSystem.MainModule m_MainModule;
    private AnimatorStateInfo info;
    public IEffectBoom(GameObject obj) : base(obj)
    {
        m_BoomAnimator = obj.transform.Find("Boom").GetComponent<Animator>();
        m_Particle = obj.transform.Find("Spark").GetComponent<ParticleSystem>();
        m_MainModule = m_Particle.main;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        m_BoomAnimator.gameObject.SetActive(true);
        m_BoomAnimator.Play("Idle", 0, 0);
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        info = m_BoomAnimator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime > 1)
        {
            m_BoomAnimator.gameObject.SetActive(false);
        }
    }
    public void SetColor(Color color)
    {
        m_BoomAnimator.GetComponent<SpriteRenderer>().color = color;
        m_MainModule.startColor = color;
    }

    public override void Remove()
    {
        base.Remove();
        if (!isDestroyOnRemove)
        {
            pool.ReturnItem(type, this);
        }
    }
}


using System.Collections;
using UnityEngine;

public class Cut : ISkill
{
    private Rigidbody2D m_rb;
    private Vector2 dir;
    public Cut(IPlayer character) : base(character)
    {
        m_Attr = AttributeFactory.Instance.GetSkillAttr(SkillType.Cut, m_Player.m_Attr);
        m_rb = m_Player.gameObject.GetComponent<Rigidbody2D>();
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        dir = m_Player.GetDirection().normalized;
        Item effect = EffectFactory.Instance.GetEffect(EffectType.Slash, m_Player.gameObject.transform.position);
        effect.SetRotation(m_Player.GetRotation());
        effect.AddToController();
        CoroutinePool.Instance.StartCoroutine(WaitForFinish());

    }
    protected override void OnSkillDuration()
    {
        base.OnSkillDuration();
        m_rb.velocity = 40f * dir;
    }
    private IEnumerator WaitForFinish()
    {
        yield return new WaitForSeconds(0.25f);
        StopSkill();
    }
}

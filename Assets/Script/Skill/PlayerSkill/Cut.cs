using System.Collections;
using UnityEngine;

public class Cut : IPlayerSkill
{
    private Rigidbody2D m_rb;
    private Vector2 dir;
    public Cut(IPlayer character) : base(character)
    {
        m_Attr = AttributeFactory.Instance.GetSkillAttr(SkillType.Cut, m_Character.m_Attr);
        m_rb = m_Character.gameObject.GetComponent<Rigidbody2D>();
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        dir = m_Character.GetDirection().normalized;
        Item effect = EffectFactory.Instance.GetEffect(EffectType.Slash, m_Character.gameObject.transform.position);
        effect.SetRotation(m_Character.GetRotation());
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

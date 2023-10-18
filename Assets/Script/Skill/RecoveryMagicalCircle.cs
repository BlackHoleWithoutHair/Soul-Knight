using System.Collections;
using UnityEngine;

public class RecoveryMagicalCircle : ISkill
{
    public RecoveryMagicalCircle(IPlayer character) : base(character)
    {
        m_Attr = AttributeFactory.Instance.GetSkillAttr(SkillType.RecoveryMagicalCircle, m_Player.m_Attr);
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        Item circle = EffectFactory.Instance.GetEffect(EffectType.RecoveryMagicCircle, m_Player.gameObject.transform.position);
        (circle as RecoveryMagicCircle).SetLifeTime(m_Attr.m_ShareAttr.SkillDuration);
        circle.AddToController();
        CoroutinePool.Instance.StartCoroutine(WaitForFinishSkill());
    }
    protected override void OnSkillDuration()
    {
        base.OnSkillDuration();
    }
    protected override void OnFinishSkill()
    {
        base.OnFinishSkill();
    }
    private IEnumerator WaitForFinishSkill()
    {
        yield return new WaitForSeconds(m_Attr.m_ShareAttr.SkillDuration);
        StopSkill();
    }
}

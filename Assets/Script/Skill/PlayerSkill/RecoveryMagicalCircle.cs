using System.Collections;
using UnityEngine;

public class RecoveryMagicalCircle : IPlayerSkill
{
    public RecoveryMagicalCircle(IPlayer character) : base(character)
    {
        m_Attr = AttributeFactory.Instance.GetSkillAttr(SkillType.RecoveryMagicalCircle, m_Character.m_Attr);
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        Item circle = EffectFactory.Instance.GetEffect(EffectType.RecoveryMagicCircle, m_Character.gameObject.transform.position);
        (circle as RecoveryMagicCircle).SetLifeTime(m_Attr.m_ShareAttr.SkillDuration);
        circle.AddToController();
        CoroutinePool.Instance.StartCoroutine(WaitForFinishSkill());
    }
    protected override void OnSkillDuration()
    {
        base.OnSkillDuration();
    }
    protected override void OnSkillFinish()
    {
        base.OnSkillFinish();
    }
    private IEnumerator WaitForFinishSkill()
    {
        yield return new WaitForSeconds(m_Attr.m_ShareAttr.SkillDuration);
        StopSkill();
    }
}

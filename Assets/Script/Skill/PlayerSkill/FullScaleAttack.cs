public class FullScaleAttack:IPlayerSkill
{

    public FullScaleAttack(ICharacter character):base(character)
    {
        m_Attr = AttributeFactory.Instance.GetSkillAttr(SkillType.FullScaleAttack, m_Character.m_Attr);
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();

    }
    protected override void OnSkillDuration()
    {
        base.OnSkillDuration();

    }
    protected override void OnSkillFinish()
    {
        base.OnSkillFinish();

    }
}

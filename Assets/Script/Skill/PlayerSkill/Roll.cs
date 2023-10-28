using UnityEngine;

public class Roll : IPlayerSkill
{
    private AnimatorStateInfo info;
    public Roll(IPlayer character) : base(character)
    {
        m_Attr = AttributeFactory.Instance.GetSkillAttr(SkillType.Roll, m_Character.m_Attr);
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        m_Character.gameObject.tag = "Untagged";
        m_Animator.SetBool("isRoll", true);
        m_Character.GetUsedWeapon().gameObject.SetActive(false);
    }
    protected override void OnSkillDuration()
    {
        base.OnSkillDuration();
        info = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Roll") && info.normalizedTime > 1)
        {
            StopSkill();
            return;
        }
    }
    protected override void OnSkillFinish()
    {
        base.OnSkillFinish();
        m_Character.gameObject.tag = "Player";
        m_Animator.SetBool("isRoll", false);
        m_Character.GetUsedWeapon().gameObject.SetActive(true);
    }
}

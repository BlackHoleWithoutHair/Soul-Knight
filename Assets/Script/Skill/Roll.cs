using UnityEngine;

public class Roll : ISkill
{
    private AnimatorStateInfo info;
    public Roll(IPlayer character) : base(character)
    {
        m_Attr = AttributeFactory.Instance.GetSkillAttr(SkillType.Roll, m_Player.m_Attr);
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        m_Player.gameObject.tag = "Untagged";
        m_Animator.SetBool("isRoll", true);
        m_Player.GetUsedWeapon().gameObject.SetActive(false);
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
    protected override void OnFinishSkill()
    {
        base.OnFinishSkill();
        m_Player.gameObject.tag = "Player";
        m_Animator.SetBool("isRoll", false);
        m_Player.GetUsedWeapon().gameObject.SetActive(true);
    }
}

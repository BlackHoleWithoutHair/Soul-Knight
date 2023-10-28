using UnityEngine;

public class IPlayerSkill : ISkill
{
    protected new IPlayer m_Character { get => base.m_Character as IPlayer; set => base.m_Character = value; }
    public SkillAttribute m_Attr;
    protected Animator m_Animator;
    public IPlayerSkill(ICharacter character) : base(character) { }
    protected override void OnInit()
    {
        base.OnInit();
        m_Animator = UnityTool.Instance.GetComponentFromChild<Animator>(m_Character.gameObject, "Sprite");
    }
}
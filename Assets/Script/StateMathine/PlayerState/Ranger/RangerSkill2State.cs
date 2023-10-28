public class RangerSkill2State : PlayerState
{
    public RangerSkill2State(PlayerStateController controller) : base(controller) { }
    protected override void StateStart()
    {
        base.StateStart();
        player.m_Skill.StartSkill();
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        if (!player.m_Skill.isSkillUpdate)
        {
            m_Controller.SetOtherState(typeof(RangerIdleState));
        }
    }
}

public class RangerSkill1State : PlayerState
{
    public RangerSkill1State(PlayerStateController controller) : base(controller)
    {

    }
    protected override void StateStart()
    {
        base.StateStart();
        player.Skill.StartSkill();
    }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        if (!player.Skill.isSkillUpdate)
        {
            m_Controller.SetOtherState(typeof(RangerIdleState));
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
    }
}

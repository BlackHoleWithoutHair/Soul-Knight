public class RangerSkill2State : PlayerState
{
    public RangerSkill2State(PlayerStateController controller) : base(controller) { }
    public override void GameStart()
    {
        base.GameStart();
        player.Skill.StartSkill();
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        if (!player.Skill.isSkillUpdate)
        {
            m_Controller.SetOtherState(typeof(RangerIdleState));
        }
    }
    public override void GameExit()
    {
        base.GameExit();
    }
}

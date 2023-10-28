using UnityEngine;

public class BerserkerIdleState : PlayerState
{
    public BerserkerIdleState(PlayerStateController controller) : base(controller)
    {

    }
    protected override void StateStart()
    {
        base.StateStart();
    }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        m_rb.velocity = Vector2.zero;
        if (hor != 0 || ver != 0)
        {
            m_Controller.SetOtherState(typeof(BerserkerWalkState));
            return;
        }
        player.GetUsedWeapon().OnUpdate();
        if (player.IsDie)
        {
            m_Controller.SetOtherState(typeof(BerserkerDieState));
            return;
        }
        if (InputUtility.Instance.GetKeyDown(KeyAction.Skill))
        {
            player.m_Skill.StartSkill();
        }
    }
    protected override void StateEnd()
    {
        base.StateEnd();
    }
}

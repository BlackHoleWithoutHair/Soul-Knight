using UnityEngine;

public class PastorIdleState : PlayerState
{
    public PastorIdleState(PlayerStateController controller) : base(controller)
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
            m_Controller.SetOtherState(typeof(PastorWalkState));
            return;
        }
        if (player.IsDie)
        {
            m_Controller.SetOtherState(typeof(PastorDieState));
            return;
        }
        player.GetUsedWeapon().OnUpdate();
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

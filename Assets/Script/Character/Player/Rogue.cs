using UnityEngine;

public class Rogue : IPlayer
{
    public Rogue(GameObject obj, PlayerAttribute attr) : base(obj, attr)
    {

    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(RangerIdleState));

    }
}

using UnityEngine;

public class Assassin : IPlayer
{
    public Assassin(GameObject obj, PlayerAttribute attr) : base(obj, attr)
    {

    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(AssassinIdleState));
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
    }
    protected override void OnCharaterDieUpdate()
    {
        base.OnCharaterDieUpdate();
        m_StateController.GameUpdate();
    }
}

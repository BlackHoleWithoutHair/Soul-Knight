using UnityEngine;

public class Alchemist : IPlayer
{
    public Alchemist(GameObject obj, PlayerAttribute attr) : base(obj, attr)
    {

    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(AlchemistIdleState));
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        m_StateController.GameUpdate();
    }
    protected override void OnCharaterDieUpdate()
    {
        base.OnCharaterDieUpdate();
        m_StateController.GameUpdate();
    }
}

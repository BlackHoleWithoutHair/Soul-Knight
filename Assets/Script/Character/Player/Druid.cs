using UnityEngine;

public class Druid : IPlayer
{
    public Druid(GameObject obj, PlayerAttribute attr) : base(obj, attr)
    {

    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(DruidIdleState));
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

using UnityEngine;

public class TrumpetFlower : IEmployeeEnemy
{
    public TrumpetFlower(GameObject obj) : base(obj) { }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(TrumpetFlowerIdleState));
    }
}

using UnityEngine;

public class TrumpetFlower : IEnemy
{
    public TrumpetFlower(GameObject obj) : base(obj) { }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(TrumpetFlowerIdleState));
    }
}

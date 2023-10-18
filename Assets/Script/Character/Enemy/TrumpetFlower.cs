using UnityEngine;

public class TrumpetFlower : IEnemy
{
    public TrumpetFlower(GameObject obj) : base(obj)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyAttr(EnemyType.TrumpetFlower);
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(TrumpetFlowerIdleState));
    }
}

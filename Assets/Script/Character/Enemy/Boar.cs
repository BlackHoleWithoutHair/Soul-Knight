using UnityEngine;

public class Boar : IEnemy
{
    public Boar(GameObject obj) : base(obj)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyAttr(EnemyType.Boar);
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(BoarIdleState));
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
    }
    protected override void OnCharacterDieStart()
    {
        base.OnCharacterDieStart();
    }
}

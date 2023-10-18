using UnityEngine;

public class Stake : IEnemy
{
    public Stake(GameObject obj) : base(obj)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyAttr(EnemyType.Stake);
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(StakeIdleState));
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
    }
    protected override void OnCharacterDieStart()
    {
        base.OnCharacterDieStart();
    }
    public override void UnderAttack(int damage)
    {
        base.UnderAttack(damage);
        m_StateController.SetOtherState(typeof(StakeBeAttackState));
    }
}

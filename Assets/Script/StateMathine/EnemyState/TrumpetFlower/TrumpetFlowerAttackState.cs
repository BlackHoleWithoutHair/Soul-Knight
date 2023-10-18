using UnityEngine;

public class TrumpetFlowerAttackState : EnemyState
{
    private EnemyWeaponShareAttribute m_WeaponAttr;
    public TrumpetFlowerAttackState(EnemyStateController controller) : base(controller)
    {
        m_WeaponAttr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.TrumpetFlower);
    }
    public override void GameStart()
    {
        base.GameStart();
        m_Animator.SetBool("isAttack", true);
        CoroutinePool.Instance.StartCoroutine(Warning(0.4f, () =>
        {
            for (int i = 0; i < 9; i++)
            {
                EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet2, m_WeaponAttr, m_GameObject.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))).AddToController();
            }
            m_Controller.SetOtherState(typeof(TrumpetFlowerIdleState));
        }), this);
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
    }
    public override void GameExit()
    {
        base.GameExit();
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
}

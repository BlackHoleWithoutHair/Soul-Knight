using UnityEngine;

public class TrumpetFlowerAttackState : EnemyState
{
    private EnemyWeaponShareAttribute m_WeaponAttr;
    public TrumpetFlowerAttackState(EnemyStateController controller) : base(controller)
    {
        m_WeaponAttr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.TrumpetFlower);
    }
    protected override void StateStart()
    {
        base.StateStart();
        m_Animator.SetBool("isIdle", false);
        CoroutinePool.Instance.StartCoroutine(Warning(0.4f, () =>
        {
            for (int i = 0; i < 9; i++)
            {
                EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet2, m_WeaponAttr, m_Character.m_Attr.GetShareAttr(), m_GameObject.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))).AddToController();
            }
            m_Controller.SetOtherState(typeof(TrumpetFlowerIdleState));
        }), this);
    }
}

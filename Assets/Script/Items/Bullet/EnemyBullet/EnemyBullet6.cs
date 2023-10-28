using System.Collections;
using UnityEngine;

public class EnemyBullet6 : IEnemyBullet
{
    public EnemyBullet6(GameObject obj) : base(obj)
    {
        type = EnemyBulletType.EnemyBullet6;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CoroutinePool.Instance.StartCoroutine(CreateBulletLoop(), this);
    }
    private IEnumerator CreateBulletLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            EnemyBulletShareAttr attr = new EnemyBulletShareAttr();
            attr.DebuffType = BuffType.None;
            attr.Damage = 2;
            attr.Speed = 8;
            ItemPool.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet2, attr, transform.position, m_Rot * Quaternion.Euler(0, 0, 90)).AddToController();
            ItemPool.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet2, attr, transform.position, m_Rot * Quaternion.Euler(0, 0, -90)).AddToController();
        }
    }
}

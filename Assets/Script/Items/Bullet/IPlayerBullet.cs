using UnityEngine;

public class IPlayerBullet : IBullet
{
    protected PlayerBulletType type;
    public IPlayerBullet( GameObject obj, PlayerWeaponShareAttribute attr) : base(obj, attr)
    {
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, gameObject, "Enemy", OnHitEnemy);
    }
    private void OnHitEnemy(GameObject obj)
    {
        isHitWall = true;
        IEnemy enemy = obj.GetComponent<Symbol>().GetCharacter() as IEnemy;
        HurtEnemy(enemy);

    }
    public override void Remove()
    {
        base.Remove();
        if (!isDestroyOnRemove)
        {
            pool.ReturnItem(type, this);
        }
    }
    private void HurtEnemy(IEnemy enemy)
    {
        if (Random.Range(0, 100) <= (m_Attr as PlayerWeaponShareAttribute).CriticalRate)
        {
            GameMediator.Instance.GetController<BuffController>().AddBuff(enemy, m_Attr.DebuffType);

            enemy.UnderAttack(m_Attr.Damage * 2);
        }
        else
        {
            enemy.UnderAttack(m_Attr.Damage);
        }
    }
}

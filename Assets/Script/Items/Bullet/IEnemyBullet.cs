using UnityEngine;

public abstract class IEnemyBullet : IBullet
{
    protected EnemyBulletType type;
    public IEnemyBullet(GameObject obj,EnemyWeaponShareAttribute attr) : base(obj, attr)
    {
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, gameObject, "Player", OnHitPlayer);
    }
    protected override void AfterHitWallStart()
    {
        base.AfterHitWallStart();
        OnBulletParticleAppear();

    }
    protected virtual void OnBulletParticleAppear()
    {
        IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1, gameObject.transform.position);
        boom.SetColor(Color.red);
        boom.AddToController();
    }
    private void OnHitPlayer(GameObject obj)
    {
        PlayerAttribute attr = obj.transform.GetComponent<Symbol>().GetCharacter().m_Attr as PlayerAttribute;
        if (attr.HurtInvincibleTimer >= attr.m_ShareAttr.HurtInvincibleTime)
        {
            isHitWall = true;
            (obj.transform.GetComponent<Symbol>().GetCharacter() as IPlayer).UnderWeaponAttack(m_Attr as EnemyWeaponShareAttribute);
        }
    }
    public override void Remove()
    {
        base.Remove();
        if (!isDestroyOnRemove)
        {
            pool.ReturnItem(type, this);
        }
    }
}

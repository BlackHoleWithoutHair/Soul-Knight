using UnityEngine;

public abstract class IEnemyBullet : IBullet
{
    protected EnemyBulletType type;
    protected new IEnemyWeapon m_Weapon { get => base.m_Weapon as IEnemyWeapon; set => base.m_Weapon = value; }
    public IEnemyBullet(GameObject obj) : base(obj)
    {
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, gameObject, "Player", (obj) =>
        {
            PlayerAttribute attr = obj.transform.GetComponent<Symbol>().GetCharacter().m_Attr as PlayerAttribute;
            if (attr.HurtInvincibleTimer >= attr.m_ShareAttr.HurtInvincibleTime)
            {
                OnHitWall();
                (obj.transform.GetComponent<Symbol>().GetCharacter() as IPlayer).UnderAttack(m_Attr.Damage);
            }
        });
    }
    protected override void OnHitWall()
    {
        base.OnHitWall();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Remove();
        OnBulletParticleAppear();

    }
    protected virtual void OnBulletParticleAppear()
    {
        IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1, gameObject.transform.position);
        boom.SetColor(Color.red);
        boom.AddToController();
    }
}

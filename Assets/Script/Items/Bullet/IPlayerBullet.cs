using UnityEngine;

public class IPlayerBullet : IBullet
{
    protected new PlayerBulletShareAttr m_Attr { get => base.m_Attr as PlayerBulletShareAttr; set => base.m_Attr = value; }
    protected new IPlayerWeapon m_Weapon { get => base.m_Weapon as IPlayerWeapon; set => base.m_Weapon = value; }
    protected PlayerBulletType type;
    protected int ReboundTimes;
    private TalentSystem talentSystem;
    public IPlayerBullet(GameObject obj) : base(obj)
    {
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, gameObject, "Enemy", (obj) =>
        {
            OnHitCharacter();
            IEnemy enemy = obj.GetComponent<Symbol>().GetCharacter() as IEnemy;
            HurtEnemy(enemy);
        });
    }
    public override void OnEnter()
    {
        base.OnEnter();
        talentSystem = GameMediator.Instance.GetSystem<TalentSystem>();
        if (m_Weapon!=null)
        {
            if (talentSystem.GetTalent(TalentType.BulletRebound, m_Weapon.m_Character as IPlayer) != null)
            {
                ReboundTimes += 1;
            }
        }
    }
    protected override void OnHitWall()
    {
        base.OnHitWall();
        ReboundTimes -= 1;
        if(ReboundTimes<0)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Remove();
        }
        else
        {
            RaycastHit2D hit= Physics2D.Raycast(transform.position, m_Rot * Vector2.right, 1f, LayerMask.GetMask("Obstacle"));
            SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, Vector2.Reflect(m_Rot * Vector2.right, hit.normal))));
            SetIsHitWall();
        }
    }
    protected override void OnExit()
    {
        base.OnExit();
        if(!isDestroyOnRemove)
        {
            pool.ReturnItem(type, this);
        }
        ReboundTimes = 0;
    }
    private void HurtEnemy(IEnemy enemy)
    {
        if (Random.Range(0, 100) <= m_Attr.CriticalRate)
        {
            GameMediator.Instance.GetController<BuffController>().AddBuff(enemy, m_Attr.DebuffType);
            enemy.UnderAttack(m_Attr.Damage * 2);
            if(m_Weapon!=null)
            {
                if(talentSystem.GetTalent(TalentType.BulletPenetrate,m_Weapon.m_Character as IPlayer)==null)
                {
                    Remove();
                }
            }
        }
        else
        {
            enemy.UnderAttack(m_Attr.Damage);
            Remove();
        }
    }
}

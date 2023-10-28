
public class AttributeAdapter : Singleton<AttributeAdapter>
{
    private AttributeAdapter() { }
    public IBulletShareAttribute PlayerWeaponShareAttrToBulletShareAttr(PlayerWeaponShareAttribute attr)
    {
        PlayerBulletShareAttr playerAttr = new PlayerBulletShareAttr();
        playerAttr.Damage = attr.Damage;
        playerAttr.DebuffType = attr.DebuffType;
        playerAttr.Speed = attr.Speed;
        playerAttr.CriticalRate = attr.CriticalRate;
        return playerAttr;
    }
    public IBulletShareAttribute EnemyWeaponShareAttrToBulletShareAttr(EnemyWeaponShareAttribute attr, EnemyShareAttr shareAttr)
    {
        EnemyBulletShareAttr enemyAttr = new EnemyBulletShareAttr(); ;
        enemyAttr.DebuffType = attr.DebuffType;
        enemyAttr.Speed = attr.Speed;
        enemyAttr.Damage = shareAttr.Damage;
        return enemyAttr;
    }
}

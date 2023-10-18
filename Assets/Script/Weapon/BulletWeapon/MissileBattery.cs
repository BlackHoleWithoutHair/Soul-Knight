using UnityEngine;

public class MissileBattery : IPlayerUnAccumulateWeapon
{
    public MissileBattery(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.MissileBattery);
    }
    protected override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    protected override void OnFire()
    {
        base.OnFire();
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_4, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_4, m_Attr, FirePoint.transform.position, Quaternion.Euler(0, 0, 30) * GetShotRot()).AddToController();
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_4, m_Attr, FirePoint.transform.position, Quaternion.Euler(0, 0, -30) * GetShotRot()).AddToController();
    }
}

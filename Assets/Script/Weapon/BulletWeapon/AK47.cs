using UnityEngine;

public class AK47 : IPlayerUnAccumulateWeapon
{
    public AK47(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.AK47);
    }
    protected override void OnFire()
    {
        base.OnFire();
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_1, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

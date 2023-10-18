using UnityEngine;

public class TheCode : IPlayerUnAccumulateWeapon
{
    public TheCode(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.TheCode);
        CanBeRotated = false;
    }
    protected override void OnFire()
    {
        base.OnFire();
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_6, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

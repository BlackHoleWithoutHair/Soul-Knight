using UnityEngine;

public class TheCodePlus : IPlayerUnAccumulateWeapon
{
    public TheCodePlus(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.TheCodePlus);
        CanBeRotated = false;
    }
    protected override void OnFire()
    {
        base.OnFire();
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_5, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();

    }
}

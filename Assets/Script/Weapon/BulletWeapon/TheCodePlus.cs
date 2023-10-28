using UnityEngine;

public class TheCodePlus : IPlayerUnAccumulateWeapon
{
    public TheCodePlus(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.TheCodePlus);
        CanBeRotated = false;
    }
    protected override void OnFire()
    {
        base.OnFire();
        CreateBullet(PlayerBulletType.Bullet_5, m_Attr).AddToController();

    }
}

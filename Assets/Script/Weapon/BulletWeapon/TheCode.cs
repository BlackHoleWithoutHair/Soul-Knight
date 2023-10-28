using UnityEngine;

public class TheCode : IPlayerUnAccumulateWeapon
{
    public TheCode(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.TheCode);
        CanBeRotated = false;
    }
    protected override void OnFire()
    {
        base.OnFire();
        CreateBullet(PlayerBulletType.Bullet_6, m_Attr).AddToController();
    }
}

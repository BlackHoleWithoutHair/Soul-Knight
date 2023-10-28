using UnityEngine;

public class CrimsonWineGlass : IPlayerUnAccumulateWeapon
{
    public CrimsonWineGlass(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.CrimsonWineGlass);
        CanBeRotated = false;
    }
    protected override void OnFire()
    {
        base.OnFire();
        CreateBullet(PlayerBulletType.Bullet_7, m_Attr).AddToController();
    }
}

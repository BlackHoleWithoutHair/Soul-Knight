using UnityEngine;

public class DesertEagle : IPlayerUnAccumulateWeapon
{
    public DesertEagle(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.DesertEagle);
    }
    protected override void OnFire()
    {
        base.OnFire();
        PlayRecoilAnim();
        CreateBullet(PlayerBulletType.Bullet_7, m_Attr).AddToController();
    }
}

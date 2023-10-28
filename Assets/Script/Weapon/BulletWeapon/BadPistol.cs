using UnityEngine;

public class BadPistol : IPlayerUnAccumulateWeapon
{
    public BadPistol(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.BadPistol);
    }
    protected override void OnFire()
    {
        base.OnFire();
        ShowFireSpark(BulletColorType.Yellow);
        PlayRecoilAnim();
        CreateBullet(PlayerBulletType.Bullet_4, m_Attr).AddToController();
    }

}

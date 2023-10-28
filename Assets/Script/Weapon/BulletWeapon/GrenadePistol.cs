using UnityEngine;

public class GrenadePistol : IPlayerUnAccumulateWeapon
{
    public GrenadePistol(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.GrenadePistol);
    }
    protected override void OnFire()
    {
        base.OnFire();
        ShowFireSpark(BulletColorType.Yellow);
        PlayRecoilAnim();
        CreateBullet(PlayerBulletType.Bullet_9, m_Attr).AddToController();
    }
}

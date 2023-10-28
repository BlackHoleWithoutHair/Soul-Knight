using UnityEngine;

public class RainbowGatling : IPlayerUnAccumulateWeapon
{
    public RainbowGatling(GameObject obj, ICharacter player) : base(obj, player)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.RainbowGatling);
    }
    protected override void OnFire()
    {
        base.OnFire();
        PlayRecoilAnim();

        PlayerWeaponShareAttribute attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.RainbowGatling);
        attr.DebuffType = BuffType.Burn;
        IBullet bullet = CreateBullet(PlayerBulletType.Bullet_8,attr);
        bullet.SetColor(BulletColorType.Red);
        bullet.AddToController();

        bullet = CreateBullet(PlayerBulletType.Bullet_8, attr);
        bullet.SetColor(BulletColorType.Yellow);
        bullet.AddToController();

        attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.RainbowGatling);
        attr.DebuffType = BuffType.Poisoning;
        bullet = CreateBullet(PlayerBulletType.Bullet_8, attr);
        bullet.SetColor(BulletColorType.Green);
        bullet.AddToController();

        attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.RainbowGatling);
        attr.DebuffType = BuffType.Freeze;
        bullet = CreateBullet(PlayerBulletType.Bullet_8, attr);
        bullet.SetColor(BulletColorType.Cyan);
        bullet.AddToController();
    }
}

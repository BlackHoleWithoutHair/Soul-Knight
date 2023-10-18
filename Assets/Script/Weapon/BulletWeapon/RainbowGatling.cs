using UnityEngine;

public class RainbowGatling : IPlayerUnAccumulateWeapon
{
    public RainbowGatling(GameObject obj, ICharacter player) : base(obj, player)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.RainbowGatling);
    }
    protected override void OnFire()
    {
        base.OnFire();

        PlayerWeaponShareAttribute attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.RainbowGatling);
        attr.DebuffType = BuffType.Burn;
        IBullet bullet = EffectFactory.Instance.GetPlayerBullet(PlayerBulletType.Bullet_8, attr, FirePoint.transform.position, GetShotRot());
        bullet.SetColor(UnityTool.Instance.GetBulletColor(BulletColorType.Red));
        bullet.AddToController();

        bullet = EffectFactory.Instance.GetPlayerBullet(PlayerBulletType.Bullet_8, m_Attr, FirePoint.transform.position, GetShotRot());
        bullet.SetColor(UnityTool.Instance.GetBulletColor(BulletColorType.Yellow));
        bullet.AddToController();

        attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.RainbowGatling);
        attr.DebuffType = BuffType.Poisoning;
        bullet = EffectFactory.Instance.GetPlayerBullet(PlayerBulletType.Bullet_8, attr, FirePoint.transform.position, GetShotRot());
        bullet.SetColor(UnityTool.Instance.GetBulletColor(BulletColorType.Green));
        bullet.AddToController();

        attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.RainbowGatling);
        attr.DebuffType = BuffType.Freeze;
        bullet = EffectFactory.Instance.GetPlayerBullet(PlayerBulletType.Bullet_8, attr, FirePoint.transform.position, GetShotRot());
        bullet.SetColor(UnityTool.Instance.GetBulletColor(BulletColorType.Cyan));
        bullet.AddToController();
    }
}

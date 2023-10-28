using UnityEngine;

public class SnowFoxL : IPlayerUnAccumulateWeapon
{
    public SnowFoxL(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.SnowFoxL);
    }
    protected override void OnFire()
    {
        base.OnFire();
        ShowFireSpark(BulletColorType.Cyan);
        PlayRecoilAnim();
        CreateBullet(PlayerBulletType.Bullet_2, m_Attr).AddToController();
    }
}

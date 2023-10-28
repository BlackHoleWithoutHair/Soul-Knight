using UnityEngine;

public class AK47 : IPlayerUnAccumulateWeapon
{
    public AK47(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.AK47);
    }
    protected override void OnFire()
    {
        base.OnFire();
        ShowFireSpark(BulletColorType.Yellow);
        PlayRecoilAnim();
        CreateBullet(PlayerBulletType.Bullet_1, m_Attr).AddToController();
    }
}

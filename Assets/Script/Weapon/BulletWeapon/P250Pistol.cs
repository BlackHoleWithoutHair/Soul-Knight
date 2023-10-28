using UnityEngine;

public class P250Pistol : IPlayerUnAccumulateWeapon
{
    public P250Pistol(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.P250Pistol);
    }
    protected override void OnFire()
    {
        base.OnFire();
        ShowFireSpark(BulletColorType.Yellow);
        PlayRecoilAnim();
        CreateBullet(PlayerBulletType.ShabbyBullet, m_Attr).AddToController();
    }
}

using UnityEngine;

public class UZI : IPlayerUnAccumulateWeapon
{
    public UZI(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.UZI);
    }
    protected override void OnFire()
    {
        base.OnFire();
        ShowFireSpark(BulletColorType.Yellow);
        PlayRecoilAnim();
        CreateBullet(PlayerBulletType.Bullet_3, m_Attr).AddToController();
        
    }
}

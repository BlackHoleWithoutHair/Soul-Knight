using UnityEngine;

public class AssaultRifle : IPlayerUnAccumulateWeapon
{
    public AssaultRifle(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.AssaultRifle);
    }
    protected override void OnFire()
    {
        base.OnFire(); 
        ShowFireSpark(BulletColorType.Yellow);
        PlayRecoilAnim();
        CreateBullet(PlayerBulletType.Bullet_7, m_Attr).AddToController();
    }
}

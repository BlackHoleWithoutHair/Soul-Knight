using UnityEngine;

public class MissileBattery : IPlayerUnAccumulateWeapon
{
    public MissileBattery(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.MissileBattery);
    }
    protected override void OnFire()
    {
        base.OnFire();
        PlayRecoilAnim();
        for(int i=-1;i<=1;i++)
        {
            CreateBullet(PlayerBulletType.Bullet_4, m_Attr, i).AddToController();
        }
    }
}

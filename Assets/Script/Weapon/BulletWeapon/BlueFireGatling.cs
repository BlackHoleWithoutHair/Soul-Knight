using UnityEngine;

public class BlueFireGatling : IPlayerUnAccumulateWeapon
{
    public BlueFireGatling(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.BlueFireGatling);
    }
    protected override void OnFire()
    {
        base.OnFire();
        PlayRecoilAnim();
        for (int i = -2; i <= 2; i++)
        {
            if (i != 0)
            {
                IBullet bullet = CreateBullet(PlayerBulletType.Bullet_8,m_Attr);
                bullet.SetColor(BulletColorType.Yellow);
                bullet.AddToController();
            }
        }
    }
}

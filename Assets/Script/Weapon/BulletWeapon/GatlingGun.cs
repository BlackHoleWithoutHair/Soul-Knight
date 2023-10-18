using UnityEngine;

public class GatlingGun : IPlayerUnAccumulateWeapon
{
    public GatlingGun(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.GatlingGun);
    }
    protected override void OnFire()
    {
        base.OnFire();
        for (int i = -2; i <= 2; i++)
        {
            if (i != 0)
            {
                IBullet bullet = ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_8, m_Attr, FirePoint.transform.position, Quaternion.Euler(0, 0, i * 10) * GetShotRot());
                bullet.SetColor(Color.yellow);
                bullet.AddToController();
            }
        }
    }
}
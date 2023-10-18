using UnityEngine;

public class Shotgun : IEnemyWeapon
{
    public Shotgun(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.Shotgun);
    }
    protected override void OnEnter()
    {
        base.OnEnter();
    }
    protected override void OnFire()
    {
        base.OnFire();
        for (int angle = -30; angle <= 30; angle += 15)
        {
            EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet3, m_Attr, FirePoint.transform.position, GetShotRotation() * Quaternion.Euler(0, 0, angle)).AddToController();
        }
    }
}

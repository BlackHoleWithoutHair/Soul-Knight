using UnityEngine;

public class Blowpipe : IEnemyWeapon
{
    public Blowpipe(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.Blowpipe);
    }
    protected override void OnFire()
    {
        base.OnFire();
        EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet4, m_Attr, FirePoint.transform.position, GetShotRotation()).AddToController();
    }
}

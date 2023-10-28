using UnityEngine;

public class Handgun : IEnemyWeapon
{
    private int ShotTimes;
    private bool isFire;
    public Handgun(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.Handgun);
    }
    protected override void OnFire()
    {
        base.OnFire();
        ShotTimes = 0;
        Cumulate = 1f / m_Attr.FireRate;
        isFire = true;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isFire)
        {
            Cumulate += Time.deltaTime;
            if (Cumulate > 1f / m_Attr.FireRate)
            {
                Cumulate = 0;
                ShotTimes += 1;
                IBullet bullet = EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet1, m_Attr, m_Character.m_Attr.GetShareAttr() as EnemyShareAttr, FirePoint.transform.position, GetShotRotation());
                bullet.AddToController();
            }
            if (ShotTimes == 2)
            {
                isFire = false;
            }
        }
    }
}

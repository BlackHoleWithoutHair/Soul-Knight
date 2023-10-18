using UnityEngine;

public class BadPistol : IPlayerUnAccumulateWeapon
{

    public BadPistol(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.BadPistol);
    }
    protected override void OnEnter()
    {
        base.OnEnter();

    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    protected override void OnFire()
    {
        base.OnFire();
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_4, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

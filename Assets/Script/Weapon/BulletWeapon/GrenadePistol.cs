using UnityEngine;

public class GrenadePistol : IPlayerUnAccumulateWeapon
{
    public GrenadePistol(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.GrenadePistol);
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
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_9, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

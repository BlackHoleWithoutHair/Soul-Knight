using UnityEngine;

public class DesertEagle : IPlayerUnAccumulateWeapon
{
    public DesertEagle(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.DesertEagle);
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
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_7, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

using UnityEngine;

public class SnowFoxL : IPlayerUnAccumulateWeapon
{
    public SnowFoxL(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.SnowFoxL);
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
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_2, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

using UnityEngine;

public class UZI : IPlayerUnAccumulateWeapon
{
    public UZI(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.UZI);
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
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_3, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

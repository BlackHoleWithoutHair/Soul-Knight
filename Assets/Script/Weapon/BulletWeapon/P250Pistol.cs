using UnityEngine;

public class P250Pistol : IPlayerUnAccumulateWeapon
{
    public P250Pistol(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.P250Pistol);
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
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.ShabbyBullet, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

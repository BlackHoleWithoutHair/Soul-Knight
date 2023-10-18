using UnityEngine;

public class Furnace : IPlayerUnAccumulateWeapon
{
    public Furnace(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.Furnace);
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
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.FireBullet, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

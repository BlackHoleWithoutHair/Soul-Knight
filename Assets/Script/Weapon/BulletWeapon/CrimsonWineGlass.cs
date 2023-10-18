using UnityEngine;

public class CrimsonWineGlass : IPlayerUnAccumulateWeapon
{
    public CrimsonWineGlass(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.CrimsonWineGlass);
        CanBeRotated = false;
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
    //protected override Quaternion GetShotRot()
    //{
    //    return Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_GameObject.transform.position;
    //}
}

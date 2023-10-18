using UnityEngine;

public class PKP : IPlayerUnAccumulateWeapon
{
    // 射出8发子弹后，射速增加到10发每秒，偏移降为5
    //15发后伤害增加到4
    public PKP(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.PKP);
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
        ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_1, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

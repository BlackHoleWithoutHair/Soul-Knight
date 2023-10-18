using UnityEngine;

public class PKP : IPlayerUnAccumulateWeapon
{
    // ���8���ӵ����������ӵ�10��ÿ�룬ƫ�ƽ�Ϊ5
    //15�����˺����ӵ�4
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

using UnityEngine;

public class DormantBubbleMachine : IPlayerUnAccumulateWeapon
{
    public DormantBubbleMachine(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.DormantBubbleMachine);
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
        for (int i = -2; i <= 2; i++)
        {
            ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_11, m_Attr, FirePoint.transform.position, GetRot(i * 6)).AddToController();
        }
    }
    private Quaternion GetRot(int angle)
    {
        return Quaternion.Euler(m_GameObject.transform.eulerAngles.x, m_GameObject.transform.eulerAngles.y, angle + m_GameObject.transform.eulerAngles.z + Random.Range(-m_Attr.ScatteringRate, m_Attr.ScatteringRate));
    }
}

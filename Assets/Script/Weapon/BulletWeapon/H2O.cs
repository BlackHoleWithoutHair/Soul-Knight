using System.Collections;
using UnityEngine;

public class H2O : IPlayerUnAccumulateWeapon
{
    private int BurstTimes;
    private float BurstInterval;
    public H2O(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.H2O);
        BurstTimes = 3;
        BurstInterval = 0.1f;
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
        CoroutinePool.Instance.StartCoroutine(BurstMode());
    }
    private IEnumerator BurstMode()
    {
        while (BurstTimes > 0)
        {
            BurstTimes--;
            ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_2, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
            yield return new WaitForSeconds(BurstInterval);
        }
        BurstTimes = 3;
    }
}

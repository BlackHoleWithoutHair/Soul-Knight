using System.Collections;
using UnityEngine;

public class NextNextNextGenSMG : IPlayerUnAccumulateWeapon
{
    private int BurstTimes;
    private float BurstInterval;
    public NextNextNextGenSMG(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.NextNextNextGenSMG);
        BurstTimes = 4;
        BurstInterval = 0.075f;
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
            ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_2, m_Attr, FirePoint.transform.position + Vector3.up * 0.25f, GetShotRot()).AddToController();
            ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_2, m_Attr, FirePoint.transform.position - Vector3.up * 0.25f, GetShotRot()).AddToController();
            yield return new WaitForSeconds(BurstInterval);
        }
        BurstTimes = 4;
    }
}

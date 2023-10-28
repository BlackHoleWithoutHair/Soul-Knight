using System.Collections;
using UnityEngine;

public class NextNextNextGenSMG : IPlayerUnAccumulateWeapon
{
    private int BurstTimes;
    private float BurstInterval;
    public NextNextNextGenSMG(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.NextNextNextGenSMG);
        BurstTimes = 4;
        BurstInterval = 0.075f;
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
            CreateBullet(PlayerBulletType.Bullet_2, m_Attr, FirePoint.transform.position + Vector3.up * 0.25f).AddToController();
            CreateBullet(PlayerBulletType.Bullet_2, m_Attr, FirePoint.transform.position - Vector3.up * 0.25f).AddToController();
            yield return new WaitForSeconds(BurstInterval);
        }
        BurstTimes = 4;
    }
}

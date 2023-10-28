using System.Collections;
using UnityEngine;

public class H2O : IPlayerUnAccumulateWeapon
{
    private int BurstTimes;
    private float BurstInterval;
    public H2O(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.H2O);
        BurstTimes = 3;
        BurstInterval = 0.1f;
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
            CreateBullet(PlayerBulletType.Bullet_2, m_Attr).AddToController();
            PlayRecoilAnim();
            ShowFireSpark(BulletColorType.Cyan);
            yield return new WaitForSeconds(BurstInterval);
        }
        BurstTimes = 3;
    }
}

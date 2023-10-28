using System.Collections;
using UnityEngine;

public class EagleOfIceAndFire : IPlayerUnAccumulateWeapon
{
    private float BurstInterval;
    public EagleOfIceAndFire(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.EagleOfIceAndFire);
        BurstInterval = 0.15f;
    }
    protected override void OnFire()
    {
        base.OnFire();
        CoroutinePool.Instance.StartCoroutine(BurstMode());
    }
    private IEnumerator BurstMode()
    {
        IPlayerBullet bullet = null;
        PlayerWeaponShareAttribute attr = UnityTool.Instance.DeepCopyByReflection(m_Attr);
        attr.DebuffType = BuffType.Freeze;
        CreateBullet(PlayerBulletType.Bullet_8, attr);
        bullet.SetColor(BulletColorType.Blue);
        bullet.AddToController();
        ShowFireSpark(BulletColorType.Blue);
        PlayRecoilAnim();
        yield return new WaitForSeconds(BurstInterval);
        attr = UnityTool.Instance.DeepCopyByReflection(m_Attr);
        attr.DebuffType = BuffType.Burn;
        CreateBullet(PlayerBulletType.Bullet_8, attr);
        bullet.SetColor(BulletColorType.Red);
        bullet.AddToController();
        ShowFireSpark(BulletColorType.Red);
        PlayRecoilAnim();
        yield return new WaitForSeconds(BurstInterval);
    }
}

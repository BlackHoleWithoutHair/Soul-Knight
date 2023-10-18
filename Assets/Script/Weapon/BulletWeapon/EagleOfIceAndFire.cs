using System.Collections;
using UnityEngine;

public class EagleOfIceAndFire : IPlayerUnAccumulateWeapon
{
    private float BurstInterval;
    public EagleOfIceAndFire(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.EagleOfIceAndFire);
        BurstInterval = 0.15f;
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
        IBullet bullet = null;
        PlayerWeaponShareAttribute attr = UnityTool.Instance.DeepCopyByReflection(m_Attr);
        bullet = ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_8, attr, FirePoint.transform.position, GetShotRot());
        bullet.m_Attr.DebuffType = BuffType.Freeze;
        bullet.SetColor(new Color(0, 255, 255));
        bullet.AddToController();
        yield return new WaitForSeconds(BurstInterval);
        attr = UnityTool.Instance.DeepCopyByReflection(m_Attr);
        bullet = ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Bullet_8, attr, FirePoint.transform.position, GetShotRot());
        bullet.m_Attr.DebuffType = BuffType.Burn;
        bullet.SetColor(new Color(204f / 255f, 34f / 255f, 34f / 255f));
        bullet.AddToController();
        yield return new WaitForSeconds(BurstInterval);
    }
}

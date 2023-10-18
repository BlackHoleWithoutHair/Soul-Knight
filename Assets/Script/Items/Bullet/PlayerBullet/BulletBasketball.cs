using UnityEngine;

public class BulletBasketball : IPlayerBullet
{
    public BulletBasketball(GameObject obj, PlayerWeaponShareAttribute attr) : base(obj, attr)
    {
        type = PlayerBulletType.Basketball;
    }
    protected override void BeforeHitWallUpdate()
    {
        base.BeforeHitWallUpdate();
    }
    protected override void AfterHitWallStart()
    {
        base.AfterHitWallStart();
        IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1,gameObject.transform.position);
        boom.SetColor(Color.white);
        boom.AddToController();
    }
    protected override void AfterHitWallUpdate()
    {
        base.AfterHitWallUpdate();
    }
}

using UnityEngine;

public class BulletBasketball : IPlayerBullet
{
    public BulletBasketball(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Basketball;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        ReboundTimes = 3;
    }
    protected override void OnHitWall()
    {
        base.OnHitWall();
        CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.White);
    }
    protected override void OnHitCharacter()
    {
        base.OnHitCharacter();
        CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.White);
    }
}

using UnityEngine;

public class Arrow_1 : IPlayerBullet
{
    public Arrow_1(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Arrow_1;
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

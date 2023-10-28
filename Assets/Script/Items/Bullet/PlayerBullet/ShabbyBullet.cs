using UnityEngine;

public class ShabbyBullet : IPlayerBullet
{
    public ShabbyBullet(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.ShabbyBullet;
    }
    protected override void OnHitWall()
    {
        base.OnHitWall();
        CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.Yellow);
    }
    protected override void OnHitCharacter()
    {
        base.OnHitCharacter();
        CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.Yellow);
    }
}

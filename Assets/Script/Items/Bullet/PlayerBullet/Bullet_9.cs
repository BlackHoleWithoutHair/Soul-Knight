using UnityEngine;

public class Bullet_9 : IPlayerBullet
{
    public Bullet_9(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Bullet_9;
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

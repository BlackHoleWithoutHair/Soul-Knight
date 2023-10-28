using UnityEngine;

public class Bullet_1 : IPlayerBullet
{
    public Bullet_1(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Bullet_1;
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

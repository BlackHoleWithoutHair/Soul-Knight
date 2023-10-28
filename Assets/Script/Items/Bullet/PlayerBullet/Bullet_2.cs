using UnityEngine;

public class Bullet_2 : IPlayerBullet
{
    public Bullet_2(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Bullet_2;
    }
    protected override void OnHitWall()
    {
        base.OnHitWall();
        CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.Cyan);
    }
    protected override void OnHitCharacter()
    {
        base.OnHitCharacter();
        CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.Cyan);
    }
}

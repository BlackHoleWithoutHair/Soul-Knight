using UnityEngine;

public class Bullet_3 : IPlayerBullet
{
    public Bullet_3(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Bullet_3;
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

using UnityEngine;

public class Bullet_8 : IPlayerBullet
{
    public Bullet_8(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Bullet_8;
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

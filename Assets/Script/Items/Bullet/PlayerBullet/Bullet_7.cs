using UnityEngine;

public class Bullet_7 : IPlayerBullet
{
    public Bullet_7(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Bullet_7;
    }
    protected override void OnHitWall()
    {
        base.OnHitWall();
        CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.Red);
    }
    protected override void OnHitCharacter()
    {
        base.OnHitCharacter();
        CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.Red);
    }
}

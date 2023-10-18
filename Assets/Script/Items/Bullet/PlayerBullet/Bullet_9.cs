using UnityEngine;

public class Bullet_9 : IPlayerBullet
{
    public Bullet_9(GameObject obj, PlayerWeaponShareAttribute attr) : base(obj, attr)
    {
        type = PlayerBulletType.Bullet_9;
    }
    protected override void AfterHitWallStart()
    {
        base.AfterHitWallStart();
        IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1, gameObject.transform.position);
        boom.SetColor(new Color(1, 1, 0));
        boom.AddToController();
    }
    protected override void AfterHitWallUpdate()
    {
        base.AfterHitWallUpdate();
    }
}

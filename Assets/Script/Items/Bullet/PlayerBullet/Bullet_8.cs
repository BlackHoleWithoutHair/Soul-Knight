using UnityEngine;

public class Bullet_8 : IPlayerBullet
{
    public Bullet_8(GameObject obj, PlayerWeaponShareAttribute attr) : base(obj, attr)
    {
        type = PlayerBulletType.Bullet_8;
    }
    protected override void AfterHitWallStart()
    {
        base.AfterHitWallStart();
        IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1, gameObject.transform.position);
        boom.SetColor(new Color(1, 1, 0));
        boom.AddToController();
    }
}

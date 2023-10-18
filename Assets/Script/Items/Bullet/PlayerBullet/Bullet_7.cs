using UnityEngine;

public class Bullet_7 : IPlayerBullet
{
    public Bullet_7(GameObject obj, PlayerWeaponShareAttribute attr) : base(obj, attr)
    {
        type = PlayerBulletType.Bullet_7;
    }
    protected override void AfterHitWallStart()
    {
        base.AfterHitWallStart();
        IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1, gameObject.transform.position);
        boom.SetColor(new Color(0.4f, 0f, 0.06f));
        boom.AddToController();
    }
}

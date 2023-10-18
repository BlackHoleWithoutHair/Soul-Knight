using UnityEngine;

public class ShabbyBullet : IPlayerBullet
{
    public ShabbyBullet(GameObject obj, PlayerWeaponShareAttribute attr) : base(obj, attr)
    {
        type = PlayerBulletType.ShabbyBullet;
    }
    protected override void AfterHitWallStart()
    {
        base.AfterHitWallStart();
        IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1, gameObject.transform.position);
        boom.SetColor(new Color(1, 1, 0));
        boom.AddToController();
    }
}

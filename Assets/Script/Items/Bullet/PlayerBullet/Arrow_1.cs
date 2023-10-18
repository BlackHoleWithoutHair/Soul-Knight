using UnityEngine;

public class Arrow_1 : IPlayerBullet
{
    public Arrow_1(GameObject obj,PlayerWeaponShareAttribute attr) : base(obj, attr)
    {
        type = PlayerBulletType.Arrow_1;
    }
    protected override void AfterHitWallStart()
    {
        base.AfterHitWallStart();
        IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1, gameObject.transform.position);
        boom.SetColor(new Color(0, 0, 0));
        boom.AddToController();
    }
}

using UnityEngine;

public class Bullet_2 : IPlayerBullet
{
    public Bullet_2(GameObject obj, PlayerWeaponShareAttribute attr) : base(obj, attr)
    {
        type = PlayerBulletType.Bullet_2;
    }
    protected override void AfterHitWallStart()
    {
        base.AfterHitWallStart();
        IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1, gameObject.transform.position);
        boom.SetColor(new Color(0.266f, 0.66f, 0));
        boom.AddToController();
    }
}

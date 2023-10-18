using UnityEngine;
namespace MiddleScene
{
    public class FireBullet : IPlayerBullet
    {
        public FireBullet(GameObject obj, PlayerWeaponShareAttribute attr) : base(obj, attr)
        {
            type = PlayerBulletType.FireBullet;
        }
        protected override void AfterHitWallStart()
        {
            base.AfterHitWallStart();
            IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1, gameObject.transform.position);
            boom.SetColor(new Color(1, 0.4f, 0));
            boom.AddToController();
        }
    }
}


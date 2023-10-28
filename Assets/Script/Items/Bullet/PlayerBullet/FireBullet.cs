using UnityEngine;
namespace MiddleScene
{
    public class FireBullet : IPlayerBullet
    {
        public FireBullet(GameObject obj) : base(obj)
        {
            type = PlayerBulletType.FireBullet;
        }
        protected override void OnHitWall()
        {
            base.OnHitWall();
            CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.Orange);
        }
        protected override void OnHitCharacter()
        {
            base.OnHitCharacter();
            CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.Orange);
        }
    }
}


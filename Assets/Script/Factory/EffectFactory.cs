using MiddleScene;
using UnityEngine;


public class EffectFactory
{
    private static EffectFactory instance;
    public static EffectFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EffectFactory();
            }
            return instance;
        }
    }
    public EffectFactory() { }
    public IBullet GetPlayerBullet(PlayerBulletType type, PlayerWeaponShareAttribute attr, Vector2 pos, Quaternion rot)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayerBullet(type));
        if (obj.GetComponent<Symbol>() == null)
        {
            obj.AddComponent<Symbol>();
        }
        if (obj.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        IBullet bullet = null;
        switch (type)
        {
            case PlayerBulletType.ShabbyBullet:
                bullet = new ShabbyBullet(obj, attr);
                break;
            case PlayerBulletType.FireBullet:
                bullet = new FireBullet(obj, attr);
                break;
            case PlayerBulletType.Basketball:
                bullet = new BulletBasketball(obj, attr);
                break;
            case PlayerBulletType.Beam:
                bullet = new Beam(obj, attr);
                break;
            case PlayerBulletType.Arrow_1:
                bullet = new Arrow_1(obj, attr);
                break;
            case PlayerBulletType.Bullet_1:
                bullet = new Bullet_1(obj, attr);
                break;
            case PlayerBulletType.Bullet_2:
                bullet = new Bullet_2(obj, attr);
                break;
            case PlayerBulletType.Bullet_3:
                bullet = new Bullet_3(obj, attr);
                break;
            case PlayerBulletType.Bullet_4:
                bullet = new Bullet_4(obj, attr);
                break;
            case PlayerBulletType.Bullet_5:
                bullet = new Bullet_5(obj, attr);
                break;
            case PlayerBulletType.Bullet_6:
                bullet = new Bullet_6(obj, attr);
                break;
            case PlayerBulletType.Bullet_7:
                bullet = new Bullet_7(obj, attr);
                break;
            case PlayerBulletType.Bullet_8:
                bullet = new Bullet_8(obj, attr);
                break;
            case PlayerBulletType.Bullet_9:
                bullet = new Bullet_9(obj, attr);
                break;
            case PlayerBulletType.Bullet_10:
                bullet = new Bullet_10(obj, attr);
                break;
            case PlayerBulletType.Bullet_11:
                bullet = new Bullet_11(obj, attr);
                break;
        }
        bullet?.SetPosition(pos);
        bullet?.SetRotation(rot);
        return bullet;
    }
    public IBullet GetEnemyBullet(EnemyBulletType type, EnemyWeaponShareAttribute attr, Vector2 pos, Quaternion rot)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetEnemyBullet(type));
        if (obj.GetComponent<Symbol>() == null)
        {
            obj.AddComponent<Symbol>();
        }
        if (obj.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        IBullet bullet = null;
        switch (type)
        {
            case EnemyBulletType.EnemyBullet1:
                bullet = new EnemyBullet1(obj, attr);
                break;
            case EnemyBulletType.EnemyBullet2:
                bullet = new EnemyBullet2(obj, attr);
                break;
            case EnemyBulletType.EnemyBullet3:
                bullet = new EnemyBullet3(obj, attr);
                break;
            case EnemyBulletType.EnemyBullet4:
                bullet = new EnemyBullet4(obj, attr);
                break;
            case EnemyBulletType.EnemyBullet5:
                bullet = new EnemyBullet5(obj, attr);
                break;
            case EnemyBulletType.EnemyRedArrow:
                bullet = new EnemyRedArrow(obj, attr);
                break;
        }
        bullet?.SetPosition(pos);
        bullet?.SetRotation(rot);
        return bullet;
    }
    public ILaser GetLaser(LaserType type, PlayerWeaponShareAttribute attr, Vector2 pos, Quaternion rot)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetLaser(type));
        ILaser laser = null;
        switch (type)
        {
            case LaserType.BlueLaser:
                laser = new BlueLaser( obj, rot, attr);
                break;
        }
        laser?.SetPosition(pos);
        return laser;
    }
    public Item GetEffect(EffectType type, Vector2 pos, int damage)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetEffect(type));
        Item effect = null;
        switch (type)
        {
            case EffectType.DecHp:
                effect = new DecHp(obj);
                break;
            case EffectType.CriticalDecHp:
                effect = new CriticalDecHp(obj, damage);
                break;
        }
        effect.SetPosition(pos);
        return effect;
    }
    public IEffectBoom GetEffectBoom(EffectBoomType type, Vector2 pos)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetEffectBoom(type));
        IEffectBoom effect = null;
        switch (type)
        {
            case EffectBoomType.EffectBoom_1:
                effect = new EffectBoom_1(obj);
                break;
            case EffectBoomType.EffectBoom_2:
                effect = new EffectBoom_2( obj);
                break;
        }
        effect.SetPosition(pos);
        return effect;
    }
    public Item GetEffect(EffectType type, Vector2 pos)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetEffect(type));
        Item effect = null;
        switch (type)
        {
            case EffectType.Pane:
                effect = new Pane(obj);
                break;
            case EffectType.Slash:
                effect = new Slash(obj);
                break;
            case EffectType.AppearLight:
                effect = new PlayerAppearLight(obj);
                break;
            case EffectType.RecoveryMagicCircle:
                effect = new RecoveryMagicCircle(obj);
                break;
            case EffectType.PlayerPopupNum:
                effect = new PlayerPopupNum(obj);
                break;
        }
        effect?.SetPosition(pos);
        return effect;
    }

    public GameObject GetDebuff(BuffType type)
    {
        return Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetDebuff(type));
    }
    public Item GetFreezeEffect(float FreezeTime, Vector2 pos)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetDebuff(BuffType.Freeze));
        Item item = new Freeze(obj,FreezeTime);
        item.SetPosition(pos);
        return new Freeze(obj, FreezeTime);
    }
    public IKnifeLight GetKnifeLight(KnifeLightType type, PlayerWeaponShareAttribute attr, Vector2 pos, Quaternion rot)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetKnifeLight(type));
        IKnifeLight light = null;
        switch (type)
        {
            case KnifeLightType.KnifeLight:
                light = new KnifeLight(obj, rot, attr);
                break;
        }
        light.SetPosition(pos);
        return light;
    }
}

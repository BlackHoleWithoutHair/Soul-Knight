using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemPool
{
    private static ItemPool instance;
    public static ItemPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemPool();
            }
            return instance;
        }
    }
    private static GameObject BulletParent;
    private class ItemPoolInfo
    {
        public ObjectPool<Item> bulletPool;
        public delegate Item CreateItemDelegate();
        private CreateItemDelegate CreateItemMethod;
        public ItemPoolInfo(CreateItemDelegate createMethod)
        {
            CreateItemMethod = createMethod;
            bulletPool = new ObjectPool<Item>(CreatePoolItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10, 1000);
        }
        private Item CreatePoolItem()
        {
            Item bullet = OnGetBullet();
            bullet.gameObject.SetActive(false);
            bullet.gameObject.transform.SetParent(BulletParent.transform);
            return bullet;
        }
        private void OnReturnedToPool(Item bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        private void OnTakeFromPool(Item bullet)
        {
            bullet.gameObject.SetActive(true);
        }
        void OnDestroyPoolObject(Item bullet)
        {
            UnityEngine.Object.Destroy(bullet.gameObject);
        }
        protected Item OnGetBullet()
        {
            return CreateItemMethod.Invoke();
        }
    }
    private Dictionary<Enum, ItemPoolInfo> poolDic;
    public ItemPool()
    {
        InitInfos();
        EventCenter.Instance.RegisterObserver(EventType.OnSceneChangeComplete, () =>
        {
            InitInfos();
        });
    }
    public IBullet GetPlayerBullet(PlayerBulletType type, PlayerWeaponShareAttribute attr,IPlayerWeapon weapon, Vector2 pos, Quaternion rot)
    {
        if (poolDic.ContainsKey(type))
        {
            IPlayerBullet bullet = poolDic[type].bulletPool.Get() as IPlayerBullet;
            bullet.SetAttr(AttributeAdapter.Instance.PlayerWeaponShareAttrToBulletShareAttr(attr));
            bullet.SetPosition(pos);
            bullet.SetRotation(rot);
            bullet.SetPool(this);
            bullet.SetWeapon(weapon);
            return bullet;
        }
        return null;
    }
    public IKnifeLight GetPlayerKnifeLight(KnifeLightType type, PlayerWeaponShareAttribute attr, Vector2 pos, Quaternion rot)
    {
        if (poolDic.ContainsKey(type))
        {
            IKnifeLight bullet = poolDic[type].bulletPool.Get() as IKnifeLight;
            bullet.SetAttr(attr);
            bullet.SetPosition(pos);
            bullet.SetRotation(rot);
            bullet.SetPool(this);
            return bullet;
        }
        return null;
    }
    public IBullet GetEnemyBullet(EnemyBulletType type, PlayerWeaponShareAttribute attr, Vector2 pos, Quaternion rot)
    {
        if (poolDic.ContainsKey(type))
        {
            IBullet bullet = poolDic[type].bulletPool.Get() as IBullet;
            bullet.SetAttr(AttributeAdapter.Instance.PlayerWeaponShareAttrToBulletShareAttr(attr));
            bullet.SetPosition(pos);
            bullet.SetRotation(rot);
            bullet.SetPool(this);
            return bullet;
        }
        return null;
    }
    public IBullet GetEnemyBullet(EnemyBulletType type, IBulletShareAttribute attr, Vector2 pos, Quaternion rot)
    {
        if (poolDic.ContainsKey(type))
        {
            IBullet bullet = poolDic[type].bulletPool.Get() as IBullet;
            bullet.SetAttr(attr);
            bullet.SetPosition(pos);
            bullet.SetRotation(rot);
            bullet.SetPool(this);
            return bullet;
        }
        return null;
    }
    public IEffectBoom GetEffectBoom(EffectBoomType type, Vector2 pos)
    {
        if (poolDic.ContainsKey(type))
        {
            IEffectBoom boom = poolDic[type].bulletPool.Get() as IEffectBoom;
            boom.SetPosition(pos);
            boom.SetPool(this);
            return boom;
        }
        return null;
    }
    public Item GetItem(Enum type, Vector2 pos)
    {
        if (poolDic.ContainsKey(type))
        {
            Item item = poolDic[type].bulletPool.Get();
            item.SetPosition(pos);
            item.SetPool(this);
            return item;
        }
        return null;
    }
    public void ReturnItem(Enum type, Item boom)
    {
        if (poolDic.ContainsKey(type))
        {
            poolDic[type].bulletPool.Release(boom);
        }
    }

    private void InitInfos()
    {
        BulletParent = GameObject.Find("BulletPool");
        if (BulletParent == null)
        {
            BulletParent = new GameObject("BulletPool");
        }
        poolDic = new Dictionary<Enum, ItemPoolInfo>();

        foreach (PlayerBulletType type in Enum.GetValues(typeof(PlayerBulletType)))
        {
            ItemPoolInfo info = new ItemPoolInfo(() =>
            {
                return EffectFactory.Instance.GetPlayerBullet(type, null, Vector2.zero, Quaternion.identity);
            });
            poolDic.Add(type, info);
        }

        foreach (EnemyBulletType type in Enum.GetValues(typeof(EnemyBulletType)))
        {
            ItemPoolInfo info = new ItemPoolInfo(() =>
            {
                return EffectFactory.Instance.GetEnemyBullet(type, null, Vector2.zero, Quaternion.identity);
            });
            poolDic.Add(type, info);
        }

        foreach (EffectBoomType type in Enum.GetValues(typeof(EffectBoomType)))
        {
            ItemPoolInfo info = new ItemPoolInfo(() =>
            {
                return EffectFactory.Instance.GetEffectBoom(type, Vector2.zero);
            });
            poolDic.Add(type, info);
        }

        foreach (KnifeLightType type in Enum.GetValues(typeof(KnifeLightType)))
        {
            ItemPoolInfo info = new ItemPoolInfo(() =>
            {
                return EffectFactory.Instance.GetKnifeLight(type, null, Vector2.zero, Quaternion.identity);
            });
            poolDic.Add(type, info);
        }
        foreach (EffectType type in Enum.GetValues(typeof(EffectType)))
        {
            ItemPoolInfo info = new ItemPoolInfo(() =>
            {
                return EffectFactory.Instance.GetEffect(type, Vector2.zero);
            });
            poolDic.Add(type, info);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool : Singleton<GameObjectPool>
{
    private static GameObject BulletParent;
    public class GameObjectPoolInfo
    {
        public ObjectPool<GameObject> bulletPool;
        public GameObjectPoolInfo()
        {
            bulletPool = new ObjectPool<GameObject>(CreatePoolItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10, 1000);
        }
        private GameObject CreatePoolItem()
        {
            GameObject bullet = OnGetBullet();
            bullet.SetActive(false);
            bullet.transform.SetParent(BulletParent.transform);
            return bullet;
        }
        private void OnReturnedToPool(GameObject bullet)
        {
            bullet.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        private void OnTakeFromPool(GameObject bullet)
        {
            bullet.SetActive(true);
        }
        void OnDestroyPoolObject(GameObject bullet)
        {
            Object.Destroy(bullet);
        }
        protected virtual GameObject OnGetBullet()
        {
            return Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetEffect(EffectType.DecHp));
        }
    }

    private List<GameObjectPoolInfo> objInfos;
    private GameObjectPool()
    {
        InitPool();
        EventCenter.Instance.RegisterObserver(EventType.OnSceneChangeComplete, () =>
        {
            InitPool();
        });
    }

    public GameObject GetDecHp(Vector2 pos)
    {
        GameObject obj = objInfos[0].bulletPool.Get();
        obj.transform.position = pos;
        return obj;
    }
    public void ReturnGameObject(GameObject obj)
    {
        objInfos[0].bulletPool.Release(obj);
    }
    private void InitPool()
    {
        BulletParent = GameObject.Find("BulletPool");
        if (BulletParent == null)
        {
            BulletParent = new GameObject("BulletPool");
        }
        objInfos = new List<GameObjectPoolInfo>();
        objInfos.Add(new GameObjectPoolInfo());
    }
}

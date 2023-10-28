using System.Collections.Generic;
using UnityEngine;

public class EnemyDistribution
{
    public int stage;
    public List<EnemyType> types = new List<EnemyType>();
}
public class EnemyFactory
{
    private static EnemyFactory instance;
    public static EnemyFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyFactory();
            }
            return instance;
        }
    }
    private List<EnemyDistribution> distributions = new List<EnemyDistribution>();
    public EnemyFactory()
    {
        UnityTool.Instance.WriteEnemyDistributionFromTextAssetToList(distributions, ProxyResourceFactory.Instance.Factory.GetExcelTextAsset("ForestEnemyDistribution"));
    }
    public IEnemy GetEnemy(EnemyType type, bool isElite)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetEnemy(type));
        Transform parent = obj.transform.Find("GunOriginPoint");
        IEnemy enemy = null;
        EnemyWeaponType weaponType = EnemyWeaponType.None;
        switch (type)
        {
            case EnemyType.Stake:
                enemy = new Stake(obj);
                break;
            case EnemyType.TrumpetFlower:
                enemy = new TrumpetFlower(obj);
                break;
            case EnemyType.Boar:
                enemy = new Boar(obj);
                break;
            case EnemyType.DireBoar:
                enemy = new DireBoar(obj);
                break;
            case EnemyType.GunShark:
                enemy = new GunSharkEliteState(obj);
                break;
            case EnemyType.GoblinGuard:
                enemy = new GoblinGuard(obj);
                switch (Random.Range(0, 3))
                {
                    case 0:
                        weaponType = EnemyWeaponType.Handgun;
                        break;
                    case 1:
                        weaponType = EnemyWeaponType.Bow;
                        break;
                    case 2:
                        weaponType = EnemyWeaponType.Pike;
                        break;
                }
                break;
            case EnemyType.EliteGoblinGuard:
                enemy = new EliteGoblinGuard(obj);
                switch (Random.Range(0, 3))
                {
                    case 0:
                        weaponType = EnemyWeaponType.Shotgun;
                        break;
                    case 1:
                        weaponType = EnemyWeaponType.Blowpipe;
                        break;
                    case 2:
                        weaponType = EnemyWeaponType.Hoe;
                        break;
                }
                break;
            case EnemyType.GoblinGiant:
                enemy = new GoblinGiant(obj);
                weaponType = EnemyWeaponType.Hammer;
                break;
            case EnemyType.GoblinShaman:
                enemy = new GoblinShaman(obj);
                weaponType = EnemyWeaponType.GoblinMagicStaff;
                break;

        }
        if (weaponType != EnemyWeaponType.None)
        {
            enemy.AddWeapon(WeaponFactory.Instance.GetEnemyWeapon(weaponType, enemy));
        }
        enemy.m_Attr = AttributeFactory.Instance.GetEnemyAttr(type, isElite, weaponType);
        obj.GetComponent<Symbol>().SetCharacter(enemy);
        if (enemy == null)
        {
            Debug.Log("EnemyFactory GetEnemy " + type + " return null");
        }
        return enemy;
    }
    public IBoss GetBoss(BossType type)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetBoss(type));
        IBoss boss = null;
        switch (type)
        {
            case BossType.DevilSnare:
                boss = new DevilSnare(obj);
                break;
        }
        obj.GetComponent<Symbol>().SetCharacter(boss);
        return boss;
    }
    public IEnemy GetRandomEnemy()
    {
        int smallStage = MemoryModelCommand.Instance.GetSmallStage();
        bool isElite = Random.Range(0, 10) == 0 ? true : false;
        while (true)
        {
            foreach (EnemyDistribution distribution in distributions)
            {
                if (distribution.stage == smallStage)
                {
                    EnemyType type = distribution.types[Random.Range(0, distribution.types.Count)];
                    if (EnemyCommand.Instance.ContainState(type, isElite))
                    {
                        return GetEnemy(type, isElite);
                    }
                }
            }
        }
    }
    public IEnemy GetEliteEnemy()
    {
        int smallStage = MemoryModelCommand.Instance.GetSmallStage();
        while (true)
        {
            foreach (EnemyDistribution distribution in distributions)
            {
                if (distribution.stage == smallStage)
                {
                    EnemyType type = distribution.types[Random.Range(0, distribution.types.Count)];
                    if (EnemyCommand.Instance.ContainState(type, true))
                    {
                        return GetEnemy(type, true);
                    }
                }
            }
        }
    }
}

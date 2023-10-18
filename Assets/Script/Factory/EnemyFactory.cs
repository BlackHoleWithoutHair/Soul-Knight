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
    public IEnemy GetEnemy(EnemyType type)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetEnemy(type));
        Transform parent = obj.transform.Find("GunOriginPoint");
        IEnemy enemy = null;
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
                        enemy.AddWeapon(WeaponFactory.Instance.GetEnemyWeapon(EnemyWeaponType.Handgun, enemy));
                        break;
                    case 1:
                        enemy.AddWeapon(WeaponFactory.Instance.GetEnemyWeapon(EnemyWeaponType.Bow, enemy));
                        break;
                    case 2:
                        enemy.AddWeapon(WeaponFactory.Instance.GetEnemyWeapon(EnemyWeaponType.Pike, enemy));
                        break;
                }
                break;
            case EnemyType.EliteGoblinGuard:
                enemy = new EliteGoblinGuard(obj);
                switch (Random.Range(0, 3))
                {
                    case 0:
                        enemy.AddWeapon(WeaponFactory.Instance.GetEnemyWeapon(EnemyWeaponType.Shotgun, enemy));
                        break;
                    case 1:
                        enemy.AddWeapon(WeaponFactory.Instance.GetEnemyWeapon(EnemyWeaponType.Blowpipe, enemy));
                        break;
                    case 2:
                        enemy.AddWeapon(WeaponFactory.Instance.GetEnemyWeapon(EnemyWeaponType.Hoe, enemy));
                        break;
                }
                break;
            case EnemyType.GoblinGiant:
                enemy = new GoblinGiant(obj);
                enemy.AddWeapon(WeaponFactory.Instance.GetEnemyWeapon(EnemyWeaponType.Hammer, enemy));
                break;
            case EnemyType.GoblinShaman:
                enemy = new GoblinShaman(obj);
                enemy.AddWeapon(WeaponFactory.Instance.GetEnemyWeapon(EnemyWeaponType.GoblinMagicStaff, enemy));
                break;

        }
        obj.GetComponent<Symbol>().SetCharacter(enemy);
        if (enemy == null)
        {
            Debug.Log("EnemyFactory GetEnemy " + type + " return null");
        }
        return enemy;
    }
    public IEnemy GetRandomEnemyByStage(int Stage)
    {
        int smallStage = Stage - ((Stage / 5 + 1) - 1) * 5;
        foreach (EnemyDistribution distribution in distributions)
        {
            if (distribution.stage == smallStage)
            {
                return GetEnemy(distribution.types[Random.Range(0, distribution.types.Count)]);
            }
        }
        return null;
    }
}

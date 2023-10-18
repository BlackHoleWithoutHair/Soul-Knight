using System;
using System.IO;
using UnityEngine;

public class ArchiveUtility : Singleton<ArchiveUtility>
{
    private static string FileName = "SoulKnight ArchiveData";
    private static string KeyFileName = "KeyMappingData";
    private static string SysFileName = "SystemData";
    private string GamePath;
    private string KeyPath;
    private string SysPath;
    private ArchiveUtility()
    {
        GamePath = Path.Combine(Application.persistentDataPath, FileName + ".txt");
        KeyPath = Path.Combine(Application.persistentDataPath, KeyFileName + ".txt");
        SysPath = Path.Combine(Application.persistentDataPath, SysFileName + ".text");
    }
    public void SaveData<T>(T data)
    {
        string json = JsonUtility.ToJson(data);
        switch (typeof(T).Name)
        {
            case nameof(GameData):
                File.WriteAllText(GamePath, json);
                break;
            case nameof(SystemData):
                File.WriteAllText(SysPath, json);
                break;
            case "string":
                File.WriteAllText(KeyPath, json);
                break;
        }
        //Debug.Log("Success Save Data");
    }
    public T ReadData<T>() where T : class
    {
        T data = default;
        string json = null;
        switch (typeof(T).Name)
        {
            case nameof(SystemData):
                if (!File.Exists(SysPath))
                {
                    data = GetDefaultSysData() as T;
                    SaveData<T>(data);
                }
                json = File.ReadAllText(SysPath);
                return JsonUtility.FromJson<T>(json);

            case "string":
                if (!File.Exists(KeyPath))
                {
                    data = "" as T;
                    SaveData(data);
                }
                json = File.ReadAllText(KeyPath);
                return JsonUtility.FromJson<T>(json);

            case nameof(GameData):
                if (!File.Exists(GamePath))
                {
                    data = GetDefaultGameData() as T;
                    SaveData(data);
                }
                json = File.ReadAllText(GamePath);
                return JsonUtility.FromJson<T>(json);
        }
        return null;
    }
    private GameData GetDefaultGameData()
    {
        GameData data = new GameData();
        foreach (PlayerType type in Enum.GetValues(typeof(PlayerType)))
        {
            PlayerData playerData = new PlayerData();
            playerData.PlayerType = type;
            playerData.CurrentLv = 0;
            playerData.IsLocked = false;
            playerData.IsSkillLocked.Add(false);
            playerData.IsSkillLocked.Add(true);
            data.playerDatas.Add(playerData);
        }
        foreach (MaterialType type in Enum.GetValues(typeof(MaterialType)))
        {
            MaterialInfo info = new MaterialInfo();
            info.materialType = type;
            info.num = 5555;
            data.materialDatas.Add(info);
        }
        foreach (CouponsType type in Enum.GetValues(typeof(CouponsType)))
        {
            foreach (QualityType q in Enum.GetValues(typeof(QualityType)))
            {
                CouponData coupon = new CouponData();
                coupon.CouponType = type;
                coupon.CouponQuality = q;
                coupon.num = 1;
                data.couponDatas.Add(coupon);
            }
        }
        foreach (SeedType type in Enum.GetValues(typeof(SeedType)))
        {
            if (type == SeedType.None || type == SeedType.Seed) continue;
            SeedData seed = new SeedData();
            seed.SeedType = type;
            seed.num = 1;
            data.seedDatas.Add(seed);
        }
        for (int i = 0; i < 8; i++)
        {
            GardenInfo info = new GardenInfo();
            if (i > 2)
            {
                info.isLocked = true;
            }
            info.index = i;
            data.gardenInfos.Add(info);
        }
        data.Fish = 5;
        return data;
    }
    private SystemData GetDefaultSysData()
    {
        SystemData data = new SystemData();
        data.MainVolume = 0.8f;
        data.MusicVolume = 0.8f;
        data.SoundVolume = 0.8f;
        data.Language = LanguageType.Chinese;
        return data;
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("MyCommand/InitializeArchive")]
#endif
    public static void InitializeData()
    {
        Instance.SaveData(Instance.GetDefaultGameData());
        Instance.SaveData(Instance.GetDefaultSysData());
        Debug.Log("Already Init Data");
    }
#if UNITY_EDITOR
    [UnityEditor.MenuItem("MyCommand/DeleteArchive")]
#endif
    public static void DeleteDataFile()
    {
        string path = Path.Combine(Application.persistentDataPath, FileName + ".txt");
        File.Delete(path);
        Debug.Log("Already Delete Data");
    }
#if UNITY_EDITOR
    [UnityEditor.MenuItem("MyCommand/DeleteKeyData")]
#endif
    public static void DeleteKeyDataFile()
    {
        string path = Path.Combine(Application.persistentDataPath, KeyFileName + ".txt");
        File.Delete(path);
        Debug.Log("Already Delete KeyData");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeSpan
{
    public int days;
    public int hours;
    public int minutes;
    public int seconds;
    private System.TimeSpan timeSpan;
    public TimeSpan() { }
    public void Init(System.TimeSpan time)
    {
        timeSpan = time;
        days = time.Days;
        hours = time.Hours;
        minutes = time.Minutes;
        seconds = time.Seconds;
    }
    public float GetTotleDays()
    {
        UpdateTimeSpan();
        return (float)timeSpan.TotalDays;
    }
    public float GetTotleSeconds()
    {
        UpdateTimeSpan();
        return (float)timeSpan.TotalSeconds;
    }
    public void AddMinute(int s)
    {
        UpdateTimeSpan();
        timeSpan = timeSpan.Add(new System.TimeSpan(0, s, 0));
        Init(timeSpan);
    }
    public void AddSecond(int s)
    {
        UpdateTimeSpan();
        timeSpan = timeSpan.Add(new System.TimeSpan(0, 0, s));
        Init(timeSpan);
    }
    public void AddDay(int d)
    {
        UpdateTimeSpan();
        timeSpan = timeSpan.Add(new System.TimeSpan(24 * d, 0, 0));
        Init(timeSpan);
    }
    public void SubSecond(int s)
    {
        UpdateTimeSpan();
        timeSpan = timeSpan.Subtract(new System.TimeSpan(0, 0, s));
        Init(timeSpan);
    }
    public void SubDay(int d)
    {
        UpdateTimeSpan();
        timeSpan = timeSpan.Subtract(new System.TimeSpan(24 * d, 0, 0));
        Init(timeSpan);
    }
    public void SubMinute(int m)
    {
        UpdateTimeSpan();
        timeSpan = timeSpan.Subtract(new System.TimeSpan(0, m, 0));
        Init(timeSpan);
    }
    private void UpdateTimeSpan()
    {
        if (days + hours + minutes + seconds != 0)
        {
            if (timeSpan == System.TimeSpan.Zero)
            {
                timeSpan = new System.TimeSpan(days, hours, minutes, seconds);
            }
        }

    }
}
[System.Serializable]
public class DateTime
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    private System.DateTime time;
    public DateTime() { }
    public DateTime(System.DateTime time)
    {
        Init(time);
    }
    public void Init(System.DateTime time)
    {
        year = time.Year; month = time.Month;
        day = time.Day;
        hour = time.Hour; minute = time.Minute;
        this.time = time;
    }
    public System.DateTime GetTime()
    {
        if (year + month + day + hour + minute != 0)
        {
            if (time == System.DateTime.MinValue)
            {
                time = new System.DateTime(year, month, day, hour, minute, 0);
            }
        }
        return time;
    }
}
[System.Serializable]
public class PlantInfo
{
    public SeedType SeedType;
    public DateTime PlantTime;
    public TimeSpan GrowTime;
    public TimeSpan WetTime;
    public PlantInfo()
    {
        PlantTime = new DateTime();
        GrowTime = new TimeSpan();
        WetTime = new TimeSpan();
    }
}

[System.Serializable]
public class GardenInfo
{
    public int index;
    public bool isLocked;
    public PlantInfo plantInfo;
    public GardenInfo()
    {
        plantInfo = new PlantInfo();
    }
}

[System.Serializable]
public class SystemData
{
    public LanguageType Language;
    public int Stage;
    public float MainVolume;
    public float MusicVolume;
    public float SoundVolume;
}

[System.Serializable]
public class PlayerData
{
    public PlayerType PlayerType;
    public int CurrentLv;
    public bool IsLocked;
    public List<bool> IsSkillLocked = new List<bool>();
}

[System.Serializable]
public class CouponData
{
    public CouponsType CouponType;
    public QualityType CouponQuality;
    public int num;
}

[System.Serializable]
public class SeedData
{
    public SeedType SeedType;
    public int num;
}

[System.Serializable]
public class GameData
{
    public List<PlayerData> playerDatas = new List<PlayerData>();
    public List<MaterialInfo> materialDatas = new List<MaterialInfo>();
    public List<CouponData> couponDatas = new List<CouponData>();
    public List<SeedData> seedDatas = new List<SeedData>();
    public List<GardenInfo> gardenInfos = new List<GardenInfo>();
    public int Fish;
}
public class ArchiveModel : AbstractModel
{
    public SystemData SysData;
    public GameData GameData;
    public string KeyData;
    protected override void OnInit()
    {
        SysData = ArchiveUtility.Instance.ReadData<SystemData>();
        GameData = ArchiveUtility.Instance.ReadData<GameData>();
        KeyData = ArchiveUtility.Instance.ReadData<string>();
        CoroutinePool.Instance.StartCoroutine(AutoSaveLoop());
        EventCenter.Instance.RegisterObserver(EventType.OnSceneChangeComplete, () =>
        {
            CoroutinePool.Instance.StartCoroutine(AutoSaveLoop());
        });
    }
    public void SaveSystemData()
    {
        ArchiveUtility.Instance.SaveData(SysData);
    }
    public void SaveGameData()
    {
        ArchiveUtility.Instance.SaveData(GameData);
    }
    public void SaveKeyData()
    {
        ArchiveUtility.Instance.SaveData(KeyData);
    }
    private IEnumerator AutoSaveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            SaveGameData();
            SaveSystemData();
        }
    }
}

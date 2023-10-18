using System.Collections.Generic;

public class ArchiveQuery : Singleton<ArchiveQuery>
{
    private ArchiveModel model;
    private List<MaterialInfo> materialInfos;
    private List<CouponData> couponsDatas;
    private List<SeedData> seedDatas;
    private ArchiveQuery()
    {
        model = ModelContainer.Instance.GetModel<ArchiveModel>();
        materialInfos = model.GameData.materialDatas;
        couponsDatas = model.GameData.couponDatas;
        seedDatas = model.GameData.seedDatas;
    }
    public int GetMaterialNum(MaterialType type)
    {
        foreach (MaterialInfo info in materialInfos)
        {
            if (info.materialType == type)
            {
                return info.num;
            }
        }
        return 0;
    }
    public int GetCouponsNum(CouponsType type, QualityType quality)
    {
        foreach (CouponData data in couponsDatas)
        {
            if (data.CouponType == type && data.CouponQuality == quality)
            {
                return data.num;
            }
        }
        return 0;
    }
    public int GetSeedNum(SeedType type)
    {
        foreach (SeedData data in seedDatas)
        {
            if (data.SeedType == type)
            {
                return data.num;
            }
        }
        return 0;
    }
    public GardenInfo GetGardenInfo(int index)
    {
        foreach (GardenInfo info in model.GameData.gardenInfos)
        {
            if (info.index == index)
            {
                if (info.plantInfo.PlantTime.GetTime() != System.DateTime.MinValue)
                {
                    System.TimeSpan span = System.DateTime.Now - info.plantInfo.PlantTime.GetTime();
                    if (span.TotalDays > 1)
                    {
                        info.plantInfo.GrowTime.AddDay(1);
                        info.plantInfo.WetTime.SubDay(1);
                    }
                    else if (span.TotalDays > 0)
                    {
                        info.plantInfo.GrowTime.AddSecond((int)span.TotalSeconds);
                        info.plantInfo.WetTime.SubSecond((int)span.TotalSeconds);
                    }
                }
                return info;
            }
        }
        return null;
    }
    public bool isHavingPlant(int index)
    {
        foreach (GardenInfo info in model.GameData.gardenInfos)
        {
            if (info.index == index)
            {
                if (info.plantInfo.SeedType == SeedType.None)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public LanguageType GetLanguage()
    {
        return model.SysData.Language;
    }
}

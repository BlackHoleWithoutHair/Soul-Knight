using System.Collections.Generic;

public class ArchiveCommand : Singleton<ArchiveCommand>
{
    private ArchiveModel model;
    private List<MaterialInfo> materialInfos;
    private ArchiveCommand()
    {
        model = ModelContainer.Instance.GetModel<ArchiveModel>();
        materialInfos = model.GameData.materialDatas;
    }
    public void AddMaterial(MaterialType type)
    {
        for (int i = 0; i < materialInfos.Count; i++)
        {
            if (materialInfos[i].materialType == type)
            {
                materialInfos[i].num += 1;
            }
        }
    }
    public void AddMaterial(MaterialType type,int num)
    {
        for (int i = 0; i < materialInfos.Count; i++)
        {
            if (materialInfos[i].materialType == type)
            {
                materialInfos[i].num += num;
            }
        }
    }
    public void SpendMaterial(MaterialType type, int num)
    {
        for(int i =0;i<materialInfos.Count;i++)
        {
            if (materialInfos[i].materialType==type)
            {
                materialInfos[i].num -= num;
            }
        }
    }
    public int GetMaterialNum(MaterialType type)
    {
        for (int i = 0; i < materialInfos.Count; i++)
        {
            if (materialInfos[i].materialType == type)
            {
                return materialInfos[i].num;
            }
        }
        return 0;
    }
    public void RemovePlant(int index)
    {
        foreach(GardenInfo info in model.GameData.gardenInfos)
        {
            if (info.index == index)
            {
                info.plantInfo = new PlantInfo();
                model.SaveGameData();
                break;
            }
        }
    }
    public void Plant(SeedType type,int index)
    {
        foreach (GardenInfo info in model.GameData.gardenInfos)
        {
            if (info.index == index)
            {
                info.plantInfo = new PlantInfo();
                info.plantInfo.SeedType = type;
                info.plantInfo.PlantTime = new DateTime(System.DateTime.Now);
                model.SaveGameData();
                break;
            }
        }
    }
}

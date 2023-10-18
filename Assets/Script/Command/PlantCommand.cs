public class PlantCommand : Singleton<PlantCommand>
{
    private PlantModel model;
    private PlantCommand()
    {
        model = ModelContainer.Instance.GetModel<PlantModel>();
    }
    public PlantAttr GetPlantAttr(SeedType type)
    {
        foreach (PlantAttr attr in model.m_PlantAttrs)
        {
            if (attr.PlantType == type)
            {
                return attr;
            }
        }
        return null;
    }
    public string GetPlantNameByState(SeedType type, int num)
    {
        if (num == 0) return null;
        foreach (PlantAttr attr in model.m_PlantAttrs)
        {
            if (attr.PlantType == type)
            {
                return attr.PlantStates[num - 1];
            }
        }
        return null;
    }
    public float GetPlantNextStateTime(SeedType type, TimeSpan growTime)
    {
        foreach (PlantAttr attr in model.m_PlantAttrs)
        {
            if (attr.PlantType == type)
            {
                foreach (float t in attr.StateGrowDays)
                {
                    if (t > growTime.GetTotleDays())
                    {
                        return t;
                    }
                }
            }
        }
        return 0f;
    }
    public int GetStateNum(SeedType type)
    {
        int StateNum = 0;
        foreach (PlantAttr attr in model.m_PlantAttrs)
        {
            if (attr.PlantType == type)
            {
                foreach (string s in attr.PlantStates)
                {
                    if (s != "None")
                    {
                        StateNum++;
                    }
                }
            }
        }
        return StateNum;
    }
}
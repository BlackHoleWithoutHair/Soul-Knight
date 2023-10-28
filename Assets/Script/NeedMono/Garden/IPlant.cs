using System;
using System.Collections;
using UnityEngine;

public class IPlant
{
    private PlantInfo m_PlantInfo;
    public PlantState PlantState { get; private set; }
    private Garden m_Garden;
    private GameObject m_GameObject;
    private bool isInit;
    public IPlant(GameObject obj, Garden garden)
    {
        m_GameObject = obj;
        m_Garden = garden;
        m_PlantInfo = m_Garden.GardenInfo.plantInfo;
    }
    private void Init()
    {
        CoroutinePool.Instance.StartCoroutine(Timer(), this);
    }
    public void GameUpdate()
    {
        if (!isInit)
        {
            isInit = true;
            Init();
        }
    }
    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (m_PlantInfo.WetTime.GetTotleSeconds() > 0)
            {
                m_PlantInfo.GrowTime.AddSecond(1 * 43200);
                m_PlantInfo.WetTime.SubSecond(1);
                PlantState = PlantState.Great;
            }
            else
            {
                PlantState = PlantState.NeedWater;
            }
            if (PlantState != PlantState.Ripening)
            {
                int tmpState = 0;
                for (int i = 0; i < PlantCommand.Instance.GetStateNum(m_PlantInfo.SeedType); i++)
                {
                    if (m_PlantInfo.GrowTime.GetTotleDays() > PlantCommand.Instance.GetPlantAttr(m_PlantInfo.SeedType).StateGrowDays[i])
                    {
                        tmpState += 1;
                        SetPlantSprite(PlantCommand.Instance.GetPlantNameByState(m_PlantInfo.SeedType, tmpState));
                    }
                }
                if (tmpState >= PlantCommand.Instance.GetStateNum(m_PlantInfo.SeedType))
                {
                    PlantState = PlantState.Ripening;
                }
            }
        }
    }
    public void OnExit()
    {
        UnityEngine.Object.Destroy(m_GameObject);
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
    public PlantInfo GetPlantInfo()
    {
        return m_PlantInfo;
    }
    private void SetPlantSprite(string name)
    {
        m_GameObject.GetComponent<SpriteRenderer>().sprite = ProxyResourceFactory.Instance.Factory.GetPlantSprite(name);
    }
    public void SaveHarvest()
    {
        PlantAttr attr = PlantCommand.Instance.GetPlantAttr(m_PlantInfo.SeedType);
        switch (attr.HarvestType)
        {
            case HarvestType.Material:
                ArchiveCommand.Instance.AddMaterial(Enum.Parse<MaterialType>(attr.HarvestName), attr.HarvestNum);
                break;
            case HarvestType.Weapon:
                break;
            case HarvestType.Talent:
                break;
        }
    }
}

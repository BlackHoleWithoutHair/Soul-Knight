using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlantAttr
{
    public SeedType PlantType;
    public QualityType QualityType;
    public List<string> PlantStates;
    public List<float> StateGrowDays;
    public HarvestType HarvestType;
    public string HarvestName;
    public int HarvestNum;
    public bool isRenewable;
}
[CreateAssetMenu(fileName = "PlantData", menuName = "ScriptableObjects/PlantData")]
public class PlantScriptableObject : ScriptableObject
{
    public TextAsset textAsset;
    public List<PlantAttr> plantInfos;
    private void OnValidate()
    {
        UnityTool.Instance.WriteDataToListFromTextAssest(plantInfos, textAsset);
    }
}

using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MaterialInfo
{
    public MaterialType materialType;
    public int num;
}
[System.Serializable]
public class CompositionData
{
    public PlayerWeaponType weaponType;
    public List<MaterialInfo> materialInfos = new List<MaterialInfo>();
}
[CreateAssetMenu(fileName = "CompositionData", menuName = "ScriptableObjects/CompositionData")]
public class CompositionScriptableObject : ScriptableObject
{
    public TextAsset textAsset;
    public List<CompositionData> datas;
    private void OnValidate()
    {
        UnityTool.Instance.WriteCompositionDataFromTextToList(datas, textAsset);
    }
}

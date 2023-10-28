using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BossData", menuName = "ScriptableObjects/BossData")]
public class BossScriptableObject : ScriptableObject
{
    public TextAsset textAsset;
    public List<BossShareAttr> attrs = new List<BossShareAttr>();
    private void OnValidate()
    {
        UnityTool.Instance.WriteDataToListFromTextAssest(attrs, textAsset);
    }
}

using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyScriptableObject : ScriptableObject
{
    public TextAsset textAsset;
    public List<EnemyShareAttr> EnemyShareAttrs;
    private void OnValidate()
    {
        UnityTool.Instance.WriteDataToListFromTextAssest(EnemyShareAttrs, textAsset);
    }
}

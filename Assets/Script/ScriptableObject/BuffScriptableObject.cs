
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BuffData", menuName = "ScriptableObjects/BuffData")]
public class BuffScriptableObject : ScriptableObject
{
    public TextAsset textAsset;
    public List<BuffShareAttribute> BuffShareAttrs;
    private void OnValidate()
    {
        UnityTool.Instance.WriteDataToListFromTextAssest(BuffShareAttrs, textAsset);
    }
}

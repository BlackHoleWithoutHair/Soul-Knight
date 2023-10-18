using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillScriptableObject : ScriptableObject
{
    public TextAsset textAsset;
    public List<SkillShareAttribute> SkillShareAttrs;
    private void OnValidate()
    {
        UnityTool.Instance.WriteDataToListFromTextAssest(SkillShareAttrs, textAsset);
    }
}

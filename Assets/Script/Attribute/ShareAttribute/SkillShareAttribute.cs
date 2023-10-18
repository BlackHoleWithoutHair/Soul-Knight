using UnityEngine;

[System.Serializable]
public class SkillShareAttribute
{
    public SkillType Type;
    public float SkillDuration;
    public float SkillCoolTime;
    public string SkillName;
    [TextArea]
    public string SkillDescription;
    [TextArea]
    public string SeniorSkillDescription;
}

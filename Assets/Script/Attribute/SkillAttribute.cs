

public class SkillAttribute
{
    private SkillShareAttribute Attr;
    public SkillAttrStrategy m_ShareAttr;
    public int CurrentLv;
    public SkillAttribute(SkillShareAttribute attr, PlayerAttribute playerAttr)
    {
        Attr = attr;
        CurrentLv = playerAttr.CurrentLv;
        m_ShareAttr = new SkillAttrStrategy(this);
    }
    public SkillShareAttribute GetAttr()
    {
        return Attr;
    }
}
public class SkillAttrStrategy
{
    private SkillAttribute m_Attr;
    public SkillType Type => m_Attr.GetAttr().Type;
    public float SkillCoolTime
    {
        get
        {
            if (m_Attr.CurrentLv >= 4)
            {
                return m_Attr.GetAttr().SkillCoolTime - 2;
            }
            else
            {
                return m_Attr.GetAttr().SkillCoolTime;
            }
        }
    }
    public float SkillDuration => m_Attr.GetAttr().SkillDuration;
    public string SkillName => m_Attr.GetAttr().SkillName;
    public string SkillDescription => m_Attr.GetAttr().SkillDescription;
    public string SeniorSkillDescription => m_Attr.GetAttr().SeniorSkillDescription;
    public SkillAttrStrategy(SkillAttribute attr)
    {
        m_Attr = attr;
    }
}
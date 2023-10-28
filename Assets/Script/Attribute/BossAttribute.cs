

public class BossAttribute : CharacterAttribute
{
    public new BossAttrStrategy m_ShareAttr { get => base.m_ShareAttr as BossAttrStrategy; set => base.m_ShareAttr = value; }
    public BossAttribute(CharacterShareAttr attr) : base(attr)
    {
        m_ShareAttr = new BossAttrStrategy(this);
    }
    public new BossShareAttr GetShareAttr()
    {
        return base.GetShareAttr() as BossShareAttr;
    }
}
public class CharacterAttrStrategy
{
    protected CharacterAttribute m_Attr;
    public float Speed
    {
        get
        {
            return m_Attr.GetShareAttr().Speed * (1 - m_Attr.SpeedDecreaseFac);
        }
    }
    public bool IsIdleLeft => m_Attr.GetShareAttr().IsIdleLeft;
    public int MaxHp => m_Attr.GetShareAttr().MaxHp;
    public CharacterAttrStrategy(CharacterAttribute attr)
    {
        m_Attr = attr;
    }
}

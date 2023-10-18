public abstract class CharacterAttribute
{
    public CharacterAttrStrategy m_ShareAttr;
    public int CurrentHp;
    public bool isEnemy;
    public bool isFreeze;
    public bool isDizzy;
    public bool isPause;
    public float SpeedDecreaseFac;
    protected CharacterShareAttr m_Attr;
    public CharacterAttribute(CharacterShareAttr attr)
    {
        m_Attr = attr;
    }
    public CharacterShareAttr GetShareAttr()
    {
        return m_Attr;
    }
}

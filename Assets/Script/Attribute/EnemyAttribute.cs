public class EnemyAttribute : CharacterAttribute
{
    public new EnemyAttrStrategy m_ShareAttr { get => base.m_ShareAttr as EnemyAttrStrategy; set => base.m_ShareAttr = value; }
    public bool isAttack;
    public EnemyAttribute(EnemyShareAttr attr) : base(attr)
    {
        m_ShareAttr = new EnemyAttrStrategy(this);
        isEnemy = true;
    }
    public new EnemyShareAttr GetShareAttr()
    {
        return base.GetShareAttr() as EnemyShareAttr;
    }
}

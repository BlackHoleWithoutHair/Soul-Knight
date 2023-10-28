public abstract class ITalent
{
    public TalentType type;
    protected IPlayer m_Player;
    public ITalent(IPlayer character)
    {
        m_Player = character;
    }
    public virtual void OnObtain() { }
    public virtual void OnRemove() { }
}

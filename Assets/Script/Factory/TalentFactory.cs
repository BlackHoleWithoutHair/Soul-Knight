using System;

public enum TalentType
{
    Precision,
    BulletRebound,
    BulletPenetrate,
}
public class TalentFactory:Singleton<TalentFactory>
{
    private TalentFactory() { }
    public ITalent GetTalent(TalentType type,IPlayer player)
    {
        ITalent talent = (ITalent)Activator.CreateInstance(Type.GetType(type.ToString()), player);
        talent.type = type;
        return talent;
    }
}

public enum SkillType
{
    Roll,
    Cut,
    FireOnAllCylinders,
    FullScaleAttack,
    RecoveryMagicalCircle,
    None,
}
public class SkillFactory
{
    private static SkillFactory instance;
    public static SkillFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SkillFactory();
            }
            return instance;
        }
    }
    public SkillFactory() { }
    public ISkill GetSkill(SkillType type, IPlayer character)
    {
        ISkill skill = null;
        switch (type)
        {
            case SkillType.Roll:
                skill = new Roll(character);
                break;
            case SkillType.Cut:
                skill = new Cut(character);
                break;
            case SkillType.FireOnAllCylinders:
                skill = new FireOnAllCylinders(character);
                break;
            case SkillType.FullScaleAttack:
                
            case SkillType.RecoveryMagicalCircle:
                skill = new RecoveryMagicalCircle(character);
                break;
        }
        return skill;
    }
}

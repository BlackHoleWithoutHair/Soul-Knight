public class PlayerAttribute : CharacterAttribute
{
    public new PlayerAttrStrategy m_ShareAttr { get => base.m_ShareAttr as PlayerAttrStrategy; set => base.m_ShareAttr = value; }
    public IPlayerWeapon FirstWeapon;
    public IPlayerWeapon SecondWeapon;
    public SkinType CurrentSkinType;
    public SkillType CurrentSkillType;
    public int CurrentLv
    {
        get
        {
            foreach (PlayerData data in ModelContainer.Instance.GetModel<ArchiveModel>().GameData.playerDatas)
            {
                if (data.PlayerType == m_ShareAttr.Type)
                {
                    return data.CurrentLv;
                }
            }
            return 0;
        }
        set
        {
            foreach (PlayerData data in ModelContainer.Instance.GetModel<ArchiveModel>().GameData.playerDatas)
            {
                if (data.PlayerType == m_ShareAttr.Type)
                {
                    data.CurrentLv = value;
                    break;
                }
            }
        }
    }
    public int CurrentMp;
    public int CurrentArmor;
    public int HpIncrement;
    public int MpIncrement;
    public int ArmorIncrement;
    public int CriticalIncrement;
    public bool isInvincible;
    public bool isBattle;//Check if player is fighting
    public float ArmorRecoveryTimer;
    public float HurtArmorRecoveryTimer;
    public float HurtInvincibleTimer;
    public PlayerAttribute(PlayerShareAttr attr) : base(attr)
    {
        m_ShareAttr = new PlayerAttrStrategy(this);
        isEnemy = false;
    }
    public new PlayerShareAttr GetShareAttr()
    {
        return base.GetShareAttr() as PlayerShareAttr;
    }
}

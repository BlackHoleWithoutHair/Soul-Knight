using System.Collections.Generic;

public class PlayerAttribute : CharacterAttribute
{
    public new PlayerAttrStrategy m_ShareAttr { get => base.m_ShareAttr as PlayerAttrStrategy; set => base.m_ShareAttr = value; }
    public List<IPlayerWeapon> Weapons;
    public List<PlayerWeaponType> WeaponTypes;
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
    public int ScaterringDecrease;
    public bool isInvincible;
    public bool isBattle;//Check if player is fighting
    public float ArmorRecoveryTimer;
    public float HurtArmorRecoveryTimer;
    public float HurtInvincibleTimer;
    public float SkillCoolTimer;
    public PlayerAttribute(PlayerShareAttr attr) : base(attr)
    {
        m_ShareAttr = new PlayerAttrStrategy(this);
        Weapons = new List<IPlayerWeapon>();
        WeaponTypes = new List<PlayerWeaponType>() { m_ShareAttr.IdleWeapon};
        isEnemy = false;
    }
    public new PlayerShareAttr GetShareAttr()
    {
        return base.GetShareAttr() as PlayerShareAttr;
    }
}

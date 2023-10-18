using System.Collections.Generic;

public class PlayerAttrStrategy : CharacterAttrStrategy
{
    private new PlayerAttribute m_Attr { get => base.m_Attr as PlayerAttribute; set => base.m_Attr = value; }
    public new int MaxHp
    {
        get
        {
            if (m_Attr.CurrentLv >= 1)
            {
                return m_Attr.GetShareAttr().MaxHp + 1 + m_Attr.HpIncrement;
            }
            else
            {
                return m_Attr.GetShareAttr().MaxHp + m_Attr.HpIncrement;
            }
        }
    }
    public int Armor
    {
        get
        {
            if (m_Attr.CurrentLv >= 2)
            {
                return m_Attr.GetShareAttr().Armor + 1 + m_Attr.ArmorIncrement;
            }
            else
            {
                return m_Attr.GetShareAttr().Armor + m_Attr.ArmorIncrement;
            }
        }
    }
    public int Magic
    {
        get
        {
            if (m_Attr.CurrentLv >= 3)
            {
                return m_Attr.GetShareAttr().Magic + 20 + m_Attr.ArmorIncrement;
            }
            else
            {
                return m_Attr.GetShareAttr().Magic + m_Attr.ArmorIncrement;
            }
        }
    }
    public int Critical
    {
        get
        {
            return m_Attr.GetShareAttr().Critical + m_Attr.CriticalIncrement;
        }
    }
    public bool isIdleLeft => m_Attr.GetShareAttr().IsIdleLeft;
    public new float Speed
    {
        get
        {
            if (m_Attr.isBattle)
            {
                return m_Attr.GetShareAttr().FightingSpeed * (1 - m_Attr.SpeedDecreaseFac);
            }
            else if (ModelContainer.Instance.GetModel<SceneModel>().sceneType == SceneType.Battle)
            {
                return m_Attr.GetShareAttr().FinishFightingSpeed * (1 - m_Attr.SpeedDecreaseFac);
            }
            else
            {
                return m_Attr.GetShareAttr().Speed * (1 - m_Attr.SpeedDecreaseFac);
            }
        }
    }
    public PlayerType Type => m_Attr.GetShareAttr().Type;
    public PlayerWeaponType IdleWeapon => m_Attr.GetShareAttr().IdleWeapon;
    public string PlayerName => m_Attr.GetShareAttr().PlayerName;
    public List<SkillType> SkillTypes => m_Attr.GetShareAttr().SkillTypes;
    public List<SkinType> SkinTypes => m_Attr.GetShareAttr().SkinTypes;
    public float ArmorRecoveryTime => m_Attr.GetShareAttr().ArmorRecoveryTime;
    public float HurtArmorRecoveryTime => m_Attr.GetShareAttr().HurtArmorRecoveryTime;
    public float HurtInvincibleTime => m_Attr.GetShareAttr().HurtInvincibleTime;
    public PlayerAttrStrategy(PlayerAttribute attr) : base(attr) { }
}

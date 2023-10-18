using System.Collections.Generic;

[System.Serializable]
public class PlayerShareAttr : CharacterShareAttr
{
    public PlayerType Type;
    public PlayerWeaponType IdleWeapon;
    public List<SkinType> SkinTypes;
    public List<SkillType> SkillTypes;
    public string PlayerName;
    public int Armor;
    public int Magic;
    public int Critical;
    public int HandAttackDamage;
    public float FightingSpeed;
    public float FinishFightingSpeed;
    public float ArmorRecoveryTime;
    public float HurtArmorRecoveryTime;
    public float HurtInvincibleTime;
    public PlayerShareAttr() { }
}

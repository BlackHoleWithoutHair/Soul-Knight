using UnityEngine;

public class AttributeFactory : Singleton<AttributeFactory>
{
    private AttributeFactory() { }
    public PlayerAttribute GetPlayerAttr(PlayerType type)
    {
        return new PlayerAttribute(PlayerCommand.Instance.GetPlayerShareAttr(type));
    }
    public EnemyAttribute GetEnemyAttr(EnemyType type, bool isElite, EnemyWeaponType weaponType)
    {
        return new EnemyAttribute(EnemyCommand.Instance.GetEnemyShareAttr(type, isElite, weaponType));
    }
    public BossAttribute GetBossAttr(BossType type, BossCategory category)
    {
        return new BossAttribute(BossCommand.Instance.GetBossShareAttr(type, category));
    }
    public PlayerWeaponShareAttribute GetPlayerWeaponAttr(PlayerWeaponType type)
    {
        foreach (PlayerWeaponShareAttribute attr in ProxyResourceFactory.Instance.Factory.GetScriptableObject<WeaponScriptableObject>().WeaponShareAttrs)
        {
            if (attr.Type == type)
            {
                return attr;
            }
        }
        Debug.Log("AttributeFactory GetWeaponAttr return null");
        return null;
    }
    public EnemyWeaponShareAttribute GetEnemyWeaponAttr(EnemyWeaponType type)
    {
        foreach (EnemyWeaponShareAttribute attr in ProxyResourceFactory.Instance.Factory.GetScriptableObject<EnemyWeaponScriptableObject>().EnemyWeaponShareAttrs)
        {
            if (attr.Type == type)
            {
                return attr;
            }
        }
        Debug.Log("AttributeFactory GetEnemyWeaponAttr return null");
        return null;
    }
    public CompositionData GetCompositionData(PlayerWeaponType type)
    {
        foreach (CompositionData data in ProxyResourceFactory.Instance.Factory.GetScriptableObject<CompositionScriptableObject>().datas)
        {
            if (data.weaponType == type)
            {
                return data;
            }
        }
        Debug.Log("AttributeFactory GetCompositionData return null");
        return null;
    }
    public SkillAttribute GetSkillAttr(SkillType type, PlayerAttribute playerAttr)
    {
        foreach (SkillShareAttribute attr in ProxyResourceFactory.Instance.Factory.GetScriptableObject<SkillScriptableObject>().SkillShareAttrs)
        {
            if (attr.Type == type)
            {
                return new SkillAttribute(attr, playerAttr);
            }
        }
        Debug.Log("AttributeFactory GetSkillAttr return null");
        return null;
    }
    public DialogueData GetDialogue(PlayerType type)
    {
        foreach (DialogueData attr in ProxyResourceFactory.Instance.Factory.GetScriptableObject<PlayerDialogueScriptableObject>().DialogueDatas)
        {
            if (attr.PlayerType == type)
            {
                return attr;
            }
        }
        Debug.Log("AttributeFactory GetDialogue return null");
        return null;
    }
    public BuffShareAttribute GetBuffData(BuffType type)
    {
        foreach (BuffShareAttribute attr in ProxyResourceFactory.Instance.Factory.GetScriptableObject<BuffScriptableObject>().BuffShareAttrs)
        {
            if (attr.buffType == type)
            {
                return attr;
            }
        }
        Debug.Log("AttributeFactory GetBuffData return null");
        return null;
    }
}

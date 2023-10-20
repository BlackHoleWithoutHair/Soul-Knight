public enum BossCategory
{
    Normal,
    Elite,
    Challenge,
}
[System.Serializable]
public class BossShareAttr:CharacterShareAttr
{
    public BossType BossType;
    public BossCategory BossCategory;
}

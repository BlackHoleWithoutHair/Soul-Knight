using System.Collections.Generic;

public enum BossCategory
{
    Normal,
    Elite,
    Challenge,
}
[System.Serializable]
public class BossShareAttr : CharacterShareAttr
{
    public BossType BossType;
    public BossCategory BossCategory;
    public List<MaterialType> DropMaterials = new List<MaterialType>();
    public List<SeedType> DropSeeds = new List<SeedType>();
}

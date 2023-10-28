using System.Collections.Generic;

public class BossAttrStrategy : CharacterAttrStrategy
{
    protected new BossAttribute m_Attr { get => base.m_Attr as BossAttribute; set => base.m_Attr = value; }
    public List<MaterialType> DropMaterials => m_Attr.GetShareAttr().DropMaterials;
    public List<SeedType> DropSeeds => m_Attr.GetShareAttr().DropSeeds;
    public BossAttrStrategy(CharacterAttribute attr) : base(attr) { }
}

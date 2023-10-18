﻿public class EnemyAttrStrategy : CharacterAttrStrategy
{
    protected new EnemyAttribute m_Attr { get => base.m_Attr as EnemyAttribute; set => base.m_Attr = value; }
    public int Damage => m_Attr.GetShareAttr().Damage;
    public EnemyAttrStrategy(CharacterAttribute attr) : base(attr) { }
}

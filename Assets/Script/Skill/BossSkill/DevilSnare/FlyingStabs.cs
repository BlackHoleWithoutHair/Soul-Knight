

using UnityEngine;

public class FlyingStabs : IBossSkill
{
    private EnemyBulletShareAttr m_Attr;
    public FlyingStabs(ICharacter character) : base(character) { }
    protected override void OnInit()
    {
        base.OnInit();
        m_Attr = new EnemyBulletShareAttr();
        m_Attr.Damage = 4;
        m_Attr.Speed = 8;
        m_Attr.DebuffType = BuffType.None;
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        for (int i = 0; i < 6; i++)
        {
            ItemPool.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet6, m_Attr, m_Character.transform.position, Quaternion.Euler(0, 0, i * 60)).AddToController();
        }
    }
}

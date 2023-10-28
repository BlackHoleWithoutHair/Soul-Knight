using UnityEngine;

public class HollowBulletAttack : IBossSkill
{
    private EnemyBulletShareAttr m_Attr;
    public HollowBulletAttack(ICharacter character) : base(character) { }
    protected override void OnInit()
    {
        base.OnInit();
        m_Attr = new EnemyBulletShareAttr();
        m_Attr.Damage = 2;
        m_Attr.DebuffType = BuffType.None;
        m_Attr.Speed = 10;
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        for (int i = 0; i < 24; i++)
        {
            ItemPool.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet2, m_Attr, m_Character.transform.position, Quaternion.Euler(0, 0, i * 15)).AddToController();
        }
    }

}

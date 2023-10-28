using UnityEngine;

public class PiosoningNeedle : IBossSkill
{
    public PiosoningNeedle(ICharacter character) : base(character) { }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        for (int i = 0; i < 24; i++)
        {
            EnemyBulletShareAttr attr = new EnemyBulletShareAttr();
            attr.Damage = 3;
            attr.Speed = 8f;
            attr.DebuffType = BuffType.Poisoning;

            IEnemyBullet bullet = ItemPool.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet4, attr, m_Character.transform.position, Quaternion.Euler(0, 0, i * 15)) as IEnemyBullet;
            bullet.AddToController();
        }
        StopSkill();
    }
}

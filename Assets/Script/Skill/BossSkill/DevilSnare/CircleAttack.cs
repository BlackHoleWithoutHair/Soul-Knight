using System.Collections;
using UnityEngine;

public class CircleAttack : IBossSkill
{
    private float Timer;
    private EnemyBulletShareAttr m_Attr;
    public CircleAttack(ICharacter character) : base(character) { }
    protected override void OnInit()
    {
        base.OnInit();
        m_Attr = new EnemyBulletShareAttr();
        m_Attr.Damage = 3;
        m_Attr.Speed = 8;
        m_Attr.DebuffType = BuffType.None;
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        Timer = 0;
        CoroutinePool.Instance.StartCoroutine(AttackLoop(), this);
    }
    protected override void OnSkillDuration()
    {
        base.OnSkillDuration();
        Timer += Time.deltaTime;
    }
    private IEnumerator AttackLoop()
    {
        while (Timer < 3)
        {
            for (int i = 0; i < 4; i++)
            {
                ItemPool.Instance.GetEnemyBullet(EnemyBulletType.EnemyBullet1, m_Attr, m_Character.transform.position, Quaternion.Euler(0, 0, 240 * Timer + i * 90)).AddToController();
            }
            yield return new WaitForSeconds(0.1f);
        }
        StopSkill();
    }
}
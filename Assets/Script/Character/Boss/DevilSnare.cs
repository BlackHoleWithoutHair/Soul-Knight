using UnityEngine;

public class DevilSnare : IBoss
{
    private bool isAnger;
    public DevilSnare(GameObject obj) : base(obj)
    {
        m_Attr = AttributeFactory.Instance.GetBossAttr(BossType.DevilSnare, BossCategory.Normal);
    }
    protected override void OnInit()
    {
        base.OnInit();
        HitEffectRender = transform.Find("Root").GetComponent<SpriteRenderer>();
        skills.Add(new CircleAttack(this));
        skills.Add(new HollowBulletAttack(this));
        skills.Add(new PiosoningNeedle(this));
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(DevilSnareIdleState));
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        if (m_Attr.CurrentHp < m_Attr.m_ShareAttr.MaxHp / 2 && !isAnger)
        {
            isAnger = true;
            skills.Add(new FlyingStabs(this));
        }
    }
}

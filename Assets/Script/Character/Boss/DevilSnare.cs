using UnityEngine;

public class DevilSnare:IBoss
{
    public DevilSnare(GameObject obj) : base(obj) 
    {
        m_Attr = AttributeFactory.Instance.GetBossAttr(BossType.DevilSnare, BossCategory.Normal);
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(DevilSnareIdleState));
        m_Attr.CurrentHp = 510;
        HitEffectRender = transform.Find("Root").GetComponent<SpriteRenderer>();
    }
}

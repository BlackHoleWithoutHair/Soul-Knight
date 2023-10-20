using System.Collections;
using UnityEngine;

public class IBoss : ICharacter
{
    protected GameObject DecHpPoint;
    protected BossStateController m_StateController;
    protected SpriteRenderer HitEffectRender;
    private MaterialPropertyBlock block;
    public IBoss(GameObject obj) : base(obj) { }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController=new BossStateController(this);
        m_Animator=transform.GetComponent<Animator>();
        DecHpPoint = transform.Find("DecHpPoint")?.gameObject;
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        m_StateController?.GameUpdate();
    }
    public override void UnderAttack(int damage)
    {
        base.UnderAttack(damage);
        DecHp effect = ItemPool.Instance.GetItem(EffectType.DecHp, DecHpPoint.transform.position) as DecHp;
        effect.SetTextValue(damage);
        effect.DoAnimation();
        if(HitEffectRender!=null)
        {
            HitEffect();
        }
    }
    private void HitEffect()
    {
        HitEffectRender.GetPropertyBlock(block);
        block.SetColor("_Color", Color.white);
        HitEffectRender.SetPropertyBlock(block);
        CoroutinePool.Instance.StartCoroutine(ResumeColor());
    }
    private IEnumerator ResumeColor()
    {
        yield return new WaitForSeconds(1f / 12f);
        HitEffectRender.GetPropertyBlock(block);
        block.SetColor("_Color", Color.black);
        HitEffectRender.SetPropertyBlock(block);
    }
}
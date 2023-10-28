using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBoss : IEnemy
{
    public new BossAttribute m_Attr { get => base.m_Attr as BossAttribute; set => base.m_Attr = value; }
    public List<IBossSkill> skills { get; protected set; }
    protected BossStateController m_StateController;
    protected SpriteRenderer HitEffectRender;
    public IBoss(GameObject obj) : base(obj) { }
    protected override void OnInit()
    {
        base.OnInit();
        skills = new List<IBossSkill>();
        m_Animator = transform.GetComponent<Animator>();
        m_rb = transform.GetComponent<Rigidbody2D>();
        EventCenter.Instance.RegisterObserver(EventType.OnBossCutSceneFinish, () =>
        {
            m_Attr.isRun = true;
        });
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_Attr.CurrentHp = m_Attr.m_ShareAttr.MaxHp;
        m_StateController = new BossStateController(this);
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        m_StateController?.GameUpdate();
        foreach (IBossSkill skill in skills)
        {
            skill.OnUpdate();
        }
    }
    protected override void OnCharacterDieStart()
    {
        base.OnCharacterDieStart();
        foreach (IBossSkill skill in skills)
        {
            skill.StopSkill();
        }
        if (transform.GetComponent<CircleCollider2D>())
        {
            transform.GetComponent<CircleCollider2D>().isTrigger = true;
        }
        foreach (MaterialType type in m_Attr.m_ShareAttr.DropMaterials)
        {
            if (Random.Range(0, 100) < 70)
            {
                for (int i = 0; i < Random.Range(1, 5); i++)
                {
                    ItemFactory.Instance.GetMaterial(type, GameMediator.Instance.GetController<RoomController>().RandomPointInFloor(m_Room)).AddToController();
                }
            }
        }
        foreach (SeedType type in m_Attr.m_ShareAttr.DropSeeds)
        {
            if (Random.Range(0, 100) < 70)
            {
                for (int i = 0; i < Random.Range(1, 5); i++)
                {
                    ItemFactory.Instance.GetSeed(type, GameMediator.Instance.GetController<RoomController>().RandomPointInFloor(m_Room)).AddToController();
                }
            }
        }
        Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetItem("Portal"), transform.position + Vector3.up * 7, Quaternion.identity);
    }
    public override void UnderAttack(int damage)
    {
        base.UnderAttack(damage);
        if (HitEffectRender != null)
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
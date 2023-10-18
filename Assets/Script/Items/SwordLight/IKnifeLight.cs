using UnityEngine;

public class IKnifeLight : Item
{
    protected KnifeLightType type;
    protected Animator m_Animator;
    protected PlayerWeaponShareAttribute m_Attr;
    protected AnimatorStateInfo info;
    public IKnifeLight(GameObject obj, Quaternion rot, PlayerWeaponShareAttribute attr) : base( obj)
    {
        m_Attr = attr;
        m_Rot = rot;
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, gameObject, "Enemy", OnHitCharacter);
    }
    protected override void Init()
    {
        base.Init();
        m_Animator = gameObject.GetComponent<Animator>();

    }
    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.transform.rotation = m_Rot;
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        info = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime > 1)
        {
            Remove();
        }
    }
    public override void Remove()
    {
        base.Remove();
        if (pool != null)
        {
            pool.ReturnItem(type, this);
        }
    }
    protected virtual void OnHitCharacter(GameObject obj)
    {
        Debug.Log(1);
        obj.GetComponent<Symbol>().GetCharacter().UnderAttack(m_Attr.Damage);
    }
    public void SetAttr(PlayerWeaponShareAttribute attr)
    {
        m_Attr = attr;
    }
}
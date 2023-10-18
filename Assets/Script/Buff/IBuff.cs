using UnityEngine;
public abstract class IBuff
{
    public BuffShareAttribute m_Attr;
    public GameObject DivDebuff;
    public int level;
    public float durationTimer;
    public float PlayerIntervalTimer;
    public float EnemtIntervalTimer;
    public bool CanBeRemove;
    protected ICharacter m_Owner;
    protected bool isPlayerType;
    public IBuff(ICharacter character)
    {
        m_Owner = character;
        if (m_Owner is IPlayer)
        {
            isPlayerType = true;
        }
        DivDebuff = UnityTool.Instance.GetGameObjectFromChildren(m_Owner.gameObject, "DivDebuff");
    }
    public virtual void Execute()
    {
        if (m_Attr.buffType != BuffType.Freeze)
        {
            DivDebuff?.transform.Find(m_Attr.buffType.ToString()).gameObject.SetActive(true);
        }
    }
    public virtual void OnRemove()
    {
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
        if (m_Attr.buffType != BuffType.Freeze)
        {
            DivDebuff?.transform.Find(m_Attr.buffType.ToString()).gameObject.SetActive(false);
        }
    }
    protected void HurtOwner()
    {
        if (isPlayerType)
        {
            m_Owner.UnderAttack(m_Attr.PlayerDamage);
        }
        else
        {
            m_Owner.UnderAttack(m_Attr.EnemyDamage);
        }
    }
}

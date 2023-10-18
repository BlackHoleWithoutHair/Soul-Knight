using System.Collections;
using UnityEngine;

public class BuffFreeze : IBuff
{
    public BuffFreeze(ICharacter character) : base(character)
    {

    }
    public override void Execute()
    {
        base.Execute();
        m_Owner.m_Attr.isFreeze = true;
        FreezeHurt();
        CoroutinePool.Instance.StartCoroutine(WaitForRemove());
    }
    private void FreezeHurt()
    {
        Item effect = null;
        if (isPlayerType)
        {
            effect = EffectFactory.Instance.GetFreezeEffect(m_Attr.PlayerDuration, m_Owner.gameObject.transform.position);
        }
        else
        {
            effect = EffectFactory.Instance.GetFreezeEffect(m_Attr.EnemyDuration, m_Owner.gameObject.transform.position);
        }

        if (m_Owner.gameObject.transform.Find("FreezeScale"))
        {
            effect.gameObject.transform.SetParent(m_Owner.gameObject.transform.Find("FreezeScale"));
        }
        else
        {
            effect.gameObject.transform.SetParent(m_Owner.gameObject.transform);
        }
        effect.AddToController();
    }
    public IEnumerator WaitForRemove()
    {
        if (isPlayerType)
        {
            yield return new WaitForSeconds(m_Attr.PlayerDuration);
        }
        else
        {
            yield return new WaitForSeconds(m_Attr.EnemyDuration);
        }
        m_Owner.m_Attr.isFreeze = false;
        CanBeRemove = true;
    }
}

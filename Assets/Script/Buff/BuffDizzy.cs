using System.Collections;
using UnityEngine;

public class BuffDizzy : IBuff
{
    public BuffDizzy(ICharacter character) : base(character)
    {

    }
    public override void Execute()
    {
        base.Execute();
        m_Owner.m_Attr.isDizzy = true;
        CoroutinePool.Instance.StartCoroutine(WaitForRemove(), this);
    }
    private IEnumerator WaitForRemove()
    {
        if (isPlayerType)
        {
            yield return new WaitForSeconds(m_Attr.PlayerDuration);
        }
        else
        {
            yield return new WaitForSeconds(m_Attr.EnemyDuration);
        }
        m_Owner.m_Attr.isDizzy = false;
        CanBeRemove = true;
    }
}

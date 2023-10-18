using System.Collections;
using UnityEngine;

public class BuffBurn : IBuff
{
    public BuffBurn(ICharacter character) : base(character)
    {

    }
    public override void Execute()
    {
        base.Execute();
        CoroutinePool.Instance.StartCoroutine(WaitForRemove(), this);
        CoroutinePool.Instance.StartCoroutine(BurnHurt(), this);
    }
    private IEnumerator BurnHurt()
    {
        HurtOwner();
        while (true)
        {
            HurtOwner();
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator WaitForRemove()
    {
        if (isPlayerType)
        {
            yield return new WaitForSeconds(m_Attr.PlayerDuration + 0.1f);
        }
        else
        {
            yield return new WaitForSeconds(m_Attr.EnemyDuration + 0.1f);
        }
        CanBeRemove = true;
    }
    public override void OnRemove()
    {
        base.OnRemove();
    }
}

using System.Collections;
using UnityEngine;

public class BuffPoisoning : IBuff
{
    public BuffPoisoning(ICharacter character) : base(character) { }
    public override void Execute()
    {
        base.Execute();
        CoroutinePool.Instance.StartCoroutine(PoisoningHurt(), this);
        CoroutinePool.Instance.StartCoroutine(WaitForRemove(), this);
        m_Owner.m_Attr.SpeedDecreaseFac += 0.3f;
    }
    public override void OnRemove()
    {
        base.OnRemove();
        m_Owner.m_Attr.SpeedDecreaseFac -= 0.3f;
    }
    private IEnumerator PoisoningHurt()
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
}

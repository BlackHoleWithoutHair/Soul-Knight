using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RecoveryMagicCircle : Item
{
    private float LifeTime;
    private Collider2D[] colliders;
    private bool isFinish;
    public RecoveryMagicCircle(GameObject obj) : base(obj) { }
    public override void OnEnter()
    {
        base.OnEnter();
        isFinish = false;
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.DOScale(Vector3.one, 0.3f);
        CoroutinePool.Instance.StartCoroutine(WaitForDestroy());
        CoroutinePool.Instance.StartCoroutine(Treating());
    }
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(LifeTime);
        isFinish = true;
        gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0), 0.3f).OnComplete(() =>
        {
            Remove();
        });
    }
    private IEnumerator Treating()
    {
        while (!isFinish)
        {
            colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1.875f);
            foreach (Collider2D c in colliders)
            {
                if (c.TryGetComponent(out Symbol symbol))
                {
                    if (!symbol.GetCharacter().m_Attr.isEnemy)
                    {
                        if (c.CompareTag("Player"))
                        {
                            symbol.GetCharacter().UnderTreating(1);
                        }
                        else
                        {
                            symbol.GetCharacter().UnderTreating(4);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.8f);
        }
    }

    public void SetLifeTime(float LifeTime)
    {
        this.LifeTime = LifeTime;
    }
}

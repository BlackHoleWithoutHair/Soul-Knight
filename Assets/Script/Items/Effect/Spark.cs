
using System.Collections;
using UnityEngine;

public class Spark:Item
{
    private SpriteRenderer render;
    public Spark(GameObject obj) : base(obj) 
    {
        render = transform.GetComponent<SpriteRenderer>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        CoroutinePool.Instance.StartCoroutine(WaitForRemove(), this);
    }
    protected override void OnExit()
    {
        base.OnExit();
        if (pool != null)
        {
            pool.ReturnItem(EffectType.Spark, this);
        }
    }
    private IEnumerator WaitForRemove()
    {
        yield return new WaitForSeconds(1f / 12f);
        Remove();
    }
    public void SetColor(BulletColorType type)
    {
        render.color = UnityTool.Instance.GetBulletColor(type);
    }
}

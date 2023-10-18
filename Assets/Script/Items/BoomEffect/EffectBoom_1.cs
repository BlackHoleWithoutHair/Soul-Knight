using System.Collections;
using UnityEngine;

public class EffectBoom_1 : IEffectBoom
{
    public EffectBoom_1(GameObject obj) : base(obj)
    {
        type = EffectBoomType.EffectBoom_1;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        CoroutinePool.Instance.StartCoroutine(WaitForDestroy());
    }
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Remove();
    }
}

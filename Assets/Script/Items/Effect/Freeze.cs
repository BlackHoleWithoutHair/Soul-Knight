using System.Collections;
using UnityEngine;

public class Freeze : Item
{
    private float FreezeTime;
    public Freeze(GameObject obj, float FreezeTime) : base(obj)
    {
        this.FreezeTime = FreezeTime;
    }
    protected override void Init()
    {
        base.Init();
        gameObject.transform.position = position;
        CoroutinePool.Instance.StartCoroutine(FreezeTimer());
        gameObject.transform.localScale = Vector3.one;
    }
    private IEnumerator FreezeTimer()
    {
        yield return new WaitForSeconds(FreezeTime);
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        gameObject.transform.SetParent(null);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Remove();
    }
}

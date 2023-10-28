using System.Collections;
using System.Data;
using UnityEngine;

public class EnemyBullet2 : IEnemyBullet
{
    private float fac;
    private float DecreaseFac;
    private float RandomSpeed;
    public EnemyBullet2(GameObject obj) : base(obj)
    {
        type = EnemyBulletType.EnemyBullet2;
        DecreaseFac = Time.deltaTime * 0.5f;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        CoroutinePool.Instance.StartCoroutine(WaitForDestroy(), this);
        fac = 1.0f;
        RandomSpeed = Random.Range(0.7f, 1) * m_Attr.Speed;
    }
    protected override void BeforeHitObstacleUpdate()
    {
        gameObject.transform.position += RandomSpeed * (m_Rot * Vector2.right) * Time.deltaTime * fac;
        fac = Mathf.Clamp(fac - DecreaseFac, 0.1f, 1f);
    }
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(3f);
        Remove();
    }
    protected override void OnExit()
    {
        base.OnExit();
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
    public void SetSpeedDecreaseFac(float fac)
    {
        DecreaseFac = fac;
    }
}

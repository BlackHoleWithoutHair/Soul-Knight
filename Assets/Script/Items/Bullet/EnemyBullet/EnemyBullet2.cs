using System.Collections;
using UnityEngine;

public class EnemyBullet2 : IEnemyBullet
{
    private float fac;
    private float RandomSpeed;
    public EnemyBullet2(GameObject obj, EnemyWeaponShareAttribute attr) : base(obj, attr)
    {
        type = EnemyBulletType.EnemyBullet2;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        CoroutinePool.Instance.StartCoroutine(WaitForDestroy(), this);
        fac = 1.0f;
        RandomSpeed = Random.Range(0.5f, 1) * m_Attr.Speed;
    }
    protected override void BeforeHitWallUpdate()
    {
        gameObject.transform.position += RandomSpeed * (m_Rot * Vector2.right) * Time.deltaTime * fac;
        fac = Mathf.Clamp(fac - Time.deltaTime, 0.1f, 1f);
    }
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(3f);
        Remove();
    }
    public override void Remove()
    {
        base.Remove();
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
}

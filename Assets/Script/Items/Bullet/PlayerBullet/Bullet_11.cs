using System.Collections;
using UnityEngine;

public class Bullet_11 : IPlayerBullet
{
    private float fac;
    private float RandomSpeed;
    public Bullet_11(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Bullet_11;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        CoroutinePool.Instance.StartCoroutine(WaitForDestroy(), this);
        fac = 1.0f;
        RandomSpeed = Random.Range(0.5f, 1) * m_Attr.Speed;
    }
    protected override void BeforeHitObstacleUpdate()
    {
        gameObject.transform.position += RandomSpeed * (m_Rot * Vector2.right) * Time.deltaTime * fac;
        fac = Mathf.Clamp(fac - Time.deltaTime, 0.1f, 1f);
    }
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Remove();
    }
}

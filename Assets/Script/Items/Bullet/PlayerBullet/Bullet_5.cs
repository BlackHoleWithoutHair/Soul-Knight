using DG.Tweening;
using UnityEngine;

public class Bullet_5 : IPlayerBullet
{
    private bool isStart;
    private IEnemy enemy;
    private Vector2 targetDirection;
    public Bullet_5(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Bullet_5;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        isStart = false;
        enemy = GetClosestEnemy();
        gameObject.transform.DOMove(position + (Vector2)(Quaternion.Euler(0, 0, Random.Range(-60, 60)) * m_Rot * Vector2.right), 0.4f).OnComplete(() =>
        {
            isStart = true;
        });
    }
    protected override void BeforeHitObstacleUpdate()
    {
        if (isStart)
        {
            if (enemy == null)
            {
                base.BeforeHitObstacleUpdate();
            }
            else
            {
                if (enemy.m_Attr.CurrentHp > 0)
                {
                    targetDirection = enemy.gameObject.transform.position - gameObject.transform.position;
                }
                gameObject.transform.position += (Vector3)targetDirection.normalized * m_Attr.Speed * Time.deltaTime;
            }
        }


    }
    protected override void OnHitWall()
    {
        base.OnHitWall();
        CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.Magenta);
    }
    protected override void OnHitCharacter()
    {
        base.OnHitCharacter();
        CreateBoomEffect(EffectBoomType.EffectBoom_1, BulletColorType.Magenta);
    }
    private IEnemy GetClosestEnemy()
    {
        float min = 15;
        GameObject o = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector2.Distance(gameObject.transform.position, obj.transform.position) < min)
            {
                min = Vector2.Distance(gameObject.transform.position, obj.transform.position);
                o = obj;
            }
        }
        if (o == null)
        {
            return null;
        }
        return o.GetComponent<Symbol>().GetCharacter() as IEnemy;
    }
}

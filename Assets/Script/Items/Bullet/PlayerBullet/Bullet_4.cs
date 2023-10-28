using UnityEngine;

public class Bullet_4 : IPlayerBullet
{
    private Vector3 targetDirection;
    private float Angle;
    private float StartAngle;
    private float RandomAngle;
    private IEnemy enemy;
    public Bullet_4(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Bullet_4;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        enemy = GetClosestEnemy();
        RandomAngle = Random.Range(0, 30);
        gameObject.transform.Find("Trail").GetComponent<TrailRenderer>().Clear();
    }
    protected override void BeforeHitObstacleUpdate()
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
                Angle = Vector2.SignedAngle(gameObject.transform.rotation * Vector2.right, targetDirection);
                gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z + Angle);
                if (Angle > 0)
                {
                    if (Angle < 120)
                    {
                        gameObject.transform.rotation *= Quaternion.Euler(0, 0, -StartAngle / 2);
                    }
                    else
                    {
                        gameObject.transform.rotation *= Quaternion.Euler(0, 0, -StartAngle + RandomAngle);
                    }

                }
                else if (Angle < 0)
                {
                    if (Angle > -120)
                    {
                        gameObject.transform.rotation *= Quaternion.Euler(0, 0, -StartAngle / 2);
                    }
                    else
                    {
                        gameObject.transform.rotation *= Quaternion.Euler(0, 0, -StartAngle - RandomAngle);
                    }
                }
            }
            gameObject.transform.position += gameObject.transform.rotation * Vector3.right * m_Attr.Speed * Time.deltaTime;
        }
    }
    protected override void OnHitWall()
    {
        base.OnHitWall();
        CreateBoomEffect(EffectBoomType.EffectBoom_2, BulletColorType.Yellow);
    }
    protected override void OnHitCharacter()
    {
        base.OnHitCharacter();
        CreateBoomEffect(EffectBoomType.EffectBoom_2, BulletColorType.Yellow);
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
        targetDirection = o.transform.position - gameObject.transform.position;
        StartAngle = Vector2.SignedAngle(gameObject.transform.rotation * Vector2.right, targetDirection);
        return o.GetComponent<Symbol>().GetCharacter() as IEnemy;
    }
}

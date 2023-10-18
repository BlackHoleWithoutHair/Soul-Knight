using DG.Tweening;
using UnityEngine;

public class Bullet_6 : IPlayerBullet
{
    private bool isStart;
    private IEnemy enemy;
    private Vector2 targetDirection;
    public Bullet_6(GameObject obj, PlayerWeaponShareAttribute attr) : base(obj, attr)
    {
        type = PlayerBulletType.Bullet_6;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        isStart = false;
        enemy = GetClosestEnemy();
        gameObject.transform.DOMove(position + (Vector2)((Quaternion.Euler(0, 0, Random.Range(-60, 60)) * m_Rot) * Vector2.right), 0.4f).OnComplete(() =>
        {
            isStart = true;
        });
    }
    protected override void BeforeHitWallUpdate()
    {
        if (isStart)
        {
            if (enemy == null)
            {
                base.BeforeHitWallUpdate();
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
    protected override void AfterHitWallStart()
    {
        base.AfterHitWallStart();
        IEffectBoom boom = EffectFactory.Instance.GetEffectBoom(EffectBoomType.EffectBoom_1, gameObject.transform.position);
        boom.SetColor(new Color(0, 0.4f, 1));
        boom.AddToController();
    }
    protected override void AfterHitWallUpdate()
    {
        base.AfterHitWallUpdate();
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

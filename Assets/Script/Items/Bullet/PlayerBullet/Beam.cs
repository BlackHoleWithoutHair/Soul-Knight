using UnityEngine;

public class Beam : IPlayerBullet
{
    private IEnemy enemy;
    private Animator m_Animator;
    private AnimatorStateInfo info;
    private bool isFindEnemy;
    private PlayerController controller;
    public Beam(GameObject obj) : base(obj)
    {
        type = PlayerBulletType.Beam;
    }
    protected override void Init()
    {
        base.Init();
        m_Animator = gameObject.GetComponent<Animator>();
        controller = GameMediator.Instance.GetController<PlayerController>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        enemy = controller.GetCloseEnemy(controller.Player.gameObject);
        if (enemy != null)
        {
            isFindEnemy = true;
            if (enemy.gameObject.transform.Find("Foot"))
            {
                gameObject.transform.position = enemy.gameObject.transform.Find("Foot").position;
            }
            else
            {
                gameObject.transform.position = enemy.gameObject.transform.position;
            }
            enemy.UnderAttack(m_Attr.Damage);
        }
        else
        {
            isFindEnemy = false;
            Remove();
        }
    }
    protected override void BeforeHitObstacleUpdate()
    {
        base.BeforeHitObstacleUpdate();
        if (isFindEnemy)
        {
            info = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime > 1)
            {
                Remove();
            }
        }
    }
}
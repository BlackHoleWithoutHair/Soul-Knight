using UnityEngine;

public class IBullet : Item
{
    protected IBulletShareAttribute m_Attr;
    protected IWeapon m_Weapon;
    protected bool isHitWall;
    private Color color;
    public IBullet(GameObject obj) : base(obj)
    {
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, gameObject, "Obstacles", (obj) =>
        {
            OnHitWall();
        });
    }
    public override void OnEnter()
    {
        base.OnEnter();
        isHitWall = false;
        if (gameObject.GetComponent<BoxCollider2D>())
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        gameObject.transform.rotation = m_Rot;//lookRotation «z÷·∂‘∆Î
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!isHitWall)
        {
            BeforeHitObstacleUpdate();
        }
    }
    protected virtual void BeforeHitObstacleUpdate()
    {
        gameObject.transform.position += m_Rot * Vector2.right * m_Attr.Speed * Time.deltaTime;
    }
    protected virtual void OnHitWall() { }
    protected virtual void OnHitCharacter() { }
    public virtual void SetColor(BulletColorType color)
    {
        this.color = UnityTool.Instance.GetBulletColor(color);
        gameObject.GetComponent<SpriteRenderer>().color = this.color;
    }
    public void SetAttr(IBulletShareAttribute attr)
    {
        m_Attr = attr;
    }
    protected void SetIsHitWall()
    {
        isHitWall = false;
    }
    protected void CreateBoomEffect(EffectBoomType type,BulletColorType color)
    {
        IEffectBoom boom = ItemPool.Instance.GetEffectBoom(type, gameObject.transform.position);
        if(this.color!=Color.clear)
        {
            boom.SetColor(this.color);
        }
        else
        {
            boom.SetColor(UnityTool.Instance.GetBulletColor(color));
        }
        boom.AddToController();
    }
    public void SetWeapon(IWeapon weapon)
    {
        m_Weapon=weapon;
    }
}

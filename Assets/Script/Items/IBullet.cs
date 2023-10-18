using UnityEngine;
public class IBullet : Item
{
    public IWeaponShareAttribute m_Attr;
    protected bool isHitWall;
    protected bool isHitWallStart;
    public IBullet(GameObject obj, IWeaponShareAttribute attr) : base(obj)
    {
        m_Attr = attr;
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, gameObject, "Obstacles", OnHitWall);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        isHitWall = false;
        isHitWallStart = false;
        if (gameObject.GetComponent<BoxCollider2D>())
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        gameObject.transform.rotation = m_Rot;//lookRotation «z÷·∂‘∆Î
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        if (!isHitWall)
        {
            BeforeHitWallUpdate();
        }
        if (isHitWall)
        {
            if (!isHitWallStart)
            {
                AfterHitWallStart();
            }
            AfterHitWallUpdate();
        }
    }
    protected virtual void BeforeHitWallUpdate()
    {
        gameObject.transform.position += m_Rot * Vector2.right * m_Attr.Speed * Time.deltaTime;
    }
    protected virtual void AfterHitWallStart()
    {
        isHitWallStart = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Remove();
    }
    protected virtual void AfterHitWallUpdate() { }
    protected void OnHitWall(GameObject obj)
    {
        isHitWall = true;
    }
    public void SetAttr(IWeaponShareAttribute attr)
    {
        m_Attr = attr;
    }
    public virtual void SetColor(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}

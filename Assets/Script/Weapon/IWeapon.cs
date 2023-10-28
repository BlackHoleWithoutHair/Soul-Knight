using UnityEngine;

public class IWeapon
{
    public GameObject gameObject { get; protected set; }
    public Transform transform { get => gameObject.transform; }
    public ICharacter m_Character { get; protected set; }
    protected GameObject FirePoint;
    protected GameObject RotOrigin;
    public bool CanBeRotated;
    protected float Cumulate;
    private bool isInit;
    private bool isEnter;
    public IWeapon(GameObject obj, ICharacter character)
    {
        CanBeRotated = true;
        gameObject = obj;
        m_Character = character;
        FirePoint = UnityTool.Instance.GetGameObjectFromChildren(gameObject, "FirePoint");
        RotOrigin = UnityTool.Instance.GetGameObjectFromChildren(gameObject, "RotOrigin");
    }
    protected virtual void OnInit() { }
    protected virtual void OnEnter()//玩家切换至此武器时执行一次
    {
        if (!isInit)
        {
            isInit = true;
            OnInit();
        }
    }
    public virtual void OnUpdate()
    {
        if (!isEnter)
        {
            isEnter = true;
            OnEnter();
        }
    }
    protected virtual void OnFire() { }
    public virtual void OnExit()
    {
        isEnter = false;
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
    public void RotateWeapon(Vector2 dir)
    {
        if (CanBeRotated)
        {
            if (dir.x > 0.02f)
            {
                RotOrigin.transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));
            }
            else if (dir.x < 0.02f)
            {
                RotOrigin.transform.localRotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(Vector2.left, dir));
            }
        }
    }
    public Vector2 GetRotOriginPos()
    {
        if(RotOrigin==null)
        {
            return transform.position;
        }
        else
        {
            return RotOrigin.transform.position;
        }
        
    }

}

using UnityEngine;

public class IWeapon
{
    protected GameObject m_GameObject;
    public GameObject gameObject => m_GameObject;
    protected ICharacter m_Character;
    protected GameObject FirePoint;
    protected GameObject RotOrigin;
    public bool CanBeRotated;

    protected float Cumulate;
    private bool isInit;
    private bool isEnter;
    public IWeapon(GameObject obj, ICharacter character)
    {
        CanBeRotated = true;
        m_GameObject = obj;
        m_Character = character;
        FirePoint = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "FirePoint");
        RotOrigin = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "RotOrigin");
    }
    protected virtual void OnInit() { }
    protected virtual void OnEnter()
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
    }
    public virtual void OnCharacterDie() { }
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
        return RotOrigin.transform.position;
    }

}

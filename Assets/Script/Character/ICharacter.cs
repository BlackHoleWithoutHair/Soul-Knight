using UnityEngine;

public abstract class ICharacter
{
    public CharacterAttribute m_Attr;
    public GameObject gameObject { get; protected set; }
    public Transform transform { get => gameObject.transform; }
    protected Animator m_Animator;
    protected Rigidbody2D m_rb;
    public bool IsDie { get; protected set; }
    public bool isShouldRemove;
    public bool isAlreadyRemove { get; private set; }
    protected bool m_isLeft;
    public bool isLeft
    {
        get => m_isLeft;
        set
        {
            Vector3 m_Rot = Vector3.zero;
            if (!m_Attr.m_ShareAttr.IsIdleLeft)
            {
                if (value == true)
                {
                    m_Rot.Set(0, 180, 0);
                }
                else
                {
                    m_Rot.Set(0, 0, 0);
                }
            }
            else
            {
                if (value == true)
                {
                    m_Rot.Set(0, 0, 0);
                }
                else
                {
                    m_Rot.Set(0, 180, 0);
                }
            }
            gameObject.transform.rotation = Quaternion.Euler(m_Rot);
            m_isLeft = value;
        }
    }
    private bool isSuspend;
    private bool isStopRun;
    private bool isInit;
    private bool isStart;
    public ICharacter(GameObject obj)
    {
        gameObject = obj;
        EventCenter.Instance.RegisterObserver(EventType.OnPause, () =>
        {
            m_Attr.isPause = true;
        });
        EventCenter.Instance.RegisterObserver(EventType.OnResume, () =>
        {
            m_Attr.isPause = false;
        });

    }
    public virtual void GameUpdate()
    {
        if (!isInit)
        {
            isInit = true;
            OnInit();
        }
        if (IsDie == true)
        {
            OnCharaterDieUpdate();
            if (isShouldRemove && !isAlreadyRemove)
            {
                isAlreadyRemove = true;
            }
        }
        else
        {
            if (m_Attr.isDizzy || m_Attr.isFreeze || m_Attr.isPause)
            {
                if (!isSuspend)
                {
                    isSuspend = true;
                    OnCharacterSuspendEnter();
                }
                OnCharacterSuspendUpdate();
            }
            else
            {
                if (isSuspend)
                {
                    isSuspend = false;
                    OnCharacterSuspendExit();
                }
                if (!m_Attr.isRun)
                {
                    if (!isStopRun)
                    {
                        isStopRun = true;
                        OnCharacterStopRunEnter();
                    }
                    OnCharacterStopUpdate();
                }
                else
                {
                    if (isStopRun)
                    {
                        isStopRun = false;
                        OnCharacterStopExit();
                    }
                    OnCharacterUpdate();
                }
            }
        }
        AlwaysUpdate();
    }
    protected virtual void AlwaysUpdate() { }
    protected virtual void OnInit() { }

    protected virtual void OnCharacterStart()
    {
        if (m_Attr == null)
        {
            Debug.Log("ICharacter m_Attr is null");
            return;
        }
    }
    protected virtual void OnCharacterUpdate()
    {
        if (!isStart)
        {
            isStart = true;
            OnCharacterStart();
        }
        if (m_Attr.CurrentHp <= 0 && !IsDie)
        {
            IsDie = true;
            OnCharacterDieStart();
        }
    }
    protected virtual void OnCharacterSuspendEnter()
    {
        m_Animator.speed = 0;
    }
    protected virtual void OnCharacterSuspendUpdate()
    {
        if (m_rb != null)
        {
            m_rb.velocity = Vector2.zero;
        }
    }
    protected virtual void OnCharacterSuspendExit()
    {
        m_Animator.speed = 1;
    }
    protected virtual void OnCharacterStopRunEnter()
    {
        m_Animator.SetBool("isIdle", true);
    }
    protected virtual void OnCharacterStopUpdate() { }
    protected virtual void OnCharacterStopExit() { }
    protected virtual void OnCharacterDieStart() { }
    protected virtual void OnCharaterDieUpdate() { }
    protected void Remove()
    {
        isShouldRemove = true;
    }
    public virtual void UnderAttack(int damage)
    {
        m_Attr.CurrentHp -= damage;
    }
    public virtual void UnderTreating(int num)
    {
        PlayerPopupNum pop = EffectFactory.Instance.GetEffect(EffectType.PlayerPopupNum, gameObject.transform.position) as PlayerPopupNum;
        pop.SetColor(UnityTool.Instance.GetBulletColor(BulletColorType.Red));
        pop.SetText("+" + num);
        pop.AddToController();
        if (m_Attr.CurrentHp < m_Attr.m_ShareAttr.MaxHp)
        {
            m_Attr.CurrentHp += num;
        }
    }
    public virtual void Resurrection()
    {
        IsDie = false;
    }
}

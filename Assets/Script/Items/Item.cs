using UnityEngine;
public abstract class Item
{
    protected Vector2 position;
    protected Quaternion m_Rot;
    protected ItemPool pool;
    public GameObject gameObject { get; protected set; }
    public Transform transform { get => gameObject.transform; }
    public bool isAlreadyRemove { get; protected set; }
    private bool isInit;
    private bool isShouldRemove;
    protected bool isDestroyOnRemove;
    public Item(GameObject obj)
    {
        gameObject = obj;
        isDestroyOnRemove = true;
    }
    public virtual void GameUpdate()
    {
        OnUpdate();
        if (isShouldRemove && !isAlreadyRemove)
        {
            isAlreadyRemove = true;
            OnExit();
        }
    }
    protected virtual void Init() { }
    public virtual void OnEnter()
    {
        if (!isInit)
        {
            isInit = true;
            Init();
        }
        isAlreadyRemove = false;
        isShouldRemove = false;
        gameObject.transform.position = position;
    }
    protected virtual void OnUpdate() { }
    protected virtual void OnExit()
    {
        if (isDestroyOnRemove)
        {
            TriggerCenter.Instance.RemoveObserver(TriggerType.OnTriggerEnter, gameObject);
            TriggerCenter.Instance.RemoveObserver(TriggerType.OnTriggerExit, gameObject);
            Object.Destroy(gameObject);
        }
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
    public virtual void SetPosition(Vector2 position)
    {
        this.position = position;
        transform.position = position;
    }
    public virtual void SetRotation(Quaternion rot)
    {
        m_Rot = rot;
        transform.rotation = rot;
    }
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
    public void Remove()
    {
        isShouldRemove = true;
    }
    public void SetPool(ItemPool pool)
    {
        this.pool = pool;
        DontDestroyOnRemove();
    }
    public void DontDestroyOnRemove()
    {
        isDestroyOnRemove = false;
    }
    public void AddToController()
    {
        GameMediator.Instance.GetController<ItemController>().AddItem(this);
    }
}

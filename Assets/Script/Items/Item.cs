using UnityEngine;
public abstract class Item
{
    protected Vector2 position;
    protected Quaternion m_Rot;
    protected ItemPool pool;
    public GameObject gameObject { get; protected set; }
    public Transform transform { get => gameObject.transform; }
    public bool ShouldBeRemove { get; protected set; }
    private bool isInit;
    protected bool isDestroyOnRemove;
    public Item(GameObject obj)
    {
        gameObject = obj;
        isDestroyOnRemove = true;
    }
    protected virtual void Init() { }
    public virtual void OnEnter()
    {
        if (!isInit)
        {
            isInit = true;
            Init();
        }
        ShouldBeRemove = false;
        gameObject.transform.position = position;
    }
    public virtual void GameUpdate() { }
    public virtual void OnExit() { }
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
    public virtual void Remove()
    {
        if (isDestroyOnRemove)
        {
            TriggerCenter.Instance.RemoveObserver(TriggerType.OnTriggerEnter, gameObject);
            TriggerCenter.Instance.RemoveObserver(TriggerType.OnTriggerExit, gameObject);
            Object.Destroy(gameObject);
        }
        ShouldBeRemove = true;
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

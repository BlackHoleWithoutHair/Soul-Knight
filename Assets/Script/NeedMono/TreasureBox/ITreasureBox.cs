using UnityEngine;

public abstract class ITreasureBox : MonoBehaviour
{
    protected Animator m_Animator;
    protected IPlayer player;
    protected GameObject Name;
    protected bool isPlayerEnter;
    private AnimatorStateInfo info;
    protected bool isFinish;
    protected virtual void Start()
    {
        Name = transform.Find("Name")?.gameObject;
        m_Animator = GetComponent<Animator>();
    }
    protected virtual void Update()
    {
        if (!isFinish)
        {
            if (isPlayerEnter)
            {
                Name.SetActive(true);
                if (InputUtility.Instance.GetKeyDown(KeyAction.Use))
                {
                    m_Animator.enabled = true;
                }
            }
            else
            {
                Name.SetActive(false);
            }
            info = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime > 1)
            {
                OnFinishOpen();
                isFinish = true;
                Name.SetActive(false);
            }
        }
    }
    protected virtual void OnFinishOpen() { }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerEnter = true;
            player = collision.transform.GetComponent<Symbol>().GetCharacter() as IPlayer;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerEnter = false;
        }
    }
}

using UnityEngine;

public class Fridge : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isEnter;
    private bool isReceiveInput;
    private Animator m_Animator;
    private GameObject ItemName;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        ItemName = transform.Find("ItemName").gameObject;
        isReceiveInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnter)
        {
            ItemName.SetActive(true);
            if (InputUtility.Instance.GetKeyDown(KeyAction.Use) && isReceiveInput)
            {
                isReceiveInput = false;
                m_Animator.SetBool("isOpen", !m_Animator.GetBool("isOpen"));

                EventCenter.Instance.NotisfyObserver(EventType.OnWantUseFridge);
            }
        }
        else
        {
            ItemName.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEnter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEnter = false;
            isReceiveInput = true;
        }
    }
}

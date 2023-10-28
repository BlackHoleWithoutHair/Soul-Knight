using UnityEngine;

public class SafeBox : MonoBehaviour
{
    private bool isPlayerEnter;
    private bool isReceiveInput;
    private GameObject m_Name;
    private Collider2D m_Collision;
    private void Start()
    {
        isReceiveInput = true;
        m_Name = transform.Find("Name").gameObject;
    }
    private void Update()
    {
        if (isPlayerEnter)
        {
            if (InputUtility.Instance.GetKeyDown(KeyAction.Use) && isReceiveInput)
            {
                isReceiveInput = false;
                EventCenter.Instance.NotisfyObserver(EventType.OnWantUseSafeBox);
            }
            m_Name.SetActive(true);
        }
        else
        {
            m_Name.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerEnter = true;
            m_Collision = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerEnter = false;
            isReceiveInput = true;
        }
    }
}

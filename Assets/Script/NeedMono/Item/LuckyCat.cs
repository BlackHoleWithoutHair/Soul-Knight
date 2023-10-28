

using UnityEngine;

public class LuckyCat : MonoBehaviour
{
    private bool isPlayerEnter;
    private bool isReceiveInput;
    private GameObject ItemName;
    private void Start()
    {
        ItemName = transform.Find("ItemName").gameObject;
        isReceiveInput = true;
    }
    private void Update()
    {
        if (isPlayerEnter)
        {
            ItemName.SetActive(true);
            if (isReceiveInput && InputUtility.Instance.GetKeyDown(KeyAction.Use))
            {
                isReceiveInput = false;
                EventCenter.Instance.NotisfyObserver(EventType.OnUseLuckyCat);
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
            isPlayerEnter = true;
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

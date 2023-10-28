using UnityEngine;

public class SmithingTable : MonoBehaviour
{
    private bool isEnter;
    private bool isReceiveInput;
    private Collider2D c;
    private GameObject Name;

    private void Start()
    {
        Name = transform.Find("Name").gameObject;
        isReceiveInput = true;
    }
    private void Update()
    {
        if (isEnter)
        {
            if (!isContainWeapon())
            {
                Name.SetActive(true);
                if (InputUtility.Instance.GetKeyDown(KeyAction.Use) && isReceiveInput)
                {
                    isReceiveInput = false;
                    isEnter = false;
                    Name.SetActive(false);
                    EventCenter.Instance.NotisfyObserver(EventType.OnWantForging);
                    TriggerCenter.Instance.NotisfyObserver(TriggerType.OnTriggerEnter, gameObject, c.gameObject);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEnter = true;
            c = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEnter = false;
            c = collision;
            Name.SetActive(false);
            isReceiveInput = true;
        }
    }
    private bool isContainWeapon()
    {
        foreach (Transform weapon in transform.GetComponentsInChildren<Transform>())
        {
            if (weapon.CompareTag("Weapon"))
            {
                return true;
            }
        }
        return false;
    }

}

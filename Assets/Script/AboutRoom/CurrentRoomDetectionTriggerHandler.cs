using UnityEngine;

public class CurrentRoomDetectionTriggerHandler : MonoBehaviour
{
    //private CurrentRoomDetectionRoomManager roomManager;

    public void Start()
    {
        //roomManager = transform.parent.parent.gameObject.GetComponent<CurrentRoomDetectionRoomManager>();
    }

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            Debug.Log(2);
            TriggerCenter.Instance.NotisfyObserver(TriggerType.OnTriggerEnter, otherCollider.gameObject, gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            //roomManager?.OnRoomLeave(otherCollider.gameObject);
        }
    }

}

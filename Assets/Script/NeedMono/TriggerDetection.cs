using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(gameObject + " " + collision.name+" "+collision.transform.parent.name);
        TriggerCenter.Instance.NotisfyObserver(TriggerType.OnTriggerEnter, gameObject, collision.gameObject);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        TriggerCenter.Instance.NotisfyObserver(TriggerType.OnTriggerExit, gameObject, collision.gameObject);
    }
}

using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject Text;
    private bool isPlayerEnter;
    private void Start()
    {
        Text = transform.Find("Text").gameObject;
    }
    private void Update()
    {
        if (isPlayerEnter)
        {
            Text.SetActive(true);
            if (InputUtility.Instance.GetKeyDown(KeyAction.Use))
            {
                ModelContainer.Instance.GetModel<MemoryModel>().Stage += 1;
                SceneModelCommand.Instance.ReloadActiveScene();
            }
        }
        else
        {
            Text.SetActive(false);
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
        }
    }

}

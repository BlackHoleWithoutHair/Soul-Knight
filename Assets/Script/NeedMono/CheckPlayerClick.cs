using System.Collections;
using TMPro;
using UnityEngine;

public class CheckPlayerClick : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerType PlayerType;
    private int Times;
    private bool isEnter;
    private bool isClose;
    private DialogueData dialogue;
    private TextMeshProUGUI NameText;
    private TextMeshProUGUI DialogueText;
    public void Start()
    {
        isClose = true;
        dialogue = AttributeFactory.Instance.GetDialogue(PlayerType);
        DialogueText = transform.parent.Find("Dialogue").Find("Text").GetComponent<TextMeshProUGUI>();
        NameText = transform.parent.Find("PlayerName").Find("Text").GetComponent<TextMeshProUGUI>();
        NameText.text = AttributeFactory.Instance.GetPlayerAttr(PlayerType).m_ShareAttr.PlayerName;
    }
    public void Update()
    {
        if (isEnter)
        {
            if (!DialogueText.transform.parent.gameObject.activeSelf)
            {
                NameText.transform.parent.gameObject.SetActive(true);
            }
            if (InputUtility.Instance.GetKeyDown(KeyAction.Use) && isClose)
            {
                NameText.transform.parent.gameObject.SetActive(false);
                DialogueText.transform.parent.gameObject.SetActive(true);
                isClose = false;
                DialogueText.text = dialogue.Dialogues[(Times++) % (dialogue.Dialogues.Count)];
                if (DialogueText.GetTextInfo(DialogueText.text).lineCount == 1)
                {
                    DialogueText.horizontalAlignment = HorizontalAlignmentOptions.Center;
                }
                else
                {
                    DialogueText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                }

                StartCoroutine(CloseDialogue());
            }
        }
        else
        {
            NameText.transform.parent.gameObject.SetActive(false);
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
        }
    }
    private IEnumerator CloseDialogue()
    {
        yield return new WaitForSeconds(2);
        DialogueText.transform.parent.gameObject.SetActive(false);
        isClose = true;
    }
}

using TMPro;
using UnityEngine;

public class CollectWeapon : MonoBehaviour
{
    private IPlayer player;
    private Collider2D collision;
    private GameObject NameObj;
    private TextMeshProUGUI text;
    private bool isEnter;
    private void Start()
    {
        if (transform.Find("WeaponName") != null)
        {
            NameObj = transform.Find("WeaponName").gameObject;
            text = NameObj.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            text.text = LanguageCommand.Instance.GetTranslation(name);
        }
    }
    private void Update()
    {
        if (isEnter)
        {
            if (InputUtility.Instance.GetKeyDown(KeyAction.Use))
            {
                player = collision.GetComponent<Symbol>().GetCharacter() as IPlayer;
                player.AddWeapon(System.Enum.Parse<PlayerWeaponType>(name));
                Destroy(gameObject);
            }
            NameObj?.SetActive(true);
        }
        else
        {
            NameObj?.SetActive(false);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isEnter = true;
            this.collision = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isEnter = false;
        }
    }
    public void SetWeaponBoxColliderInfo(Vector2 size, Vector2 offect)
    {
        GetComponent<BoxCollider2D>().size = size;
        GetComponent<BoxCollider2D>().offset = offect;
    }

}

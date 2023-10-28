using TMPro;
using UnityEngine;

public class WorldMaterial : Item
{
    protected MaterialType materialType;
    private GameObject playerObj;
    private TextMeshProUGUI TextName;
    private GameObject MaterialName;
    public WorldMaterial(MaterialType type, GameObject obj) : base(obj)
    {
        materialType = type;
    }
    protected override void Init()
    {
        base.Init();
        TextName = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(gameObject, "Text");
        MaterialName = gameObject.transform.Find("MaterialName").gameObject;
        TextName.text = LanguageCommand.Instance.GetTranslation(materialType.ToString());
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, gameObject, "Player", (obj) =>
        {
            MaterialName.SetActive(true);
            playerObj = obj;
        });
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerExit, gameObject, "Player", (obj) =>
        {
            MaterialName.SetActive(false);
            playerObj = null;
        });
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (playerObj != null && InputUtility.Instance.GetKeyDown(KeyAction.Use))
        {
            ArchiveCommand.Instance.AddMaterial(materialType);
            Remove();
        }
    }

}

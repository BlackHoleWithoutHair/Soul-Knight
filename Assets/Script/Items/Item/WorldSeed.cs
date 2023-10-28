using TMPro;
using UnityEngine;
public class WorldSeed : Item
{
    protected SeedType seedType;
    private GameObject playerObj;
    private TextMeshProUGUI TextName;
    private GameObject seedName;
    public WorldSeed(SeedType type, GameObject obj) : base(obj)
    {
        seedType = type;
    }
    protected override void Init()
    {
        base.Init();
        TextName = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(gameObject, "Text");
        seedName = gameObject.transform.Find("SeedName").gameObject;
        TextName.text = LanguageCommand.Instance.GetTranslation(seedType.ToString());
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, gameObject, "Player", (obj) =>
        {
            seedName.SetActive(true);
            playerObj = obj;
        });
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerExit, gameObject, "Player", (obj) =>
        {
            seedName.SetActive(false);
            playerObj = null;
        });
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (playerObj != null && InputUtility.Instance.GetKeyDown(KeyAction.Use))
        {
            ArchiveCommand.Instance.AddSeed(seedType, 1);
            Remove();
        }
    }
}

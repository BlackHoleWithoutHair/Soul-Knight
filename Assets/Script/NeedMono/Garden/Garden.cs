using TMPro;
using UnityEngine;

public class Garden : MonoBehaviour
{
    private bool isEnter;
    private bool isReceiveInput;
    private int index;
    private GameObject m_TextName;

    private GameObject m_WetSoil;
    private GameObject m_BreakSoil;

    private IPlant m_Plant;
    public IPlant Plant => m_Plant;
    private GardenInfo m_GardenInfo;
    public GardenInfo GardenInfo => m_GardenInfo;
    private GardenUI m_GardenUI;
    private void Start()
    {
        isReceiveInput = true;
        index = int.Parse(name.Replace("Garden", ""));
        m_GardenInfo = ArchiveQuery.Instance.GetGardenInfo(index - 1);
        m_TextName = transform.Find("GardenName").gameObject;
        m_WetSoil = transform.Find("WetSoil").gameObject;
        m_BreakSoil = transform.Find("BreakSoil").gameObject;

        if (m_GardenInfo.isLocked)
        {
            m_TextName.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "未解锁";
        }
        if (m_GardenInfo.plantInfo.WetTime.GetTotleSeconds() > 0)
        {
            m_WetSoil.SetActive(true);
        }
        if (ArchiveQuery.Instance.isHavingPlant(GardenInfo.index))
        {
            m_Plant = ItemFactory.Instance.GetPlant(gameObject, this);
            m_GardenUI = new GardenUI(this, transform.Find("PlantCanvas").gameObject);
        }
    }
    private void Update()
    {
        if (isEnter)
        {
            if (m_Plant == null)
            {
                m_TextName.SetActive(true);
            }
            else
            {
                m_GardenUI.gameObject.SetActive(true);
            }
            if (InputUtility.Instance.GetKeyDown(KeyAction.Use) && isReceiveInput)
            {
                isReceiveInput = false;
                if (!m_GardenInfo.isLocked && m_Plant == null)
                {
                    EventCenter.Instance.NotisfyObserver(EventType.OnWantPlant, this);
                }
                if (m_GardenInfo.isLocked)
                {
                    EventCenter.Instance.NotisfyObserver(EventType.OnWantUnlockGarden, this);
                }
                if (m_Plant != null && m_Plant.PlantState == PlantState.Ripening)
                {
                    m_Plant.SaveHarvest();
                    RemovePlant();
                }
            }
        }
        else
        {
            m_TextName.SetActive(false);
            m_GardenUI?.gameObject.SetActive(false);
        }

        if (m_GardenInfo.plantInfo.WetTime.GetTotleSeconds() > 0)
        {
            m_WetSoil.SetActive(true);
        }
        else
        {
            m_WetSoil.SetActive(false);
        }
        m_GardenUI?.GameUpdate();
        m_Plant?.GameUpdate();
    }
    public void PlantSeed(SeedType type)
    {
        ArchiveCommand.Instance.Plant(type, GardenInfo.index);
        m_Plant = ItemFactory.Instance.GetPlant(gameObject, this);
        m_GardenUI = new GardenUI(this, transform.Find("PlantCanvas").gameObject);
    }
    public void RemovePlant()
    {
        m_Plant.OnExit();
        m_Plant = null;
        m_GardenUI.OnExit();
        m_GardenUI = null;
        ArchiveCommand.Instance.RemovePlant(GardenInfo.index);
    }
    public void Watering()
    {
        m_Plant.GetPlantInfo().WetTime.Init(new System.TimeSpan(24, 0, 0));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isEnter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isEnter = false;
            isReceiveInput = true;
        }
    }
}

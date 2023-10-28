using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GardenUI
{
    private GameObject m_GameObject;
    public GameObject gameObject => m_GameObject;
    private Garden garden;
    private TextMeshProUGUI m_TextName;
    private TextMeshProUGUI m_TextGrowTime;
    private TextMeshProUGUI m_TextNextTime;
    private TextMeshProUGUI m_TextState;
    private TimeSpan GrowTime;
    private bool isStart;
    public GardenUI(Garden garden, GameObject obj)
    {
        this.garden = garden;
        m_GameObject = obj;
    }
    private void Start()
    {
        m_TextName = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextName");
        m_TextGrowTime = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextGrowTime");
        m_TextNextTime = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextNextTime");
        m_TextState = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextState");
        GrowTime = garden.Plant.GetPlantInfo().GrowTime;
        CoroutinePool.Instance.StartCoroutine(UpdateLoop(), this);
    }
    public void GameUpdate()
    {
        if (!isStart)
        {
            isStart = true;
            Start();
        }
    }
    private IEnumerator UpdateLoop()
    {
        while (true)
        {
            m_TextName.text = "植物名称:" + LanguageCommand.Instance.GetTranslation(garden.Plant.GetPlantInfo().SeedType.ToString());
            m_TextGrowTime.text = String.Format("生长时间:{0}天{1}时{2}分{3}秒", GrowTime.days, GrowTime.hours, GrowTime.minutes, GrowTime.seconds);
            m_TextNextTime.text = String.Format("下一阶段时间:{0}天", PlantCommand.Instance.GetPlantNextStateTime(garden.Plant.GetPlantInfo().SeedType, GrowTime).ToString());
            m_TextState.text = String.Format("当前状态:{0}", LanguageCommand.Instance.GetTranslation(garden.Plant.PlantState.ToString()));
            yield return new WaitForSeconds(1);
        }
    }
    public void OnExit()
    {
        gameObject.SetActive(false);
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
}

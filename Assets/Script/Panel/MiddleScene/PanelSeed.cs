using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleScene
{
    public class PanelSeed : IPanel
    {
        private Garden garden;
        private CanvasGroup canvasGroup;
        private GameObject DivVerSeed;
        private Button ButtonClose;
        public PanelSeed(IPanel parent) : base(parent)
        {
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
            isShowPanelAfterExit = true;
        }
        protected override void OnInit()
        {
            base.OnInit();
            canvasGroup = m_GameObject.GetComponent<CanvasGroup>();
            DivVerSeed = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivVerSeed");
            ButtonClose = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonClose");
            canvasGroup.alpha = 0;
            DivVerSeed.SetActive(false);
            ButtonClose.onClick.AddListener(() =>
            {
                OnExit();
            });
            EventCenter.Instance.RegisterObserver<Garden>(EventType.OnWantPlant, (garden) =>
            {
                this.garden = garden;
            });

        }
        protected override void OnEnter()
        {
            base.OnEnter();
            m_GameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.3f).SetUpdate(true);
            int i = 0;
            for (i = 0; i < ModelContainer.Instance.GetModel<ArchiveModel>().GameData.seedDatas.Count; i++)
            {
                DivVerSeed.SetActive(true);
                if (i <DivVerSeed.transform.parent.childCount)
                {
                    Transform child = DivVerSeed.transform.parent.GetChild(i);
                    SetSeedInfo(child.gameObject, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.seedDatas[i]);
                }
                else
                {
                    GameObject obj = Object.Instantiate(DivVerSeed, DivVerSeed.transform.parent);
                    SetSeedInfo(obj, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.seedDatas[i]);
                }
            }
            UnityTool.Instance.ClearResidualChild(DivVerSeed.transform.parent, i);
            Time.timeScale = 0;
            EventCenter.Instance.NotisfyObserver(EventType.OnPause);
        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            canvasGroup.DOFade(0, 0.3f).OnComplete(() =>
            {
                m_GameObject.SetActive(false);
            });
            EventCenter.Instance.NotisfyObserver(EventType.OnResume);
        }
        private void SetSeedInfo(GameObject obj, SeedData info)
        {
            obj.transform.Find("TextName").GetComponent<TextMeshProUGUI>().text = LanguageCommand.Instance.GetTranslation(info.SeedType.ToString());
            UnityTool.Instance.GetComponentFromChild<Image>(obj, "ImageSeed").sprite = ProxyResourceFactory.Instance.Factory.GetPlantSprite(info.SeedType.ToString());
            obj.transform.Find("TextNum").GetComponent<TextMeshProUGUI>().text = ArchiveQuery.Instance.GetSeedNum(info.SeedType).ToString();
            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (garden != null)
                {
                    garden.PlantSeed(info.SeedType);
                }
                OnExit();
            });
            obj.SetActive(true);
        }
    }
}

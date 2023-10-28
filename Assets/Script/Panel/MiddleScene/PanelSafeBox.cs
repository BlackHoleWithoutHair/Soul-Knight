using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelSafeBox : IPanel
{
    private Button ButtonClose;
    private Toggle ToggleMaterial;
    private Toggle ToggleSeed;
    private Toggle ToggleWeaponCoupons;
    private GameObject ScrollViewSeed;
    private GameObject ScrollViewOther;
    private GameObject DivVer;
    private GameObject DivVerSeed;
    private CanvasGroup canvasGroup;
    public PanelSafeBox(IPanel parent) : base(parent)
    {
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        isShowPanelAfterExit = true;
    }
    protected override void OnInit()
    {
        base.OnInit();
        canvasGroup = m_GameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        ScrollViewOther = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "ScrollViewOther");
        ScrollViewSeed = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "ScrollViewSeed");
        DivVer = UnityTool.Instance.GetGameObjectFromChildren(ScrollViewOther, "DivVer");
        DivVerSeed = UnityTool.Instance.GetGameObjectFromChildren(ScrollViewSeed, "DivVerSeed");
        ButtonClose = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonClose");
        ToggleMaterial = UnityTool.Instance.GetComponentFromChild<Toggle>(m_GameObject, "ToggleMaterial");
        ToggleSeed = UnityTool.Instance.GetComponentFromChild<Toggle>(m_GameObject, "ToggleSeed");
        ToggleWeaponCoupons = UnityTool.Instance.GetComponentFromChild<Toggle>(m_GameObject, "ToggleWeaponCoupons");
        DivVer.SetActive(false);
        DivVerSeed.SetActive(false);
        ButtonClose.onClick.AddListener(() =>
        {
            OnExit();
        });
        ToggleMaterial.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                ShowScrollViewSeed(false);
                int i = 0;
                for (i = 0; i < ModelContainer.Instance.GetModel<ArchiveModel>().GameData.materialDatas.Count; i++)
                {
                    if (i <DivVer.transform.parent.childCount)
                    {
                        Transform child = DivVer.transform.parent.GetChild(i);
                        SetMaterialInfo(child.gameObject, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.materialDatas[i]);
                    }
                    else
                    {
                        GameObject obj = Object.Instantiate(DivVer, DivVer.transform.parent);
                        SetMaterialInfo(obj, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.materialDatas[i]);
                    }
                }
                UnityTool.Instance.ClearResidualChild(DivVer.transform.parent, i);
            }
        });
        ToggleSeed.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                ShowScrollViewSeed(true);
                int i = 0;
                for (i = 0; i < ModelContainer.Instance.GetModel<ArchiveModel>().GameData.seedDatas.Count; i++)
                {
                    DivVerSeed.SetActive(true);
                    if (i < DivVerSeed.transform.parent.childCount)
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
            }
        });
        ToggleWeaponCoupons.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                ShowScrollViewSeed(false);
                int i = 0;
                for (i = 0; i < ModelContainer.Instance.GetModel<ArchiveModel>().GameData.couponDatas.Count; i++)
                {
                    DivVer.SetActive(true);
                    if (i < DivVer.transform.parent.childCount)
                    {
                        Transform child = DivVer.transform.parent.GetChild(i);
                        SetCouponsInfo(child.gameObject, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.couponDatas[i]);
                    }
                    else
                    {
                        GameObject obj = Object.Instantiate(DivVer, DivVer.transform.parent);
                        SetCouponsInfo(obj, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.couponDatas[i]);
                    }
                }
                UnityTool.Instance.ClearResidualChild(DivVerSeed.transform.parent, i);
            }
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        ToggleMaterial.isOn = false;
        ToggleMaterial.isOn = true;
        Time.timeScale = 0;
        EventCenter.Instance.NotisfyObserver(EventType.OnPause);
        canvasGroup.DOFade(1, 0.3f).SetUpdate(true);
    }
    public override void OnExit()
    {
        base.OnExit();
        Time.timeScale = 1;
        canvasGroup.DOFade(0, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        EventCenter.Instance.NotisfyObserver(EventType.OnResume);
    }
    private void ShowScrollViewSeed(bool val)
    {
        if(val)
        {
            ScrollViewSeed.SetActive(true);
            ScrollViewOther.SetActive(false);
        }
        else
        {
            ScrollViewSeed.SetActive(false);
            ScrollViewOther.SetActive(true);
        }
    }
    private void SetMaterialInfo(GameObject obj, MaterialInfo info)
    {
        obj.transform.Find("TextName").GetComponent<TextMeshProUGUI>().text = LanguageCommand.Instance.GetTranslation(info.materialType.ToString());
        obj.transform.Find("ImageMaterial").GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetMaterialSprite(info.materialType.ToString());
        obj.transform.Find("TextNum").GetComponent<TextMeshProUGUI>().text = ArchiveQuery.Instance.GetMaterialNum(info.materialType).ToString();
        obj.SetActive(true);
    }
    private void SetCouponsInfo(GameObject obj, CouponData info)
    {
        obj.transform.Find("TextName").GetComponent<TextMeshProUGUI>().text = LanguageCommand.Instance.GetTranslation(info.CouponType.ToString())
          + "(" + LanguageCommand.Instance.GetTranslation(info.CouponQuality.ToString()) + ")";
        obj.transform.Find("ImageMaterial").GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetMaterialSprite(info.CouponType.ToString());
        obj.transform.Find("TextNum").GetComponent<TextMeshProUGUI>().text = ArchiveQuery.Instance.GetCouponsNum(info.CouponType, info.CouponQuality).ToString();
        UnityTool.Instance.SetTextColor(obj.transform.Find("TextName").GetComponent<TextMeshProUGUI>(), info.CouponQuality);
        obj.SetActive(true);
    }
    private void SetSeedInfo(GameObject obj, SeedData info)
    {
        obj.transform.Find("TextName").GetComponent<TextMeshProUGUI>().text = LanguageCommand.Instance.GetTranslation(info.SeedType.ToString());
        UnityTool.Instance.GetComponentFromChild<Image>(obj, "ImageSeed").sprite = ProxyResourceFactory.Instance.Factory.GetPlantSprite(info.SeedType.ToString());
        obj.transform.Find("TextNum").GetComponent<TextMeshProUGUI>().text = ArchiveQuery.Instance.GetSeedNum(info.SeedType).ToString();
        obj.SetActive(true);
    }
}
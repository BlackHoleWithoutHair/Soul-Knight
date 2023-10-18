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
        DivVer = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivVer");
        DivVerSeed = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivVerSeed");
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
                UnityTool.Instance.DestroyAllActiveObjExcept(DivVer.transform);
                for (int i = 0; i < ModelContainer.Instance.GetModel<ArchiveModel>().GameData.materialDatas.Count; i++)
                {
                    DivVer.SetActive(true);
                    if (i == 0)
                    {
                        SetMaterialInfo(DivVer, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.materialDatas[i]);
                    }
                    else
                    {
                        GameObject obj = Object.Instantiate(DivVer, DivVer.transform.parent);
                        SetMaterialInfo(obj, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.materialDatas[i]);
                    }
                }
            }
            else
            {
                DivVer.SetActive(false);
            }
        });
        ToggleSeed.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                UnityTool.Instance.DestroyAllActiveObjExcept(DivVerSeed.transform);
                for (int i = 0; i < ModelContainer.Instance.GetModel<ArchiveModel>().GameData.seedDatas.Count; i++)
                {
                    DivVerSeed.SetActive(true);
                    if (i == 0)
                    {
                        SetSeedInfo(DivVerSeed, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.seedDatas[i]);
                    }
                    else
                    {
                        GameObject obj = Object.Instantiate(DivVerSeed, DivVerSeed.transform.parent);
                        SetSeedInfo(obj, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.seedDatas[i]);
                    }
                }
            }
            else
            {
                DivVerSeed.gameObject.SetActive(false);
            }
        });
        ToggleWeaponCoupons.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                UnityTool.Instance.DestroyAllActiveObjExcept(DivVer.transform);
                for (int i = 0; i < ModelContainer.Instance.GetModel<ArchiveModel>().GameData.couponDatas.Count; i++)
                {
                    DivVer.SetActive(true);
                    if (i == 0)
                    {
                        SetCouponsInfo(DivVer, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.couponDatas[i]);
                    }
                    else
                    {
                        GameObject obj = UnityEngine.Object.Instantiate(DivVer, DivVer.transform.parent);
                        SetCouponsInfo(obj, ModelContainer.Instance.GetModel<ArchiveModel>().GameData.couponDatas[i]);
                    }
                }
            }
            else
            {
                DivVer.SetActive(false);
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
        canvasGroup.DOFade(0, 0.3f);
        EventCenter.Instance.NotisfyObserver(EventType.OnResume);
    }
    private void SetMaterialInfo(GameObject obj, MaterialInfo info)
    {
        obj.transform.Find("TextName").GetComponent<TextMeshProUGUI>().text = LanguageCommand.Instance.GetTranslation(info.materialType.ToString());
        obj.transform.Find("ImageMaterial").GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetMaterialSprite(info.materialType.ToString());
        obj.transform.Find("TextNum").GetComponent<TextMeshProUGUI>().text = ArchiveQuery.Instance.GetMaterialNum(info.materialType).ToString();
    }
    private void SetCouponsInfo(GameObject obj, CouponData info)
    {
        obj.transform.Find("TextName").GetComponent<TextMeshProUGUI>().text = LanguageCommand.Instance.GetTranslation(info.CouponType.ToString())
          + "(" + LanguageCommand.Instance.GetTranslation(info.CouponQuality.ToString()) + ")";
        obj.transform.Find("ImageMaterial").GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetMaterialSprite(info.CouponType.ToString());
        obj.transform.Find("TextNum").GetComponent<TextMeshProUGUI>().text = ArchiveQuery.Instance.GetCouponsNum(info.CouponType, info.CouponQuality).ToString();
        UnityTool.Instance.SetTextColor(obj.transform.Find("TextName").GetComponent<TextMeshProUGUI>(), info.CouponQuality);
    }
    private void SetSeedInfo(GameObject obj, SeedData info)
    {
        obj.transform.Find("TextName").GetComponent<TextMeshProUGUI>().text = LanguageCommand.Instance.GetTranslation(info.SeedType.ToString());
        UnityTool.Instance.GetComponentFromChild<Image>(obj, "ImageSeed").sprite = ProxyResourceFactory.Instance.Factory.GetPlantSprite(info.SeedType.ToString());
        obj.transform.Find("TextNum").GetComponent<TextMeshProUGUI>().text = ArchiveQuery.Instance.GetSeedNum(info.SeedType).ToString();
    }
}
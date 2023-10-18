using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelSmithingTable : IPanel
{
    private Button ButtonClose;
    private Toggle TogglePistol;
    private GameObject ButtonWeapon;
    private int WeaponCount;
    private bool isFirstEnter;
    public PanelSmithingTable(IPanel parent) : base(parent)
    {
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        children.Add(new PanelSmithingAlert(this));
    }
    protected override void OnInit()
    {
        base.OnInit();
        ButtonClose = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonClose");
        ButtonWeapon = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "BadPistol");
        TogglePistol = UnityTool.Instance.GetComponentFromChild<Toggle>(m_GameObject, "TogglePistol");
        ButtonClose.onClick.AddListener(() =>
        {
            OnExit();
        });
        foreach (WeaponCategory category in Enum.GetValues(typeof(WeaponCategory)))
        {
            if (category != WeaponCategory.Pistol)
            {
                GameObject obj = UnityEngine.Object.Instantiate(TogglePistol.gameObject, TogglePistol.transform.parent);
                obj.name = "Toggle" + category.ToString();
                obj.transform.Find("Image").GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetWeaponSprite(GetWeaponTypeByCategory(category));
                AddListenerToToggle(obj.GetComponent<Toggle>(), category);
            }
            else
            {
                AddListenerToToggle(TogglePistol, category);
            }
        }
    }
    private void AddListenerToToggle(Toggle toggle, WeaponCategory category)
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                ClearChildren(ButtonWeapon.transform.parent);
                WeaponCount = 0;
                foreach (PlayerWeaponType type in Enum.GetValues(typeof(PlayerWeaponType)))
                {
                    PlayerWeaponShareAttribute attr = AttributeFactory.Instance.GetPlayerWeaponAttr(type);
                    if (attr.Category == category)
                    {
                        if (WeaponCount < 1)
                        {
                            ButtonWeapon.transform.GetChild(0).GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetWeaponSprite(type.ToString());
                            ButtonWeapon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LanguageCommand.Instance.GetTranslation(attr.Type.ToString());
                            ButtonWeapon.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                EnterPanel(typeof(PanelSmithingAlert));
                                EventCenter.Instance.NotisfyObserver(EventType.OnConfirmSmithingWeapon, attr);
                            });
                        }
                        else
                        {
                            GameObject obj = UnityEngine.Object.Instantiate(ButtonWeapon, ButtonWeapon.transform.parent);
                            obj.name = type.ToString();
                            obj.transform.GetChild(0).GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetWeaponSprite(type.ToString());
                            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LanguageCommand.Instance.GetTranslation(type.ToString());
                            obj.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                EnterPanel(typeof(PanelSmithingAlert));
                                EventCenter.Instance.NotisfyObserver(EventType.OnConfirmSmithingWeapon, attr);
                            });
                        }
                        WeaponCount++;
                    }
                }
            }
        });

    }
    private void ClearChildren(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject != ButtonWeapon)
            {
                UnityEngine.Object.Destroy(t.GetChild(i).gameObject);
            }
        }
    }
    private string GetWeaponTypeByCategory(WeaponCategory category)
    {
        foreach (PlayerWeaponType type in Enum.GetValues(typeof(PlayerWeaponType)))
        {
            if (AttributeFactory.Instance.GetPlayerWeaponAttr(type).Category == category)
            {
                return type.ToString();
            }
        }
        return null;
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        if (!isFirstEnter)
        {
            isFirstEnter = true;
            TogglePistol.isOn = true;
        }
        Time.timeScale = 0;
        EventCenter.Instance.NotisfyObserver(EventType.OnPause);
    }
    public override void OnExit()
    {
        base.OnExit();
        TogglePistol.isOn = true;
        Time.timeScale = 1;
        EventCenter.Instance.NotisfyObserver(EventType.OnResume);
    }
}

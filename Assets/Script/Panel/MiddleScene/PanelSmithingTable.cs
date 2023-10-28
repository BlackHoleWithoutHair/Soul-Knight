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
                WeaponCount = 0;
                foreach (PlayerWeaponType type in Enum.GetValues(typeof(PlayerWeaponType)))
                {
                    PlayerWeaponShareAttribute attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(type);
                    if (attr.Category == category)
                    { 
                        if(WeaponCount<ButtonWeapon.transform.parent.childCount)
                        {
                            Transform child = ButtonWeapon.transform.parent.GetChild(WeaponCount);
                            SetWeaponButton(child.gameObject, attr);
                        }
                        else
                        {
                            GameObject obj = UnityEngine.Object.Instantiate(ButtonWeapon, ButtonWeapon.transform.parent);
                            SetWeaponButton(obj, attr);
                        }
                        WeaponCount++;
                    }
                }
                if(WeaponCount!=ButtonWeapon.transform.parent.childCount)
                {
                    UnityTool.Instance.ClearResidualChild(ButtonWeapon.transform.parent,WeaponCount);
                }
            }
        });
    }
    private void SetWeaponButton(GameObject obj,PlayerWeaponShareAttribute attr)
    {
        obj.name = attr.Type.ToString();
        obj.transform.GetChild(0).GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetWeaponSprite(attr.Type.ToString());
        obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LanguageCommand.Instance.GetTranslation(attr.Type.ToString());
        obj.GetComponent<Button>().onClick.AddListener(() =>
        {
            EnterPanel(typeof(PanelSmithingAlert));
            GetPanel<PanelSmithingAlert>().SetAttr(attr);
        });
        obj.SetActive(true);
    }
    private string GetWeaponTypeByCategory(WeaponCategory category)
    {
        foreach (PlayerWeaponType type in Enum.GetValues(typeof(PlayerWeaponType)))
        {
            if(type!=PlayerWeaponType.None)
            {
                if (WeaponCommand.Instance.GetPlayerWeaponShareAttr(type).Category == category)
                {
                    return type.ToString();
                }
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

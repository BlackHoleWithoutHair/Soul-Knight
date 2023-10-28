using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IBackpackButtonWeapon
{
    protected IPanel m_Panel;
    public GameObject gameObject { get; protected set; }
    public Transform transform { get => gameObject.transform; }
    protected GameObject DivContainer;
    protected Button ButtonEquip;
    protected Button ButtonUnEquip;
    private TextMeshProUGUI TextDamage;
    private TextMeshProUGUI TextMagic;
    private TextMeshProUGUI TextScattering;
    private TextMeshProUGUI TextCritical;
    public PlayerWeaponType weapon { get; protected set; }
    public IBackpackButtonWeapon(IPanel panel, GameObject obj) 
    {
        gameObject = obj;
        m_Panel = panel;
        OnInit();
    }
    protected virtual void OnInit()
    {
        GameObject DivLeftBottom = UnityTool.Instance.GetGameObjectFromChildren(m_Panel.gameObject, "DivLeftBottom");
        DivContainer = DivLeftBottom.transform.GetChild(0).gameObject;
        ButtonEquip = UnityTool.Instance.GetComponentFromChild<Button>(DivContainer, "ButtonEquip");
        ButtonUnEquip = UnityTool.Instance.GetComponentFromChild<Button>(DivContainer, "ButtonUnEquip");
        TextDamage = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(DivContainer, "TextDamage");
        TextMagic = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(DivContainer, "TextMagic");
        TextScattering = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(DivContainer, "TextScattering");
        TextCritical = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(DivContainer, "TextCritical");
    }
    public void ShowInfo()
    {
        (m_Panel as PanelBackpack).setCurrentSelectButton(this);
        DivContainer.SetActive(true);
        PlayerWeaponShareAttribute attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(weapon);
        TextDamage.text = attr.Damage.ToString();
        TextMagic.text = attr.MagicSpend.ToString();
        TextScattering.text = attr.ScatteringRate.ToString();
        TextCritical.text = attr.CriticalRate.ToString();
    }
    public void SetPlayerWeapon(PlayerWeaponType weapon)
    {
        this.weapon = weapon;
        if (weapon == PlayerWeaponType.None)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetWeaponSprite(weapon.ToString());
            gameObject.SetActive(true);
        }
    }
}

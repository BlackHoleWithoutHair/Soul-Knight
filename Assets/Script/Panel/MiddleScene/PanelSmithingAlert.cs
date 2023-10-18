using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelSmithingAlert : IPanel
{
    private GameObject SmithingTable;
    private GameObject DivMaterialVer;
    private PlayerWeaponShareAttribute m_Attr;
    private Button ButtonClose;
    private Button ButtonOk;
    private Image ImageWeapon;
    private TextMeshProUGUI TextName;
    private TextMeshProUGUI TextDamage;
    private TextMeshProUGUI TextMagic;
    private TextMeshProUGUI TextCritical;
    private TextMeshProUGUI TextScattering;
    public PanelSmithingAlert(IPanel parent) : base(parent)
    {
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        EventCenter.Instance.RegisterObserver<PlayerWeaponShareAttribute>(EventType.OnConfirmSmithingWeapon, (attr) =>
        {
            m_Attr = attr;
        });
    }
    protected override void OnInit()
    {
        base.OnInit();
        SmithingTable = GameObject.Find("SmithingTable");
        DivMaterialVer = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivMaterialVer");
        ImageWeapon = UnityTool.Instance.GetComponentFromChild<Image>(m_GameObject, "ImageWeapon");
        ButtonClose = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonCancel");
        ButtonOk = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonOk");
        TextName = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextName");
        TextDamage = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextDamage");
        TextMagic = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextMagic");
        TextCritical = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextCritical");
        TextScattering = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextScattering");
        ButtonClose.onClick.AddListener(() =>
        {
            OnExit();
        });
        ButtonOk.onClick.AddListener(() =>
        {
            GameObject weapon = WeaponFactory.Instance.GetPlayerWeaponObj(m_Attr.Type, SmithingTable.transform.Find("WeaponCreatePoint").transform.position);
            weapon.transform.SetParent(SmithingTable.transform);
            SetWeaponCollider(weapon);
            OnExit();
            parent.OnExit();
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        TextName.text = LanguageCommand.Instance.GetTranslation(m_Attr.Type.ToString());
        TextDamage.text = m_Attr.Damage.ToString();
        TextMagic.text = m_Attr.MagicSpend.ToString();
        TextCritical.text = m_Attr.CriticalRate.ToString();
        TextScattering.text = m_Attr.ScatteringRate.ToString();
        ImageWeapon.sprite = ProxyResourceFactory.Instance.Factory.GetWeaponSprite(m_Attr.Type.ToString());

        UnityTool.Instance.DestroyChildrenExceptFirstChild(DivMaterialVer.transform.parent);
        List<MaterialInfo> infos = AttributeFactory.Instance.GetCompositionData(m_Attr.Type).materialInfos;
        for (int i = 0; i < infos.Count; i++)
        {
            if (i == 0)
            {
                SetMaterialInfo(DivMaterialVer, infos[i]);
            }
            else
            {
                GameObject obj = Object.Instantiate(DivMaterialVer, DivMaterialVer.transform.parent);
                SetMaterialInfo(obj, infos[i]);
            }
        }

    }
    private void SetWeaponCollider(GameObject weapon)
    {
        BoxCollider2D collider = weapon.transform.GetChild(0).GetComponent<BoxCollider2D>();
        BoxCollider2D table = SmithingTable.GetComponent<BoxCollider2D>();
        Vector3 worldOffest = weapon.transform.position - SmithingTable.transform.position;
        Vector2 offest = Vector2.zero;
        offest.Set(table.offset.x - worldOffest.x, table.offset.y - worldOffest.y);
        collider.size = table.size;
        collider.offset = offest;
    }
    private void SetMaterialInfo(GameObject obj, MaterialInfo info)
    {
        obj.transform.Find("ImageMaterial").GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetMaterialSprite(info.materialType.ToString());
        obj.transform.Find("TextSpend").GetComponent<TextMeshProUGUI>().text = info.num.ToString();
    }

}

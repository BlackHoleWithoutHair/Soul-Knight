using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelBackpack:IPanel
{
    private Image ImageProfile;
    private GameObject DivContainerInfo;
    private IBackpackButtonWeapon oldSelectButton;//用于显示外边框
    private IBackpackButtonWeapon currentSelectButton;
    private List<ButtonEquippedWeapon> ImageEquippedWeapons;//已装备的
    private List<ButtonUnEquippedWeapon> ImageUnEquippedWeapons;
    private Button ButtonUnEquip;
    private Button ButtonEquip;
    private BackpackSystem system;
    public PanelBackpack(IPanel parent) : base(parent) 
    {
        isShowPanelAfterExit = true;
    }
    protected override void OnInit()
    {
        base.OnInit();
        system = GameMediator.Instance.GetSystem<BackpackSystem>();
        ImageEquippedWeapons = new List<ButtonEquippedWeapon>();
        ImageUnEquippedWeapons= new List<ButtonUnEquippedWeapon>();
        m_GameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1080);
        GameObject DivLeftBottom = UnityTool.Instance.GetGameObjectFromChildren(gameObject, "DivLeftBottom");
        DivContainerInfo = DivLeftBottom.transform.GetChild(0).gameObject;
        ButtonUnEquip = UnityTool.Instance.GetComponentFromChild<Button>(DivContainerInfo, "ButtonUnEquip");
        ButtonEquip = UnityTool.Instance.GetComponentFromChild<Button>(DivContainerInfo, "ButtonEquip");
        ImageProfile = UnityTool.Instance.GetComponentFromChild<Image>(m_GameObject, "Profile");
        for(int i=0;i<3;i++)
        {
            ButtonEquippedWeapon item = new ButtonEquippedWeapon(this, UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "Weapon" + i));
            ImageEquippedWeapons.Add(item);
        }
        GameObject cell0 = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "Cell0");
        for(int i=0;i<20;i++)
        {
            if(i!=0)
            {
                GameObject obj = Object.Instantiate(cell0, cell0.transform.parent);
                obj.name = "Cell" + i;
                ButtonUnEquippedWeapon item = new ButtonUnEquippedWeapon(this, UnityTool.Instance.GetGameObjectFromChildren(obj,"ImageWeapon"));
                ImageUnEquippedWeapons.Add(item);
            }
            else
            {
                ButtonUnEquippedWeapon item = new ButtonUnEquippedWeapon(this, UnityTool.Instance.GetGameObjectFromChildren(cell0, "ImageWeapon"));
                ImageUnEquippedWeapons.Add(item);
            }
        }
        UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonBack").onClick.AddListener(() =>
        {
            OnExit();
        });
        ButtonUnEquip.onClick.AddListener(() =>
        {
            if(currentSelectButton is ButtonEquippedWeapon)
            {
                GameMediator.Instance.GetController<PlayerController>().Player.RemoveWeapon(currentSelectButton.weapon);
                GameMediator.Instance.GetSystem<BackpackSystem>().AddWeapon(currentSelectButton.weapon);
                RefreshInfo();
                DivContainerInfo.SetActive(false);
                HideOutline();
            }
            else
            {

            }
        });
        ButtonEquip.onClick.AddListener(() =>
        {
            IPlayer player = GameMediator.Instance.GetController<PlayerController>().Player;
            if(player.m_Attr.Weapons.Count<3)
            {
                player.AddWeapon(currentSelectButton.weapon);
                GameMediator.Instance.GetSystem<BackpackSystem>().RemoveWeapon(currentSelectButton.weapon);
                RefreshInfo();
                DivContainerInfo.SetActive(false);
                HideOutline();
            }
            else
            {
                EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "持有武器数量已达上限");
            }
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        PlayAppearAnim();
        Time.timeScale = 0;
        EventCenter.Instance.NotisfyObserver(EventType.OnPause);
        DivContainerInfo.SetActive(false);
        RefreshInfo();
        HideOutline();
    }
    public void RefreshInfo()
    {
        IPlayer player = GameMediator.Instance.GetController<PlayerController>().Player; 
        ImageProfile.sprite = ProxyResourceFactory.Instance.Factory.GetProfileSprite(player.m_Attr.CurrentSkinType.ToString());
        int i;
        for (i = 0; i < player.m_Attr.Weapons.Count; i++)
        {
            ImageEquippedWeapons[i].SetPlayerWeapon(player.m_Attr.Weapons[i].m_Attr.Type);
        }
        for (int k = i; k < 3; k++)
        {
            ImageEquippedWeapons[k].SetPlayerWeapon(PlayerWeaponType.None);
        }
        for (i = 0; i < system.Weapons.Count; i++)
        {
            ImageUnEquippedWeapons[i].SetPlayerWeapon(system.Weapons[i]);
        }
        for (int k = i; k < 20; k++)
        {
            ImageUnEquippedWeapons[i].SetPlayerWeapon(PlayerWeaponType.None);
        }
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if(currentSelectButton!=oldSelectButton)
        {
            oldSelectButton?.transform.parent.Find("OutLine").gameObject.SetActive(false);
            currentSelectButton?.transform.parent.Find("OutLine").gameObject.SetActive(true);
            oldSelectButton = currentSelectButton;
        }
    }
    public override void OnExit()
    {
        base.OnExit();
        Time.timeScale = 1;
        PlayDisappearAnim();
        EventCenter.Instance.NotisfyObserver(EventType.OnResume);
    }
    private void PlayAppearAnim()
    {
        m_GameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 0.3f).SetUpdate(true);
    }
    private void PlayDisappearAnim()
    {
        m_GameObject.GetComponent<RectTransform>().DOAnchorPosY(1080, 0.3f);
    }
    private void HideOutline()
    {
        currentSelectButton?.transform.parent.Find("OutLine").gameObject.SetActive(false);
        oldSelectButton = null;
        currentSelectButton = null;
    }
    public void setCurrentSelectButton(IBackpackButtonWeapon obj)
    {
        currentSelectButton = obj;
    }
}

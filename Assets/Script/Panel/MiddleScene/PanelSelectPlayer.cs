using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleScene
{
    public class PanelSelectPlayer : IPanel
    {
        private Button ButtonBack;
        private Button ButtonStart;
        private Button ButtonUpgrade;
        private Button ButtonLeftSkin;
        private Button ButtonRightSkin;
        private Toggle ToggleSkillOne;
        private Image ImageWeapon;
        private Image ImageRightSkill;
        private List<Image> ImageStars;
        private TextMeshProUGUI TextName;
        private TextMeshProUGUI TextHp;
        private TextMeshProUGUI TextArmor;
        private TextMeshProUGUI TextMp;
        private TextMeshProUGUI TextCritical;
        private TextMeshProUGUI TextSpend;
        private TextMeshProUGUI TextSkillName;
        private TextMeshProUGUI TextSkillDescription;
        private GameObject Title;
        private GameObject DivMiddleVer;
        private GameObject DivMiddleVerNext;
        private GameObject DivLeft;
        private GameObject DivRight;
        private GameObject DivCenter;
        private GameObject DivBottom;
        private PlayerAttribute m_Attr;
        private GameObject selectObj;
        private PlayerType selectType;
        private Color ColorDark;
        private int SkinIndex;
        private int SkinCount;
        private bool isDivMiddleVerNextShow;
        public PanelSelectPlayer(IPanel parent) : base(parent)
        {
            isShowPanelAfterExit = true;
            ColorDark = new Color(100f / 255f, 100f / 255f, 100f / 255f);
            ImageStars = new List<Image>();
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
            children.Add(new PanelBattle(this));
            isDivMiddleVerNextShow = false;
        }
        protected override void OnInit()
        {
            base.OnInit();
            EventCenter.Instance.RegisterObserver<GameObject>(EventType.OnPlayerClick, OnPlayerSelect);
            ButtonBack = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonBack");
            ButtonStart = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonStart");
            ButtonUpgrade = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonUpgrade");
            ButtonLeftSkin = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonLeftSkin");
            ButtonRightSkin = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonRightSkin");
            ToggleSkillOne = UnityTool.Instance.GetComponentFromChild<Toggle>(m_GameObject, "ToggleSkill1");
            ImageWeapon = UnityTool.Instance.GetComponentFromChild<Image>(m_GameObject, "ImageWeapon");
            ImageRightSkill = UnityTool.Instance.GetComponentFromChild<Image>(m_GameObject, "ImageRightSkill");
            TextName = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "CharacterName");
            TextHp = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextHp");
            TextMp = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextMp");
            TextArmor = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextArmor");
            TextCritical = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextCritical");
            TextSpend = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextSpend");
            TextSkillName = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextSkillName");
            TextSkillDescription = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextSkillDescription");
            Title = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "Title");
            DivMiddleVer = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivMiddleVer");
            DivMiddleVerNext = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivMiddleVerNext");
            DivBottom = m_GameObject.transform.Find("DivBottom").gameObject;
            DivLeft = UnityTool.Instance.GetGameObjectFromChildren(DivMiddleVer, "DivLeft");
            DivRight = UnityTool.Instance.GetGameObjectFromChildren(DivMiddleVer, "DivRight");
            DivCenter = UnityTool.Instance.GetGameObjectFromChildren(DivMiddleVer, "DivCenter");
            Title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 55);
            DivBottom.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -130);
            DivLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 0);
            DivRight.GetComponent<RectTransform>().anchoredPosition = new Vector2(300, 0);
            ButtonLeftSkin.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350, 0);
            ButtonRightSkin.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(350f, 0);
            DivCenter.GetComponent<CanvasGroup>().alpha = 0f;
            DivMiddleVerNext.SetActive(true);
            for (int i = 0; i < 5; i++)
            {
                ImageStars.Add(UnityTool.Instance.GetComponentFromChild<Image>(m_GameObject, "Star" + i));
            }
            ButtonBack.onClick.AddListener(() =>
            {
                if (!isDivMiddleVerNextShow)
                {
                    OnExit();
                    CameraUtility.Instance.ChangeActiveCamera(CameraType.StaticCamera);
                }
                else
                {
                    isDivMiddleVerNextShow = false;
                    DivLeft.GetComponent<RectTransform>().DOAnchorPosX(300, 0.3f);
                    DivRight.GetComponent<RectTransform>().DOAnchorPosX(-300, 0.3f);
                    ButtonLeftSkin.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(-350, 0.3f);
                    ButtonRightSkin.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(350, 0.3f);
                }

            });
            ButtonStart.onClick.AddListener(() =>
            {
                if (isDivMiddleVerNextShow)
                {
                    isDivMiddleVerNextShow = false;
                    EnterPanel(typeof(PanelBattle));
                    CameraUtility.Instance.ChangeActiveCamera(CameraType.FollowCamera);
                    EventCenter.Instance.NotisfyObserver(EventType.OnFinishSelectPlayer);
                    GameMediator.Instance.GetController<PlayerController>().SetPlayer(m_Attr);

                    Title.GetComponent<RectTransform>().DOAnchorPosY(55, 0.3f);
                    ButtonLeftSkin.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(-350, 0.3f);
                    ButtonRightSkin.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(350, 0.3f);
                    DivBottom.GetComponent<RectTransform>().DOAnchorPosY(-130, 0.3f);
                    DivCenter.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
                }
                else
                {
                    isDivMiddleVerNextShow = true;

                    DivLeft.GetComponent<RectTransform>().DOAnchorPosX(-300, 0.3f);
                    DivRight.GetComponent<RectTransform>().DOAnchorPosX(300, 0.3f);
                    ButtonLeftSkin.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(0, 0.3f);
                    ButtonRightSkin.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(0, 0.3f);
                }
            });
            ButtonRightSkin.onClick.AddListener(() =>
            {
                SkinIndex++;
                m_Attr.CurrentSkinType = m_Attr.m_ShareAttr.SkinTypes[GetSkinIndex(SkinIndex)];
                GameObject.Find(m_Attr.m_ShareAttr.Type.ToString()).transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = ProxyResourceFactory.Instance.Factory.GetCharacterAnimatorController(m_Attr.CurrentSkinType.ToString());
            });
            ButtonLeftSkin.onClick.AddListener(() =>
            {
                SkinIndex--;
                m_Attr.CurrentSkinType = m_Attr.m_ShareAttr.SkinTypes[GetSkinIndex(SkinIndex)];
                GameObject.Find(m_Attr.m_ShareAttr.Type.ToString()).transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = ProxyResourceFactory.Instance.Factory.GetCharacterAnimatorController(m_Attr.CurrentSkinType.ToString());
            });
            ButtonUpgrade.onClick.AddListener(() =>
            {
                if (ArchiveCommand.Instance.GetMaterialNum(MaterialType.Gem) >= int.Parse(TextSpend.text))
                {
                    ArchiveCommand.Instance.SpendMaterial(MaterialType.Gem, int.Parse(TextSpend.text));
                    m_Attr.CurrentLv += 1;
                    ModelContainer.Instance.GetModel<ArchiveModel>().SaveGameData();
                }
            });
        }
        protected override void OnEnter()
        {
            base.OnEnter();
            m_Attr = AttributeFactory.Instance.GetPlayerAttr(selectType);
            m_Attr.CurrentSkillType = m_Attr.m_ShareAttr.SkillTypes[0];
            m_Attr.CurrentSkinType = m_Attr.m_ShareAttr.SkinTypes[0];
            SkinCount = m_Attr.m_ShareAttr.SkinTypes.Count;
            ToggleSkillOne.isOn = true;
            SetWeaponSprite(m_Attr);
            SetRightSkillSprite(m_Attr, 0);
            ClearToggleGroup();
            for (int i = 0; i < m_Attr.m_ShareAttr.SkillTypes.Count; i++)
            {
                int j = i;
                Toggle toggle = null;
                if (i == 0)
                {
                    toggle = ToggleSkillOne;
                    ToggleSkillOne.transform.Find("ImageLeftSkill").GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetSkillSprite(m_Attr.m_ShareAttr.SkillTypes[i].ToString());
                }
                else
                {
                    if (m_Attr.m_ShareAttr.SkillTypes[i] != SkillType.None)
                    {
                        toggle = Object.Instantiate(ToggleSkillOne.gameObject, ToggleSkillOne.transform.parent).GetComponent<Toggle>();
                        toggle.transform.Find("ImageLeftSkill").GetComponent<Image>().sprite = ProxyResourceFactory.Instance.Factory.GetSkillSprite(m_Attr.m_ShareAttr.SkillTypes[i].ToString());
                    }
                }
                if (toggle != null)
                {
                    toggle.onValueChanged.RemoveAllListeners();
                    toggle.onValueChanged.AddListener((isOn) =>
                    {
                        if (isOn)
                        {
                            SetRightSkillSprite(m_Attr, j);
                            m_Attr.CurrentSkillType = m_Attr.m_ShareAttr.SkillTypes[j];
                        }
                    });
                }
            }
            ToggleSkillOne.isOn = true;
            CameraUtility.Instance.ChangeActiveCamera(CameraType.SelectCamera);
            CameraUtility.Instance.SetSelect(GameObject.Find(selectType.ToString()).transform);
            TextName.text = m_Attr.m_ShareAttr.PlayerName;
            if (m_Attr.CurrentLv == 7)
            {
                ButtonUpgrade.gameObject.SetActive(false);
            }
            Title.GetComponent<RectTransform>().DOAnchorPosY(-55, 0.3f);
            DivBottom.GetComponent<RectTransform>().DOAnchorPosY(130, 0.3f);
            DivLeft.GetComponent<RectTransform>().DOAnchorPosX(300, 0.3f);
            DivRight.GetComponent<RectTransform>().DOAnchorPosX(-300, 0.3f);
            DivCenter.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            TextHp.text = m_Attr.m_ShareAttr.MaxHp.ToString();
            TextMp.text = m_Attr.m_ShareAttr.Magic.ToString();
            TextArmor.text = m_Attr.m_ShareAttr.Armor.ToString();
            TextCritical.text = m_Attr.m_ShareAttr.Critical.ToString();
            SetStarColor(m_Attr.CurrentLv);
            SetSpendText(m_Attr.CurrentLv);
        }
        public override void OnExit()
        {
            base.OnExit();
            Title.GetComponent<RectTransform>().DOAnchorPosY(55, 0.3f);
            DivBottom.GetComponent<RectTransform>().DOAnchorPosY(-130, 0.3f);
            DivLeft.GetComponent<RectTransform>().DOAnchorPosX(-300, 0.3f);
            DivRight.GetComponent<RectTransform>().DOAnchorPosX(300, 0.3f);
            DivCenter.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
        }

        private void OnPlayerSelect(GameObject obj)
        {
            selectObj = obj;
            selectType = selectObj.GetComponent<CheckPlayerClick>().PlayerType;
        }
        private void SetWeaponSprite(PlayerAttribute attr)
        {
            ImageWeapon.sprite = ProxyResourceFactory.Instance.Factory.GetWeaponSprite(attr.m_ShareAttr.IdleWeapon.ToString());
            ImageWeapon.SetNativeSize();
            RectTransform t = ImageWeapon.GetComponent<RectTransform>();
            float MaxScale = 1 / Mathf.Max(t.sizeDelta.x / 150, t.sizeDelta.y / 150);
            t.localScale = new Vector3(MaxScale * 0.8f, MaxScale * 0.8f, 1);
        }
        private void SetRightSkillSprite(PlayerAttribute attr, int index)
        {
            if (attr.m_ShareAttr.SkillTypes[index] != SkillType.None)
            {
                ImageRightSkill.sprite = ProxyResourceFactory.Instance.Factory.GetSkillSprite(attr.m_ShareAttr.SkillTypes[index].ToString());
                TextSkillName.text = AttributeFactory.Instance.GetSkillAttr(attr.m_ShareAttr.SkillTypes[index], attr).m_ShareAttr.SkillName;
                TextSkillDescription.text = AttributeFactory.Instance.GetSkillAttr(attr.m_ShareAttr.SkillTypes[index], attr).m_ShareAttr.SkillDescription;
            }
        }
        private void SetStarColor(int lv)
        {
            lv = Mathf.Clamp(lv, 0, 5);
            for (int i = 0; i < lv; i++)
            {
                ImageStars[i].color = Color.white;
            }
            for (int i = lv; i < 5; i++)
            {
                ImageStars[i].color = ColorDark;
            }
        }
        private void SetSpendText(int lv)
        {
            int num = 0;
            switch (lv)
            {
                case 0:
                    num = 500;
                    break;
                case 1:
                    num = 1000;
                    break;
                case 2:
                    num = 1500;
                    break;
                case 3:
                    num = 2000;
                    break;
                case 4:
                    num = 2500;
                    break;
                case 5:
                    num = 5000;
                    break;
                case 6:
                    num = 8000;
                    break;
            }
            TextSpend.text = num.ToString();
        }
        private int GetSkinIndex(int index)
        {
            if (index < 0)
            {
                index = -index;
            }
            return index % SkinCount;
        }
        private void ClearToggleGroup()
        {
            for (int i = 0; i < ToggleSkillOne.transform.parent.childCount; i++)
            {
                if (ToggleSkillOne.transform.parent.GetChild(i) != ToggleSkillOne.transform)
                {
                    Object.Destroy(ToggleSkillOne.transform.parent.GetChild(i).gameObject);
                }
            }
        }
    }
}


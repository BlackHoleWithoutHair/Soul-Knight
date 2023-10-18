using MiddleScene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScene
{
    public class PanelRoot : IPanel
    {
        private Animator m_Animator;
        private Button ButtonPause;
        private Slider SliderHp;
        private Slider SliderMp;
        private Slider SliderArmor;
        private TextMeshProUGUI TextHp;
        private TextMeshProUGUI TextMp;
        private TextMeshProUGUI TextArmor;
        private TextMeshProUGUI TextMoney;
        private TextMeshProUGUI TextMiddle;

        private MemoryModel m_MemoryModel;
        private AnimatorStateInfo info;

        private bool isFirstEnter;
        private bool isInit;
        public PanelRoot() : base(null)
        {
            isFirstEnter = true;
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
            m_Animator = m_GameObject.GetComponent<Animator>();
            children.Add(new PanelPause(this));
            children.Add(new PanelResurrection(this));
            children.Add(new PanelWait(this));
            m_MemoryModel = ModelContainer.Instance.GetModel<MemoryModel>();
        }
        protected override void OnInit()
        {
            base.OnInit();
            OnResume();
            ButtonPause = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonPause");
            SliderHp = UnityTool.Instance.GetComponentFromChild<Slider>(m_GameObject, "SliderHp");
            SliderMp = UnityTool.Instance.GetComponentFromChild<Slider>(m_GameObject, "SliderMp");
            SliderArmor = UnityTool.Instance.GetComponentFromChild<Slider>(m_GameObject, "SliderArmor");
            TextHp = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(SliderHp.gameObject, "Text");
            TextMp = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(SliderMp.gameObject, "Text");
            TextArmor = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(SliderArmor.gameObject, "Text");
            TextMoney = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextMoney");
            TextMiddle = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextMiddle");
            ButtonPause.onClick.AddListener(() =>
            {
                EnterPanel(typeof(PanelPause));
                m_GameObject.SetActive(false);
            });
        }
        protected override void OnEnter()
        {
            base.OnEnter();
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            info = m_Animator.GetCurrentAnimatorStateInfo(0);
            //Debug.Log(GetPlayer().m_Attr.CurrentHp);
            if (GetPlayer() != null)
            {
                SliderHp.value = GetPlayer().m_Attr.CurrentHp / (float)GetPlayer().m_Attr.m_ShareAttr.MaxHp;
                SliderArmor.value = GetPlayer().m_Attr.CurrentArmor / (float)GetPlayer().m_Attr.m_ShareAttr.Armor;
                SliderMp.value = GetPlayer().m_Attr.CurrentMp / (float)GetPlayer().m_Attr.m_ShareAttr.Magic;
                TextHp.text = GetPlayer().m_Attr.CurrentHp + "/" + GetPlayer().m_Attr.m_ShareAttr.MaxHp;
                TextMp.text = GetPlayer().m_Attr.CurrentMp + "/" + GetPlayer().m_Attr.m_ShareAttr.Magic;
                TextArmor.text = GetPlayer().m_Attr.CurrentArmor + "/" + GetPlayer().m_Attr.m_ShareAttr.Armor;
                TextMoney.text = m_MemoryModel.Money.ToString();
                if (GetPlayer().IsDie)
                {
                    EnterPanel(typeof(PanelResurrection));
                }
            }
            if (isFirstEnter)
            {
                isFirstEnter = false;
                EnterPanel(typeof(PanelWait));
            }
            else if (!isInit)
            {
                isInit = true;
                m_Animator.enabled = true;
                CoroutinePool.Instance.StartAnimatorCallback(m_Animator, "SceneStart", () =>
                {
                    m_Animator.enabled = false;
                    GameMediator.Instance.GetController<PlayerController>().Player.EnterBattleScene();
                    GameMediator.Instance.GetController<PlayerController>().Player.m_Attr.isPause = false;
                });
                TextMiddle.text = GetStageText();
            }
        }
        private IPlayer GetPlayer()
        {
            return GameMediator.Instance.GetController<PlayerController>().Player;
        }
        private string GetStageText()
        {
            int bigStage = m_MemoryModel.Stage / 5 + 1;
            int smallStage = m_MemoryModel.Stage - (bigStage - 1) * 5;
            return bigStage + "-" + smallStage;
        }
    }
}

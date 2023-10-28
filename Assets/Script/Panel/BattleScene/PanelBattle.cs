using MiddleScene;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace BattleScene
{
    public class PanelBattle : IPanel
    {
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
        private PlayableDirector TimeLine;
        private bool isFirstEnter;
        public PanelBattle(IPanel parent) : base(parent)
        {
            isFirstEnter = true;
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
            TimeLine = GameObject.Find("TimeLine").GetComponent<PlayableDirector>();
            children.Add(new PanelPause(this));
            children.Add(new PanelResurrection(this));
            children.Add(new PanelBossCutScene(this));
            m_MemoryModel = ModelContainer.Instance.GetModel<MemoryModel>();
        }
        protected override void OnInit()
        {
            base.OnInit();
            ButtonPause = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonPause");
            SliderHp = UnityTool.Instance.GetComponentFromChild<Slider>(m_GameObject, "SliderHp");
            SliderMp = UnityTool.Instance.GetComponentFromChild<Slider>(m_GameObject, "SliderMp");
            SliderArmor = UnityTool.Instance.GetComponentFromChild<Slider>(m_GameObject, "SliderArmor");
            TextHp = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(SliderHp.gameObject, "Text");
            TextMp = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(SliderMp.gameObject, "Text");
            TextArmor = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(SliderArmor.gameObject, "Text");
            TextMoney = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextMoney");
            TextMiddle = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextMiddle");
            EventCenter.Instance.RegisterObserver<Room>(EventType.OnPlayerEnterBossRoom, (enterRoom) =>
            {
                EventCenter.Instance.NotisfyObserver(EventType.OnPause);
                CoroutinePool.Instance.DelayInvoke(() =>
                {
                    EnterPanel(typeof(PanelBossCutScene));
                    OnResume();
                }, 0.5f);
            });
            ButtonPause.onClick.AddListener(() =>
            {
                EnterPanel(typeof(PanelPause));
                m_GameObject.SetActive(false);
            });
        }
        protected override void OnEnter()
        {
            base.OnEnter();
            if (isFirstEnter)
            {
                isFirstEnter = false;
                TimeLine.Play();
                CoroutinePool.Instance.DelayInvoke(() =>
                {
                    gameObject.GetComponent<Animator>().enabled = false;
                    GameMediator.Instance.GetController<PlayerController>().Player.EnterBattleScene();
                    GameMediator.Instance.GetController<PlayerController>().Player.m_Attr.isRun = true;
                }, (float)TimeLine.duration);
                TextMiddle.text = GetStageText();
            }
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
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
        }
        private IPlayer GetPlayer()
        {
            return GameMediator.Instance.GetController<PlayerController>().Player;
        }
        private string GetStageText()
        {
            return MemoryModelCommand.Instance.GetBigStage() + "-" + MemoryModelCommand.Instance.GetSmallStage();
        }
    }
}

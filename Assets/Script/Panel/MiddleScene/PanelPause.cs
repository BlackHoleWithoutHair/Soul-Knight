using UnityEngine;
using UnityEngine.UI;

namespace MiddleScene
{
    public class PanelPause : IPanel
    {
        private Button ButtonHome;
        private Button ButtonBack;
        private Button ButtonSetting;
        public PanelPause(IPanel parent) : base(parent)
        {
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        }
        protected override void OnInit()
        {
            base.OnInit();
            ButtonHome = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonHome");
            ButtonBack = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonBack");
            ButtonSetting = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonSetting");
            ButtonHome.onClick.AddListener(() =>
            {
                SceneModelCommand.Instance.LoadScene(SceneName.MainMenuScene);
            });
            ButtonBack.onClick.AddListener(() =>
            {
                OnExit();
            });
            ButtonSetting.onClick.AddListener(() =>
            {

            });
        }
        protected override void OnEnter()
        {
            base.OnEnter();
            Time.timeScale = 0;
            EventCenter.Instance.NotisfyObserver(EventType.OnPause);
        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            EventCenter.Instance.NotisfyObserver(EventType.OnResume);
        }
    }
}


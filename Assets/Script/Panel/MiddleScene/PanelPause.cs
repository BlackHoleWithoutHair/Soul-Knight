using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MiddleScene
{
    public class PanelPause : IPanel
    {
        private Button ButtonHome;
        private Button ButtonBack;
        private Button ButtonSetting;
        private GameObject DivMiddle;
        private Image Profile;
        private Image[] ImageTalents;
        private IPlayer player;
        public PanelPause(IPanel parent) : base(parent)
        {
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        }
        protected override void OnInit()
        {
            base.OnInit();
            ImageTalents = new Image[6];
            ButtonHome = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonHome");
            ButtonBack = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonBack");
            ButtonSetting = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonSetting");
            Profile = UnityTool.Instance.GetComponentFromChild<Image>(m_GameObject, "Profile");
            DivMiddle = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivMiddle");
            for (int i = 0; i < DivMiddle.transform.childCount; i++)
            {
                ImageTalents[i]=DivMiddle.transform.GetChild(i).GetComponent<Image>();
            }
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
            player = GameMediator.Instance.GetController<PlayerController>().Player;
            Time.timeScale = 0;
            EventCenter.Instance.NotisfyObserver(EventType.OnPause);
            Profile.sprite = ProxyResourceFactory.Instance.Factory.GetProfileSprite(player.m_Attr.CurrentSkinType.ToString());
            List<ITalent> talents = GameMediator.Instance.GetSystem<TalentSystem>().GetTalents(player);
            if (talents != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (i<talents.Count)
                    {
                        ImageTalents[i].sprite = ProxyResourceFactory.Instance.Factory.GetTalentSprite(talents[i].type.ToString());
                    }
                    else
                    {
                        ImageTalents[i].sprite = ProxyResourceFactory.Instance.Factory.GetTalentSprite("Empty");
                    }
                }
            }

        }
        public override void OnExit()
        {
            base.OnExit();
            Time.timeScale = 1;
            EventCenter.Instance.NotisfyObserver(EventType.OnResume);
        }
    }
}


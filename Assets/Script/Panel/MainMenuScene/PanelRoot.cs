using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuScene
{
    public class PanelRoot : IPanel
    {
        private Button ButtonPanel;
        private Button ButtonLogin;
        private Button ButtonSetting;
        private Button ButtonSinglePlayer;
        private Button ButtonMultiPlayer;
        private GameObject DivStart;
        private GameObject DivLeft;
        private GameObject TextGameStart;
        private bool isDivStartUp;
        private bool isDivLeftLeft;

        public PanelRoot() : base(null)
        {
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
            children.Add(new PanelLogin(this));
            children.Add(new PanelOnlineAlert(this));
            children.Add(new PanelSetting(this));
            isDivStartUp = false;
            isDivLeftLeft = false;
        }
        protected override void OnInit()
        {
            base.OnInit();
            OnResume();
            TextGameStart = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "TextStart");
            ButtonLogin = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonLogin");
            ButtonSetting = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonSetting");
            ButtonPanel = m_GameObject.GetComponent<Button>();
            ButtonSinglePlayer = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonSinglePlayer");
            ButtonMultiPlayer = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonMultiPlayer");
            DivStart = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivStart");
            DivLeft = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivLeft");
            DivStart.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -125);
            ButtonPanel.onClick.AddListener(() =>
            {
                TextGameStart.SetActive(!TextGameStart.activeSelf);
                if (!isDivStartUp)
                {
                    Debug.Log(1);
                    isDivStartUp = !isDivStartUp;
                    DivStart.SetActive(true);
                    DivStart.GetComponent<RectTransform>().DOAnchorPosY(125, 0.3f);
                    Debug.Log(1);

                }
                else
                {
                    Debug.Log(2);
                    isDivStartUp = !isDivStartUp;
                    DivStart.SetActive(true);
                    DivStart.GetComponent<RectTransform>().DOAnchorPosY(-125, 0.3f);
                }
                
                if (!isDivLeftLeft)
                {
                    isDivLeftLeft = !isDivLeftLeft;
                    DivLeft.GetComponent<RectTransform>().DOAnchorPosX(-120, 0.3f);
                }
                else
                {
                    isDivLeftLeft = !isDivLeftLeft;
                    DivLeft.GetComponent<RectTransform>().DOAnchorPosX(120, 0.3f);
                }
            });
            ButtonSetting.onClick.AddListener(() =>
            {
                EnterPanel(typeof(PanelSetting));
            });
            ButtonSinglePlayer.onClick.AddListener(() =>
            {
                SceneModelCommand.Instance.LoadScene(SceneName.MiddleScene);
            });
            ButtonMultiPlayer.onClick.AddListener(() =>
            {
                EnterPanel(typeof(PanelOnlineAlert));
            });
            ButtonLogin.onClick.AddListener(() =>
            {
                EnterPanel(typeof(PanelLogin));
            });
        }
        protected override void OnEnter()
        {
            base.OnEnter();
            if (!isDivStartUp)
            {
                TextGameStart.SetActive(true);
                DivLeft.GetComponent<RectTransform>().DOAnchorPosX(120, 0.3f);
                isDivLeftLeft = false;
            }
        }
        protected override void EnterPanel(Type type)
        {
            base.EnterPanel(type);
            TextGameStart.SetActive(false);
            DivLeft.GetComponent<RectTransform>().DOAnchorPosX(-120, 0.3f);
            isDivLeftLeft = true;
        }
    }
}


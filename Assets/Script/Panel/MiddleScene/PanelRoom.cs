using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleScene
{
    public class PanelRoom : IPanel
    {
        private GameObject DivBottom;
        private GameObject Title;
        public PanelRoom(IPanel parent) : base(parent)
        {
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
            children.Add(new PanelGemStore(this));
            children.Add(new PanelSelectPlayer(this));
            children.Add(new PanelSelectPet(this));
        }
        protected override void OnInit()
        {
            base.OnInit();
            DivBottom = m_GameObject.transform.Find("DivBottom").gameObject;
            Title = m_GameObject.transform.Find("Title").gameObject;
            UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonHome").onClick.AddListener(() =>
            {
                SceneModelCommand.Instance.LoadScene(SceneName.MainMenuScene);
            });
            UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonStore").onClick.AddListener(() =>
            {
                EnterPanel(typeof(PanelGemStore));
            });
            EventCenter.Instance.RegisterObserver<GameObject>(EventType.OnPlayerClick, (obj) =>
            {
                EnterPanel(typeof(PanelSelectPlayer));
                DisappearAnim();
            });
            EventCenter.Instance.RegisterObserver(EventType.OnPetClick, () =>
            {
                EnterPanel(typeof(PanelSelectPet));
                DisappearAnim();
            });
        }
        protected override void OnEnter()
        {
            base.OnEnter();
            DivBottom.GetComponent<RectTransform>().DOAnchorPosY(130, 0.3f);
            Title.GetComponent<RectTransform>().DOAnchorPosY(-45, 0.3f);
        }
        private void DisappearAnim()
        {
            DivBottom.GetComponent<RectTransform>().DOAnchorPosY(-130, 0.3f);
            Title.GetComponent<RectTransform>().DOAnchorPosY(45, 0.3f);
        }
    }
}


using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace BattleScene
{
    public class PanelRoot : IPanel
    {
        private TextMeshProUGUI Tip;
        private List<string> tips;
        public PanelRoot() : base(null)
        {
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
            children.Add(new PanelBattle(this));
        }
        protected override void OnInit()
        {
            base.OnInit();
            OnResume();
            Tip = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "Text");
            tips = ProxyResourceFactory.Instance.Factory.GetScriptableObject<TipScriptableObject>().tips;
            EventCenter.Instance.RegisterObserver(EventType.OnCameraArriveAtPlayer, () =>
            {
                EnterPanel(typeof(PanelBattle));
                m_GameObject.SetActive(false);
            });
        }
        protected override void OnEnter()
        {
            base.OnEnter();
            Tip.text = tips[Random.Range(0, tips.Count)];
        }
    }
}


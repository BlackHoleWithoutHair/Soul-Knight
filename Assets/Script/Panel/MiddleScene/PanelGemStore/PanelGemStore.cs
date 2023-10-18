using UnityEngine;

namespace MiddleScene
{
    public class PanelGemStore : IPanelGemStore
    {
        public PanelGemStore(IPanel parent) : base(parent)
        {
            Debug.Log(parent);
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        }
        protected override void OnInit()
        {
            base.OnInit();
        }
        protected override void OnEnter()
        {
            base.OnEnter();

        }
        protected override void OnUpdate()
        {
            base.OnUpdate();

        }
    }
}

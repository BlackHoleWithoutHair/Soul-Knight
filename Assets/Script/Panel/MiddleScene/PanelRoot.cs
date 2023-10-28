using TMPro;
using UnityEngine;

namespace MiddleScene
{
    public class PanelRoot : IPanel
    {
        private TextMeshProUGUI TextGem;
        public PanelRoot() : base(null)
        {
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
            children.Add(new PanelRoom(this));
        }
        protected override void OnInit()
        {
            base.OnInit();
            OnResume();
            TextGem = UnityTool.Instance.GetComponentFromChild<TextMeshProUGUI>(m_GameObject, "TextGem");
            EventCenter.Instance.RegisterObserver(EventType.OnFinishSelectPlayer, () =>
            {
                m_GameObject.SetActive(false);
            });
        }
        protected override void OnEnter()
        {
            base.OnEnter();
            EnterPanel(typeof(PanelRoom));
            OnResume();
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            TextGem.text = ArchiveCommand.Instance.GetMaterialNum(MaterialType.Gem).ToString();
            if (!GameMediator.Instance.isFinishSelecctPlayer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.CircleCast(mousePosition, 0.1f, Vector2.zero, 0, LayerMask.GetMask("Player"));
                    if (hit.collider != null && hit.collider.tag == "Player")
                    {
                        EventCenter.Instance.NotisfyObserver(EventType.OnPlayerClick, hit.collider.gameObject);
                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.CircleCast(mousePosition, 0.1f, Vector2.zero, 0, LayerMask.GetMask("Pet"));
                    if (hit.collider != null && hit.collider.tag == "Pet")
                    {
                        EventCenter.Instance.NotisfyObserver(EventType.OnPetClick);
                    }
                }
            }
        }
    }
}


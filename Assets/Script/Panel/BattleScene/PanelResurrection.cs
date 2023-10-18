using UnityEngine.UI;

public class PanelResurrection : IPanel
{
    private Button ButtonHome;
    private Button ButtonResurrection;
    private IPlayer player;
    public PanelResurrection(IPanel panel) : base(panel)
    {
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
    }

    protected override void OnInit()
    {
        base.OnInit();
        ButtonHome = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonHome");
        ButtonResurrection = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonResurrection");
        ButtonHome.onClick.AddListener(() =>
        {
            SceneModelCommand.Instance.LoadScene(SceneName.MiddleScene);
        });
        ButtonResurrection.onClick.AddListener(() =>
        {
            GameMediator.Instance.GetController<PlayerController>().Player.Resurrection();
            OnExit();
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
    }
}

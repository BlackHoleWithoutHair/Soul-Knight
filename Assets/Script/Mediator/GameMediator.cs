public class GameMediator : AbstractMediator
{
    private static GameMediator instance;
    public static GameMediator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameMediator();
            }
            return instance;
        }
    }
    public bool isFinishSelecctPlayer { get; private set; }
    protected GameMediator()
    {
        EventCenter.Instance.RegisterObserver(EventType.OnSceneChangeComplete, () =>
        {
            controllers.Clear();
            systems.Clear();
        });
        EventCenter.Instance.RegisterObserver(EventType.OnFinishSelectPlayer, () =>
        {
            isFinishSelecctPlayer = true;
        });
    }
}
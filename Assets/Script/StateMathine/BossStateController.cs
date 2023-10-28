public class BossStateController : IStateController
{
    public IBoss m_Boss { get; private set; }
    public BossStateController(IBoss boss) : base()
    {
        m_Boss = boss;
    }
}

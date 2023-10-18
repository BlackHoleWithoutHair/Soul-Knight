public class PlayerStateController : IStateController
{
    protected IPlayer m_Character;
    public PlayerStateController(IPlayer player) : base()
    {
        m_Character = player;

    }
    public IPlayer GetPlayer()
    {
        return m_Character;
    }
}

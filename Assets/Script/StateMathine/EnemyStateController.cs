public class EnemyStateController : IStateController
{
    protected IEnemy m_Character;
    public EnemyStateController(IEnemy enemy) : base()
    {
        m_Character = enemy;
    }
    public IEnemy GetCharacter()
    {
        return m_Character;
    }
}

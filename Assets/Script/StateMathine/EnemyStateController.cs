public class EnemyStateController : IStateController
{
    protected IEmployeeEnemy m_Character;
    public EnemyStateController(IEmployeeEnemy enemy) : base()
    {
        m_Character = enemy;
    }
    public IEmployeeEnemy GetCharacter()
    {
        return m_Character;
    }
}

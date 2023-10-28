public class PetStateController : IStateController
{
    protected IPlayerPet m_Pet;
    public ICharacter target;
    public PetStateController(IPlayerPet pet)
    {
        m_Pet = pet;
        target = m_Pet.GetPlayer();
    }
    public IPlayerPet GetPet()
    {
        return m_Pet;
    }
}

using UnityEngine;

public class PetState : IState
{
    protected new PetStateController m_Controller { get => base.m_Controller as PetStateController; set => base.m_Controller = value; }
    protected Rigidbody2D m_rb;
    protected IPlayerPet pet;
    protected IPlayer player;
    protected Animator m_Animator;
    public PetState(PetStateController controller) : base(controller)
    {
        pet = m_Controller.GetPet();
        player = pet.GetPlayer();
        m_Animator = pet.gameObject.GetComponent<Animator>();
        m_rb = pet.transform.GetComponent<Rigidbody2D>();
    }
    protected float GetDistanceToTarget(GameObject target)
    {
        return Vector2.Distance(target.gameObject.transform.position, pet.gameObject.transform.position);
    }
    protected Vector2 GetDirToTarget(GameObject target)
    {
        return (target.transform.position - pet.transform.position).normalized;
    }
    protected IEnemy GetCloseEnemy()
    {
        return GameMediator.Instance.GetController<PlayerController>().GetCloseEnemy(pet.gameObject);
    }
}

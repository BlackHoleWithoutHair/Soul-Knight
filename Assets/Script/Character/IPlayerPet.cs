using UnityEngine;

public class IPlayerPet : ICharacter
{
    protected IPlayer m_Player;
    protected PetStateController m_StateController;
    public IPlayerPet(GameObject obj, IPlayer player) : base(obj)
    {
        m_Player = player;
        gameObject.SetActive(false);
        m_Animator = transform.GetComponent<Animator>();
        m_rb = transform.GetComponent<Rigidbody2D>();
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController = new PetStateController(this);
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        m_StateController?.GameUpdate();
    }
    public IPlayer GetPlayer()
    {
        return m_Player;
    }
    public void EnterBattleScene()
    {
        EffectFactory.Instance.GetEffect(EffectType.AppearLight, gameObject.transform.position).AddToController();
        gameObject.SetActive(true);
    }
}

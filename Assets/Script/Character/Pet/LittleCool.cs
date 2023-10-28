using UnityEngine;

public class LittleCool : IPlayerPet
{
    public LittleCool(GameObject obj, IPlayer player) : base(obj, player)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerAttr(PlayerType.Knight);
        m_Attr.isRun = true;
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_Attr.CurrentHp = m_Attr.m_ShareAttr.MaxHp;
        m_StateController.SetOtherState(typeof(PetIdleState));
    }
}

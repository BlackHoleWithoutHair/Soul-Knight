using UnityEngine;

public class Werewolf : IPlayer
{
    public Werewolf(GameObject obj, PlayerAttribute attr) : base(obj, attr)
    {

    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(WerewolfIdleState));

    }
}

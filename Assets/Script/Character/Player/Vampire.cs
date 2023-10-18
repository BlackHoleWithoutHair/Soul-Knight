using UnityEngine;

public class Vampire : IPlayer
{
    public Vampire(GameObject obj, PlayerAttribute attr) : base(obj, attr)
    {

    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(VampireIdleState));

    }

}

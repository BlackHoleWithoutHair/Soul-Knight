using UnityEngine;

public class IPlayerLaserWeapon : IPlayerUnAccumulateWeapon
{

    public IPlayerLaserWeapon(GameObject obj, ICharacter character) : base(obj, character)
    {

    }
    protected override void OnEnter()
    {
        base.OnEnter();

    }
    public override void OnUpdate()
    {
        base.OnUpdate();

    }
}

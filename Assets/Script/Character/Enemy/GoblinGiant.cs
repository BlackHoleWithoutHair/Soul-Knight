using UnityEngine;

public class GoblinGiant : IEmployeeEnemy
{
    public GoblinGiant(GameObject obj) : base(obj) { }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(GoblinGiantIdleState));
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
    }
    protected override void OnCharacterDieStart()
    {
        base.OnCharacterDieStart();
    }
}

using UnityEngine;

public class GoblinGuard : IEmployeeEnemy
{
    public GoblinGuard(GameObject obj) : base(obj) { }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        if (m_Weapon.m_Attr.Type != EnemyWeaponType.Pike)
        {
            m_StateController.SetOtherState(typeof(GoblinGuardIdleState));
        }
        else
        {
            m_StateController.SetOtherState(typeof(GoblinGuardMeleeIdleState));
        }
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

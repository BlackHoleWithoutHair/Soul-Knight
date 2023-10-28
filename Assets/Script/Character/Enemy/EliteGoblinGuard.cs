using UnityEngine;

public class EliteGoblinGuard : IEmployeeEnemy
{
    public EliteGoblinGuard(GameObject obj) : base(obj) { }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();

        if (m_Weapon.m_Attr.Type != EnemyWeaponType.Hoe)
        {
            m_StateController.SetOtherState(typeof(EliteGoblinGuardIdleState));
        }
        else
        {
            m_StateController.SetOtherState(typeof(EliteGoblinGuardMeleeIdleState));
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

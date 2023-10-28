using UnityEngine;

public class IPlayerUnAccumulateWeapon : IPlayerWeapon
{
    public IPlayerUnAccumulateWeapon(GameObject obj, ICharacter character) : base(obj, character) { }
    protected override void OnEnter()
    {
        base.OnEnter();
        Cumulate = 1f / m_Attr.FireRate;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        Cumulate += Time.deltaTime;
        if (isAttackKeyDown)
        {
            if (Cumulate > 1f / m_Attr.FireRate)
            {
                if ((m_Character.m_Attr as PlayerAttribute).CurrentMp - m_Attr.MagicSpend >= 0)
                {
                    OnFire();
                    Cumulate = 0;
                }
            }
        }
    }
}

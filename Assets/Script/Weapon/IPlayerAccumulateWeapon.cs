using UnityEngine;

public class IPlayerAccumulateWeapon : IPlayerWeapon
{
    private bool isKeyDown;
    public IPlayerAccumulateWeapon(GameObject obj, ICharacter player) : base(obj, player) { }
    protected override void OnEnter()
    {
        base.OnEnter();
        Cumulate = 0;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isAttackKeyDown)
        {
            Cumulate += Time.deltaTime;
            if (!isKeyDown)
            {
                isKeyDown = true;
                OnAttackKeyDownStart();
            }
        }
        if (isKeyDown && !isAttackKeyDown)
        {
            isKeyDown = false;
            if ((m_Character.m_Attr as PlayerAttribute).CurrentMp - m_Attr.MagicSpend >= 0)
            {
                OnFire();
                Cumulate = 0;
            }
        }
    }

    protected virtual void OnAttackKeyDownStart() { }
}

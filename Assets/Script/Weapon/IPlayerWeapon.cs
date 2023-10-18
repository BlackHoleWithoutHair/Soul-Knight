using UnityEngine;
public class IPlayerWeapon : IWeapon
{
    public PlayerWeaponShareAttribute m_Attr;
    protected BindableProperty<bool> isAttackKeyDown = new BindableProperty<bool>(false);
    protected Vector2 aimDir;
    public bool isBeingUsed;
    public IPlayerWeapon(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_GameObject = obj;
        m_Character = character;
        isAttackKeyDown.Register((val) =>
        {
            if (val == true)
            {
                (m_Character.m_Attr as PlayerAttribute).SpeedDecreaseFac += m_Attr.SpeedDecrease;
            }
            else
            {
                (m_Character.m_Attr as PlayerAttribute).SpeedDecreaseFac -= m_Attr.SpeedDecrease;
            }
        });
    }
    protected override void OnInit()
    {
        base.OnInit();
        m_Attr.CriticalRate += (m_Character.m_Attr as PlayerAttribute).m_ShareAttr.Critical;
    }
    public override void OnExit()
    {
        base.OnExit();
        isAttackKeyDown.Value = false;
    }
    protected override void OnFire()
    {
        (m_Character.m_Attr as PlayerAttribute).CurrentMp -= m_Attr.MagicSpend;
    }
    protected virtual Quaternion GetShotRot()
    {
        return Quaternion.Euler(0, 0, Random.Range(-m_Attr.ScatteringRate, m_Attr.ScatteringRate)) * Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, aimDir));
    }
    public void ControlWeapon(bool isAttackKeyDown, Vector2 aimDir)
    {
        this.isAttackKeyDown.Value = isAttackKeyDown;
        this.aimDir = aimDir;
    }
}

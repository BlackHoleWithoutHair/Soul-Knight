using UnityEngine;

public class FireOnAllCylinders : ISkill
{
    private GameObject GunOriginPoint;
    private IWeapon weapon;
    private float Timer;
    public FireOnAllCylinders(IPlayer character) : base(character)
    {
        GunOriginPoint = UnityTool.Instance.GetGameObjectFromChildren(m_Player.gameObject, "GunOriginPoint2");
        m_Attr = AttributeFactory.Instance.GetSkillAttr(SkillType.FireOnAllCylinders, m_Player.m_Attr);
    }
    protected override void OnSkillStart()
    {
        base.OnSkillStart();
        Timer = 0;
        weapon = WeaponFactory.Instance.GetPlayerWeapon(m_Player.GetUsedWeapon().m_Attr.Type, m_Player, GunOriginPoint.transform, "Floor", 10);
    }
    protected override void OnSkillDuration()
    {
        base.OnSkillDuration();
        Timer += Time.deltaTime;
        weapon.OnUpdate();
        if (Timer > 5)
        {
            StopSkill();
        }
    }
    protected override void OnFinishSkill()
    {
        base.OnFinishSkill();
        Object.Destroy(weapon.gameObject);
    }
}

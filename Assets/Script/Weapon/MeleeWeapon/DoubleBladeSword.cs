using UnityEngine;

public class DoubleBladeSword : IPlayerUnAccumulateWeapon
{
    private Animator m_Animator;
    public DoubleBladeSword(GameObject obj, ICharacter character) : base(obj, character)
    {   
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.DoubleBladeSword);
    }
    protected override void OnInit()
    {
        base.OnInit();
        m_Animator = gameObject.GetComponent<Animator>();
    }
    protected override void OnFire()
    {
        base.OnFire();
        m_Animator.enabled = true;
        m_Animator.Play("Attack", 0, 0);
        IKnifeLight light= ItemPool.Instance.GetPlayerKnifeLight(KnifeLightType.KnifeLight, m_Attr, FirePoint.transform.position, FirePoint.transform.rotation);
        light.transform.localScale= Vector3.one*1.5f;
        light.AddToController();
    }
}

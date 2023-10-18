using UnityEngine;

public class DoubleBladeSword : IPlayerUnAccumulateWeapon
{
    private Animator m_Animator;
    private AnimatorStateInfo info;
    public DoubleBladeSword(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.DoubleBladeSword);
    }
    protected override void OnInit()
    {
        base.OnInit();
        m_Animator = m_GameObject.GetComponent<Animator>();
    }
    protected override void OnFire()
    {
        base.OnFire();
        m_Animator.enabled = true;
        m_Animator.Play("Attack", 0, 0);
        ItemPool.Instance.GetPlayerKnifeLight(KnifeLightType.KnifeLight, m_Attr, FirePoint.transform.position, FirePoint.transform.rotation).AddToController();
    }
}

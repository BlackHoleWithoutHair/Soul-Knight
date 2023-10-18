using System.Collections;
using UnityEngine;

public class WoodenCross : IPlayerUnAccumulateWeapon
{
    private Animator m_Animator;
    public WoodenCross(GameObject obj, ICharacter characeter) : base(obj, characeter)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.WoodenCross);
        CanBeRotated = false;
    }
    protected override void OnInit()
    {
        base.OnInit();
        m_Animator = m_GameObject.GetComponent<Animator>();
        RotOrigin.transform.Find(m_Attr.Type.ToString()).rotation = Quaternion.Euler(0, 0, 90);
    }
    protected override void OnFire()
    {
        base.OnFire();
        m_Animator.enabled = true;
        m_Animator.Play("Attack", 0, 0);
        CoroutinePool.Instance.StartCoroutine(WaitForFire());
    }
    private IEnumerator WaitForFire()
    {
        yield return new WaitForSeconds(0.17f);
        for (int i = 0; i < 2; i++)
        {
            ItemPool.Instance.GetPlayerBullet(PlayerBulletType.Beam, m_Attr, Vector2.zero, Quaternion.identity).AddToController();
        }
    }
}

using UnityEngine;

public class PlayerBow : IPlayerAccumulateWeapon
{
    private IBullet arrow;
    private SpriteRenderer firstSprite;
    private SpriteRenderer nextSprite;

    public PlayerBow(GameObject obj, ICharacter player) : base(obj, player)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.Bow);
        firstSprite = UnityTool.Instance.GetComponentFromChild<SpriteRenderer>(gameObject, m_Attr.Type.ToString());
        nextSprite = firstSprite.transform.GetChild(0).GetComponent<SpriteRenderer>();
        firstSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
        CanBeRotated = false;
    }

    protected override void OnAttackKeyDownStart()//攻击键按下执行一次
    {
        base.OnAttackKeyDownStart();
        CanBeRotated = true;
        firstSprite.enabled = false;
        nextSprite.enabled = true;
        firstSprite.transform.localRotation = Quaternion.Euler(0, 0, 45);
        arrow = CreateBullet(PlayerBulletType.Arrow_1, m_Attr);
        arrow.gameObject.transform.SetParent(FirePoint.transform);
        arrow.gameObject.transform.localPosition = Vector3.zero;
        arrow.gameObject.transform.localRotation = Quaternion.identity;
    }
    protected override void OnFire()//攻击键松开执行一次
    {
        base.OnFire();
        CanBeRotated = false;
        RotOrigin.transform.localRotation = Quaternion.identity;
        firstSprite.enabled = true;
        nextSprite.enabled = false;
        firstSprite.transform.localRotation = Quaternion.identity;
        arrow.gameObject.transform.SetParent(null);
        arrow.SetRotation(GetShotRot());
        arrow.SetPosition(FirePoint.transform.position);
        arrow.AddToController();
        arrow = null;
    }
}

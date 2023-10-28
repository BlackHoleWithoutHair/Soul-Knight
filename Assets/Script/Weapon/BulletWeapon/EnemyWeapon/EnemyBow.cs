using UnityEngine;

public class EnemyBow : IEnemyWeapon
{
    private IBullet arrow;
    private SpriteRenderer firstSprite;
    private SpriteRenderer nextSprite;
    public EnemyBow(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyWeaponAttr(EnemyWeaponType.Bow);
    }
    protected override void OnInit()
    {
        base.OnInit();
        firstSprite = UnityTool.Instance.GetComponentFromChild<SpriteRenderer>(gameObject, m_Attr.Type.ToString());
        nextSprite = firstSprite.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    protected override void OnBeforeFireStart()
    {
        base.OnBeforeFireStart();
        firstSprite.enabled = false;
        nextSprite.enabled = true;
        arrow = EffectFactory.Instance.GetEnemyBullet(EnemyBulletType.EnemyRedArrow, m_Attr, m_Character.m_Attr.GetShareAttr() as EnemyShareAttr, FirePoint.transform.position, GetShotRotation());
        arrow.gameObject.transform.SetParent(FirePoint.transform);
        arrow.gameObject.transform.localPosition = Vector3.zero;
        arrow.gameObject.transform.localRotation = Quaternion.identity;
    }
    protected override void OnFire()
    {
        base.OnFire();
        firstSprite.enabled = true;
        nextSprite.enabled = false;
        arrow.gameObject.transform.SetParent(null);
        arrow.SetRotation(GetShotRotation());
        arrow.SetPosition(arrow.gameObject.transform.position);
        arrow.AddToController();
    }
}

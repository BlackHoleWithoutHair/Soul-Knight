using UnityEngine;

public class Icebreaker : IPlayerLaserWeapon
{
    public Icebreaker(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.Icebreaker);
    }
    protected override void OnFire()
    {
        base.OnFire();
        EffectFactory.Instance.GetLaser(LaserType.BlueLaser, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
    }
}

using UnityEngine;

public class Furnace : IPlayerUnAccumulateWeapon
{
    public Furnace(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.Furnace);
    }
    protected override void OnFire()
    {
        base.OnFire();
        PlayRecoilAnim();
        ShowFireSpark(BulletColorType.Red);
        CreateBullet(PlayerBulletType.FireBullet, m_Attr).AddToController();
    }
}

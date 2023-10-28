using UnityEngine;

public class DormantBubbleMachine : IPlayerUnAccumulateWeapon
{
    public DormantBubbleMachine(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.DormantBubbleMachine);
    }
    protected override void OnFire()
    {
        base.OnFire();
        PlayRecoilAnim();
        ShowFireSpark(BulletColorType.Green);
        for (int i = -2; i <= 2; i++)
        {
            CreateBullet(PlayerBulletType.Bullet_11, m_Attr,i).AddToController();
        }
    }
}

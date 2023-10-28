using UnityEngine;

public class PKP : IPlayerUnAccumulateWeapon
{
    // ���8���ӵ����������ӵ�10��ÿ�룬ƫ�ƽ�Ϊ5
    //15�����˺����ӵ�4
    public PKP(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.PKP);
    }
    protected override void OnFire()
    {
        base.OnFire();
        PlayRecoilAnim();
        ShowFireSpark(BulletColorType.Yellow);
        CreateBullet(PlayerBulletType.Bullet_1, m_Attr).AddToController();
    }
}

using System.Collections.Generic;
using System.Linq;

public class WeaponModel:AbstractModel
{
    public List<PlayerWeaponShareAttribute> playerWeaponModel;
    public List<EnemyWeaponShareAttribute> enemyWeaponModel;
    protected override void OnInit()
    {
        base.OnInit();
        playerWeaponModel = ProxyResourceFactory.Instance.Factory.GetScriptableObject<WeaponScriptableObject>().WeaponShareAttrs;
        enemyWeaponModel = ProxyResourceFactory.Instance.Factory.GetScriptableObject<EnemyWeaponScriptableObject>().EnemyWeaponShareAttrs;
    }
}

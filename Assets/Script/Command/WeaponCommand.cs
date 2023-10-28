using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeaponCommand:Singleton<WeaponCommand>
{
    private WeaponModel model;
    private WeaponCommand()
    {
        model = ModelContainer.Instance.GetModel<WeaponModel>();
    }
    public PlayerWeaponShareAttribute GetPlayerWeaponShareAttr(PlayerWeaponType type)
    {
        PlayerWeaponShareAttribute[] a = model.playerWeaponModel.Where(attr => attr.Type == type).ToArray();
        if(a.Length==0)
        {
            return null;
        }
        else
        {
            return a[0];
        }

    }
    public EnemyWeaponShareAttribute GetEnemyWeaponShareAttr(EnemyWeaponType type)
    {
        return model.enemyWeaponModel.Where(attr => attr.Type == type).ToArray()[0];
    }
}

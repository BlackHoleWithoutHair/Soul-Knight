using System.Linq;
using UnityEngine;

public class EnemyCommand : Singleton<EnemyCommand>
{
    private EnemyModel model;
    private EnemyCommand()
    {
        model = ModelContainer.Instance.GetModel<EnemyModel>();
    }
    public EnemyShareAttr GetEnemyShareAttr(EnemyType type, bool isElite, EnemyWeaponType weaponType)
    {
        EnemyShareAttr attr = model.attrs.Where(x => x.Type == type && x.isElite == isElite && x.WeaponType == weaponType).ToArray()[0];
        if (attr == null)
        {
            Debug.Log("EnemyCommand GetEnemyShareAttr " + type + " return null");
        }
        return attr;
    }
    public bool ContainState(EnemyType type, bool isElite)
    {
        return model.attrs.Any(x => x.Type == type && x.isElite == isElite);
    }
}

using System.Linq;
using UnityEngine;

public class BossCommand : Singleton<BossCommand>
{
    private BossModel model;
    private BossCommand()
    {
        model = ModelContainer.Instance.GetModel<BossModel>();
    }
    public BossShareAttr GetBossShareAttr(BossType type, BossCategory category)
    {
        BossShareAttr result = model.attrs.Where(attr => attr.BossType == type && attr.BossCategory == category).ToArray()[0];
        if (result == null)
        {
            Debug.Log("GetBossShareAttr " + type + " return null");
        }
        return result;
    }
}

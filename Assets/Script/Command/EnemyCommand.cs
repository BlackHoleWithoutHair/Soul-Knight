using UnityEngine;

public class EnemyCommand : Singleton<EnemyCommand>
{
    private EnemyModel model;
    private EnemyCommand()
    {
        model = ModelContainer.Instance.GetModel<EnemyModel>();
    }
    public EnemyShareAttr GetEnemyShareAttr(EnemyType type)
    {
        foreach (EnemyShareAttr attr in model.attrs)
        {
            if (attr.Type == type)
            {
                return attr;
            }
        }
        Debug.Log("EnemyCommand GetEnemyShareAttr " + type + " return null");
        return null;
    }
}

using System.Collections.Generic;

public class EnemyModel : AbstractModel
{
    public List<EnemyShareAttr> attrs;
    protected override void OnInit()
    {
        attrs = ProxyResourceFactory.Instance.Factory.GetScriptableObject<EnemyScriptableObject>().EnemyShareAttrs;
    }
}

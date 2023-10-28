using System.Collections.Generic;

public class BossModel : AbstractModel
{
    public List<BossShareAttr> attrs;
    protected override void OnInit()
    {
        base.OnInit();
        attrs = ProxyResourceFactory.Instance.Factory.GetScriptableObject<BossScriptableObject>().attrs;
    }
}

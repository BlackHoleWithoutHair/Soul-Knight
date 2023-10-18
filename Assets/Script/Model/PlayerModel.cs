using System.Collections.Generic;


public class PlayerModel : AbstractModel
{
    public List<PlayerShareAttr> attrs;
    public PlayerModel() { }
    protected override void OnInit()
    {
        attrs = ProxyResourceFactory.Instance.Factory.GetScriptableObject<PlayerScriptableObject>().PlayerShareAttrs;
    }
}

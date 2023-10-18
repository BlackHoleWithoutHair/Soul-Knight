using System.Collections.Generic;

public class PlantModel : AbstractModel
{
    public List<PlantAttr> m_PlantAttrs;
    public PlantModel()
    {
        m_PlantAttrs = ProxyResourceFactory.Instance.Factory.GetScriptableObject<PlantScriptableObject>().plantInfos;
    }
}

using UnityEngine;
public class ItemFactory : Singleton<ItemFactory>
{
    private ItemFactory() { }
    public GameObject GetTreasureBox(TreasureBoxType type, Vector2 pos)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetTreasureBox(type));
        obj.transform.position = pos;
        return obj;
    }
    public ICoin GetCoin(CoinType type, Vector2 pos)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetItem(type.ToString()));
        ICoin coin = null;
        switch (type)
        {
            case CoinType.Coppers:
                coin = new Coppers(obj);
                break;
            case CoinType.SilverCoin:
                coin = new SilverCoin(obj);
                break;
            case CoinType.GoldCoin:
                coin = new GoldCoin(obj);
                break;

        }
        coin?.SetPosition(pos);
        return coin;
    }
    public Item GetItem(ItemType type, Vector2 pos)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetItem(type.ToString()));
        Item effect = null;
        switch (type)
        {
            case ItemType.EnergyBall:
                effect = new EnergyBall(obj);
                effect.SetPosition(pos);
                break;
        }
        return effect;
    }
    public WorldMaterial GetMaterial(MaterialType type, Vector2 pos)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetItem("MaterialTemplate"));
        obj.GetComponent<SpriteRenderer>().sprite = ProxyResourceFactory.Instance.Factory.GetMaterialSprite(type.ToString());
        WorldMaterial material = new WorldMaterial(type, obj);
        material.SetPosition(pos);
        return material;
    }
    public WorldSeed GetSeed(SeedType type, Vector2 pos)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetItem("SeedTemplate"));
        WorldSeed seed = new WorldSeed(type, obj);
        seed.SetPosition(pos);
        return seed;
    }
    public WorldMaterial GetMaterial(MaterialType type, GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().sprite = ProxyResourceFactory.Instance.Factory.GetMaterialSprite(type.ToString());
        WorldMaterial material = new WorldMaterial(type, obj);
        material.SetPosition(obj.transform.position);
        return material;
    }
    public IPlant GetPlant(GameObject parent, Garden garden)
    {
        GameObject obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetItem("PlantTemplate"));
        obj.GetComponent<SpriteRenderer>().sprite = ProxyResourceFactory.Instance.Factory.GetPlantSprite("Seed");
        obj.transform.SetParent(parent.transform);
        obj.transform.localPosition = Vector3.zero;
        return new IPlant(obj, garden);
    }
}

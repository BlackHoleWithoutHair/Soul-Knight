using UnityEngine;

public class GoldCoin : ICoin
{
    public GoldCoin(GameObject obj) : base(obj) { }
    protected override void OnHitPlayer()
    {
        base.OnHitPlayer();
        MemoryModelCommand.Instance.AddMoney(5);
    }
}

using UnityEngine;

public class SilverCoin : ICoin
{
    public SilverCoin(GameObject obj) : base(obj) { }
    protected override void OnHitPlayer()
    {
        base.OnHitPlayer();
        MemoryModelCommand.Instance.AddMoney(3);
    }
}

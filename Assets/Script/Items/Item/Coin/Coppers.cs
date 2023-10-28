using UnityEngine;

public class Coppers : ICoin
{
    public Coppers(GameObject obj) : base(obj) { }
    protected override void OnHitPlayer()
    {
        base.OnHitPlayer();
        MemoryModelCommand.Instance.AddMoney(1);
    }
}

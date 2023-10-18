using UnityEngine;

public class EnergyBall : Item
{
    private IPlayer player;
    public EnergyBall(GameObject obj) : base(obj)
    {
        player = GameMediator.Instance.GetController<PlayerController>().Player;
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, gameObject, player.gameObject.transform.Find("BulletCheckBox").gameObject, (obj) =>
        {
            player.AddMagicPower(2);
            Remove();
        });
    }
    protected override void Init()
    {
        base.Init();
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        gameObject.transform.position += 10 * (player.gameObject.transform.position - gameObject.transform.position).normalized * Time.deltaTime;
    }
}

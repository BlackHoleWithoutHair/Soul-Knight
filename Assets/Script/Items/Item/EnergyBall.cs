using UnityEngine;

public class EnergyBall : Item
{
    private IPlayer player;
    private Vector2 AppearPos;
    private Collider2D collider;
    private bool isFollowPlayer;
    public EnergyBall(GameObject obj) : base(obj)
    {
        player = GameMediator.Instance.GetController<PlayerController>().Player;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        AppearPos = transform.position;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!isFollowPlayer)
        {
            if (Vector2.Distance(transform.position, AppearPos) < 1)
            {
                gameObject.transform.position += 5 * Vector3.up * Time.deltaTime;
            }
            else
            {
                isFollowPlayer = true;
            }
        }
        else
        {
            gameObject.transform.position += 10 * (player.gameObject.transform.position - gameObject.transform.position).normalized * Time.deltaTime;
            collider = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Player"));
            if (collider != null && collider.GetComponent<Symbol>())
            {
                if (collider.GetComponent<Symbol>().GetCharacter() == player)
                {
                    player.AddMagicPower(2);
                    Remove();
                }
            }
        }
    }
}

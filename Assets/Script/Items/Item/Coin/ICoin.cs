using UnityEngine;

public class ICoin : Item
{
    private bool isFindPlayer;
    private bool isFollowPlayer;
    private Vector2 originPos;
    private Collider2D hit;
    private IPlayer player;
    public ICoin(GameObject obj) : base(obj) { }
    protected override void Init()
    {
        base.Init();
        isFindPlayer = false;
        player = GameMediator.Instance.GetController<PlayerController>().Player;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        isFollowPlayer = false;
        originPos = transform.position;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!isFindPlayer)
        {
            hit = Physics2D.OverlapCircle(gameObject.transform.position, 4f, LayerMask.GetMask("Player"));
            if (hit && hit.GetComponent<Symbol>().GetCharacter() == player)
            {
                isFindPlayer = true;
            }
        }
        if (isFindPlayer)
        {
            if (!isFollowPlayer)
            {
                if (Vector2.Distance(transform.position, originPos) < 1)
                {
                    gameObject.transform.position += 5f * Vector3.up * Time.deltaTime;
                }
                else
                {
                    isFollowPlayer = true;
                }
            }
            else
            {
                gameObject.transform.position += 10f * (player.transform.position - gameObject.transform.position).normalized * Time.deltaTime;
                hit = Physics2D.OverlapCircle(gameObject.transform.position, 0.5f, LayerMask.GetMask("Player"));
                if (hit != null && hit.GetComponent<Symbol>())
                {
                    if (hit.GetComponent<Symbol>().GetCharacter() == player)
                    {
                        OnHitPlayer();
                        Remove();
                    }
                }

            }
        }
    }
    protected virtual void OnHitPlayer() { }
}

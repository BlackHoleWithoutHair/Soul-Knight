using UnityEngine;

public class ICoin : Item
{
    private bool isFindPlayer;
    private Collider2D hit;
    public ICoin(GameObject obj) : base( obj)
    {

    }
    protected override void Init()
    {
        base.Init();
        isFindPlayer = false;
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        if (!isFindPlayer)
        {
            hit = Physics2D.OverlapCircle(gameObject.transform.position, 3.5f, LayerMask.GetMask("Player"));
            if (hit)
            {
                isFindPlayer = true;
            }
        }
        if (isFindPlayer)
        {
            gameObject.transform.position += 10f * (hit.transform.position - gameObject.transform.position).normalized * Time.deltaTime;
            if (Physics2D.OverlapCircle(gameObject.transform.position, 0.5f, LayerMask.GetMask("Player")))
            {
                OnHitPlayer();
                Remove();
            }
        }
    }
    protected virtual void OnHitPlayer() { }
}

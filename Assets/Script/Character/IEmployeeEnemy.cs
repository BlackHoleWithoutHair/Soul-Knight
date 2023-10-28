
using System.Collections;
using UnityEngine;

public class IEmployeeEnemy : IEnemy
{
    public new EnemyAttribute m_Attr { get => base.m_Attr as EnemyAttribute; protected set => base.m_Attr = value; }
    protected EnemyStateController m_StateController;
    public IEmployeeEnemy(GameObject obj) : base(obj)
    {
        m_StateController = new EnemyStateController(this);
    }
    protected override void OnInit()
    {
        base.OnInit();
        if (m_Attr.m_ShareAttr.isElite)
        {
            transform.localScale = Vector3.one * 1.5f;
        }
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        m_StateController.GameUpdate();
    }
    protected override void OnCharacterDieStart()
    {
        base.OnCharacterDieStart();
        m_StateController.StopCurrentState();
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
        if (gameObject.transform.Find("Collider"))
        {
            gameObject.transform.Find("Collider").GetComponent<CapsuleCollider2D>().isTrigger = true;
        }
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            ItemFactory.Instance.GetItem(ItemType.EnergyBall, (Vector2)gameObject.transform.position + Random.insideUnitCircle * 2).AddToController();
        }
        if (Random.Range(0, 2) == 0)
        {
            ItemFactory.Instance.GetCoin(CoinType.Coppers, (Vector2)gameObject.transform.position + Random.insideUnitCircle * 2).AddToController();
        }
    }
    public override void UnderAttack(int damage)
    {
        base.UnderAttack(damage);
        HitEffect();
    }
    private void HitEffect()
    {
        gameObject.GetComponent<SpriteRenderer>().GetPropertyBlock(block);
        block.SetColor("_Color", Color.white);
        gameObject.GetComponent<SpriteRenderer>().SetPropertyBlock(block);
        CoroutinePool.Instance.StartCoroutine(ResumeColor());
    }
    private IEnumerator ResumeColor()
    {
        yield return new WaitForSeconds(1f / 12f);
        gameObject.GetComponent<SpriteRenderer>().GetPropertyBlock(block);
        block.SetColor("_Color", Color.black);
        gameObject.GetComponent<SpriteRenderer>().SetPropertyBlock(block);
    }
}

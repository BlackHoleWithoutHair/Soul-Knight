
using UnityEngine;

public class WhiteTreasureBox : ITreasureBox
{
    private GameObject ballPoint;
    private AnimatorStateInfo info;
    private bool isCreateItem;
    protected override void Start()
    {
        base.Start();
        ballPoint = transform.Find("WeaponCreatePoint").gameObject;
    }
    protected override void Update()
    {
        if (!isFinish)
        {
            if (isPlayerEnter && !isCreateItem)
            {
                isCreateItem = true;
                m_Animator.enabled = true;
            }
            info = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime > 1 && !isFinish)
            {
                isFinish = true;
                for (int i = 0; i < 2; i++)
                {
                    ItemFactory.Instance.GetItem(ItemType.EnergyBall, (Vector2)ballPoint.transform.position + Random.insideUnitCircle).AddToController();
                }
                ItemFactory.Instance.GetCoin(CoinType.Coppers, (Vector2)ballPoint.transform.position + Random.insideUnitCircle * 2).AddToController();
            }
        }
    }
}

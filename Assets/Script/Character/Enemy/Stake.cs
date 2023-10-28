using TMPro;
using UnityEngine;

public class Stake : IEmployeeEnemy
{
    private float DamageAccumulateTimer;
    protected TextMeshProUGUI m_TextTotleDamage;
    public Stake(GameObject obj) : base(obj) { }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        DamageAccumulateTimer += Time.deltaTime;
    }
    protected override void OnInit()
    {
        base.OnInit();
        m_TextTotleDamage = gameObject.transform.Find("TextTotleDamage")?.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        DamageAccumulateTimer = 2f;
        m_StateController.SetOtherState(typeof(StakeIdleState));
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        if (m_TextTotleDamage != null)
        {
            if (DamageAccumulateTimer < 2)
            {
                m_TextTotleDamage.gameObject.SetActive(true);
                m_TextTotleDamage.gameObject.transform.rotation = Quaternion.identity;
            }
            else
            {
                m_TextTotleDamage.gameObject.SetActive(false);
                m_TextTotleDamage.text = "0";
            }
        }
    }
    protected override void OnCharacterDieStart()
    {
        base.OnCharacterDieStart();
    }
    public override void UnderAttack(int damage)
    {
        base.UnderAttack(damage);
        DamageAccumulateTimer = 0;
        if (m_TextTotleDamage != null)
        {
            m_TextTotleDamage.text = (int.Parse(m_TextTotleDamage.text) + damage).ToString();
        }
        m_StateController.SetOtherState(typeof(StakeBeAttackState));
    }
}

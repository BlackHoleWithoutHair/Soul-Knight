using TMPro;
using UnityEngine;

public class CriticalDecHp : Item
{
    private Animator m_Animator;
    private TextMeshProUGUI m_Text;
    private AnimatorStateInfo info;
    private int Damage;
    public CriticalDecHp(GameObject obj, int damage) : base(obj)
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_Text = gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        Damage = damage;
    }
    protected override void Init()
    {
        base.Init();
        gameObject.transform.position = position;
        m_Text.text = Damage.ToString();
    }
    public override void GameUpdate()
    {
        base.GameUpdate();
        info = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime > 1)
        {
            Remove();
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}

using UnityEngine;

public class ISkill
{
    public SkillAttribute m_Attr;
    protected IPlayer m_Player;
    protected Animator m_Animator;
    public bool isSkillUpdate { get; private set; }
    private bool isSkillEverUse;//技能是否曾经使用过，防止Exit被意外执行
    private bool isInit;
    private bool isStart;
    private bool isFinishSkill;
    public ISkill(IPlayer character)
    {
        m_Player = character;
        m_Animator = UnityTool.Instance.GetComponentFromChild<Animator>(m_Player.gameObject, "Sprite");
    }
    protected virtual void OnInit() { }
    protected virtual void OnSkillStart()
    {
        if (!isInit)
        {
            isInit = true;
            OnInit();
        }
    }
    public virtual void OnUpdate()
    {
        if (isSkillUpdate)
        {
            if (!isStart)
            {
                isStart = true;
                OnSkillStart();
            }
            OnSkillDuration();
        }
        else if (!isFinishSkill && isSkillEverUse)
        {
            isFinishSkill = true;
            OnFinishSkill();
        }
    }
    protected virtual void OnSkillDuration() { }
    protected virtual void OnFinishSkill() { }
    public void StartSkill()
    {
        isSkillEverUse = true;
        isSkillUpdate = true;
        isFinishSkill = false;
        isStart = false;
    }
    public void StopSkill()
    {
        isSkillUpdate = false;
    }
}

public class ISkill
{

    protected ICharacter m_Character;

    public bool isSkillUpdate { get; private set; }
    private bool isInit;
    private bool isStart;
    private bool isFinishSkill;
    public ISkill(ICharacter character)
    {
        m_Character = character;
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
    }
    protected virtual void OnSkillDuration() { }
    protected virtual void OnSkillFinish() { }
    public void StartSkill()
    {
        isSkillUpdate = true;
        isFinishSkill = false;
        isStart = false;
    }
    public void StopSkill()
    {
        isSkillUpdate = false;
        if (!isFinishSkill)
        {
            isFinishSkill = true;
            OnSkillFinish();
        }
    }
}

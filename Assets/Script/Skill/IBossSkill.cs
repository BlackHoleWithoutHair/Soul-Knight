

public class IBossSkill : ISkill
{
    public IBossSkill(ICharacter character) : base(character) { }
    protected override void OnSkillFinish()
    {
        base.OnSkillFinish();
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
}

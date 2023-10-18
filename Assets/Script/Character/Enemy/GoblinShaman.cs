using UnityEngine;

public class GoblinShaman : IEnemy
{
    public GoblinShaman(GameObject obj) : base(obj)
    {
        m_Attr = AttributeFactory.Instance.GetEnemyAttr(EnemyType.GoblinShaman);
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(GoblinShamanIdleState));
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
    }
    protected override void OnCharacterDieStart()
    {
        base.OnCharacterDieStart();
    }
}

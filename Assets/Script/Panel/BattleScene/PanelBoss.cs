
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelBoss : IPanel
{
    private Slider SliderHp;
    private int DeadBossNum;
    private bool isClearBoss;
    public PanelBoss(IPanel parent) : base(parent) { }
    protected override void OnInit()
    {
        base.OnInit();
        SliderHp = UnityTool.Instance.GetComponentFromChild<Slider>(gameObject, "SliderHp");
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        SliderHp.value = GetSliderValue();
    }
    private float GetSliderValue()
    {
        List<IBoss> bosses = GameMediator.Instance.GetController<EnemyController>().bosses;
        float CurrentHp = 0;
        float MaxHp = 0;
        foreach (IBoss boss in bosses)
        {
            CurrentHp += boss.m_Attr.CurrentHp;
            MaxHp += boss.m_Attr.m_ShareAttr.MaxHp;
        }
        if (!isClearBoss)
        {
            DeadBossNum = 0;
            foreach (IBoss boss in bosses)
            {
                if (boss.IsDie)
                {
                    DeadBossNum += 1;
                }
            }
            if (DeadBossNum == bosses.Count)
            {
                isClearBoss = true;
                OnExit();
            }
        }
        return CurrentHp / MaxHp;
    }
}

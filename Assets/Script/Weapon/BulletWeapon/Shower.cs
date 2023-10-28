using System.Collections;
using UnityEngine;

public class Shower : IPlayerUnAccumulateWeapon
{
    private GameObject m_Garden;
    private Animator m_Animator;
    private AnimatorStateInfo info;
    private bool isStartWater;
    public Shower(GameObject obj, ICharacter character) : base(obj, character)
    {
        m_Attr = WeaponCommand.Instance.GetPlayerWeaponShareAttr(PlayerWeaponType.Shower);
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, m_Character.gameObject, (obj) =>
        {
            if (obj.name.Contains("Garden"))
            {
                if (ArchiveQuery.Instance.isHavingPlant(obj.GetComponent<Garden>().GardenInfo.index))
                {
                    m_Garden = obj;
                }
            }
        });
        TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerExit, m_Character.gameObject, (obj) =>
        {
            if (obj.name.Contains("Garden"))
            {
                m_Garden = null;
            }
        });

    }
    protected override void OnInit()
    {
        base.OnInit();
        m_Animator = UnityTool.Instance.GetComponentFromChild<Animator>(gameObject, "Shower");

    }
    protected override void OnFire()
    {
        base.OnFire();
        if (m_Garden == null)
        {
            IBullet bullet = CreateBullet(PlayerBulletType.Bullet_7, m_Attr);
            bullet.SetColor(BulletColorType.Cyan);
            bullet.AddToController();
        }
        else
        {
            EventCenter.Instance.NotisfyObserver(EventType.OnPause);
            m_Animator.SetBool("isWater", true);
            isStartWater = true;
            CoroutinePool.Instance.StartCoroutine(WaitForCloseAnim());
        }
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    private IEnumerator WaitForCloseAnim()
    {
        while (true)
        {
            if (isStartWater)
            {
                info = m_Animator.GetCurrentAnimatorStateInfo(0);
            }
            if (info.normalizedTime > 1 && isStartWater && info.IsName("Water"))
            {
                isStartWater = false;
                EventCenter.Instance.NotisfyObserver(EventType.OnResume);
                m_Animator.SetBool("isWater", false);
                m_Garden.GetComponent<Garden>().Watering();
                yield break;
            }
            yield return null;
        }
    }
}

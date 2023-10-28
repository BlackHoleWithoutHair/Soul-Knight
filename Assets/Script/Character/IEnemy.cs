using UnityEngine;

public class IEnemy : ICharacter
{
    public IEnemyWeapon m_Weapon { get; protected set; }
    public Room m_Room;
    protected GameObject DecHpPoint;
    protected GameObject Exclamation;
    private GameObject FootCircle;
    protected MaterialPropertyBlock block;
    private Vector2 weaponDir;

    public IEnemy(GameObject obj) : base(obj)
    {
        block = new MaterialPropertyBlock();
        m_Animator = gameObject?.GetComponent<Animator>();
        m_rb = gameObject.GetComponent<Rigidbody2D>();
        DecHpPoint = gameObject.transform.Find("DecHpPoint")?.gameObject;
        Exclamation = gameObject.transform.Find("Exclamation")?.gameObject;
        FootCircle = gameObject.transform.Find("FootCircle")?.gameObject;
        EventCenter.Instance.RegisterObserver<Room>(EventType.OnPlayerEnterBattleRoom, (room) =>
        {
            if (m_Room == room)
            {
                m_Attr.isRun = true;
            }
        });
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_Attr.CurrentHp = m_Attr.m_ShareAttr.MaxHp;
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        if (m_Weapon != null)//有武器时朝向敌人
        {
            weaponDir = (Vector2)GameMediator.Instance.GetController<PlayerController>().Player.gameObject.transform.position - m_Weapon.GetRotOriginPos();
            if (weaponDir.normalized.x > 0.02f)
            {
                isLeft = false;
            }
            else if (weaponDir.normalized.x < -0.02f)
            {
                isLeft = true;
            }
            m_Weapon.RotateWeapon(weaponDir);
            m_Weapon.OnUpdate();

        }
    }
    protected override void OnCharacterDieStart()
    {
        base.OnCharacterDieStart();
        if (m_Room != null)
        {
            m_Room.CurrentEnemyNum -= 1;
        }
        Exclamation?.SetActive(false);
        m_Weapon?.OnExit();
        Object.Destroy(m_Weapon?.gameObject);
        m_Animator.SetBool("isDie", true);
        gameObject.tag = "Untagged";

        if (m_rb != null)
        {
            m_rb.velocity = Vector2.zero;
        }
        Remove();
    }
    public override void UnderAttack(int damage)
    {
        if (m_Attr.isRun)
        {
            base.UnderAttack(damage);
        }
        DecHp effect = ItemPool.Instance.GetItem(EffectType.DecHp, DecHpPoint.transform.position) as DecHp;
        effect.SetTextValue(damage);
        effect.AddToController();
    }
    public void AddWeapon(IEnemyWeapon weapon)
    {
        m_Weapon = weapon;
    }
    public void SetFootCircleActive(bool isopen)
    {
        FootCircle?.SetActive(isopen);
    }

}

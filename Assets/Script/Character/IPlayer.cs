using UnityEngine;

public abstract class IPlayer : ICharacter
{
    public new PlayerAttribute m_Attr { get => base.m_Attr as PlayerAttribute; protected set => base.m_Attr = value; }
    protected PlayerStateController m_StateController;
    public PlayerControlInput m_Input;
    private IPlayerWeapon oldFirstWeapon;
    private IPlayerWeapon oldSecondWeapon;
    private IPlayerPet m_Pet;
    private ISkill m_Skill;
    public ISkill Skill => m_Skill;
    private Vector2 mouseDir;
    private IEnemy currentEnemy;
    private IEnemy lastAimedEnemy;
    public string UserName;
    private float MouseAngle;
    public IPlayer(GameObject obj, PlayerAttribute attr) : base(obj)
    {
        m_Attr = attr;
        gameObject.SetActive(false);
        m_Input = new PlayerControlInput();
        m_Animator = gameObject.transform.Find("Sprite").GetComponent<Animator>();
        m_rb = gameObject.GetComponent<Rigidbody2D>();
        if (m_Attr.FirstWeapon != null)
        {
            if (m_Attr.FirstWeapon.gameObject == null)
            {
                m_Attr.FirstWeapon = WeaponFactory.Instance.GetPlayerWeapon(m_Attr.FirstWeapon.m_Attr.Type, this);
            }
        }
        if (m_Attr.SecondWeapon != null)
        {
            if (m_Attr.SecondWeapon.gameObject == null)
            {
                m_Attr.SecondWeapon = WeaponFactory.Instance.GetPlayerWeapon(m_Attr.SecondWeapon.m_Attr.Type, this);
            }
        }
        if (m_Attr.FirstWeapon != null)
        {
            UseWeapon(m_Attr.FirstWeapon);
        }
        oldFirstWeapon = m_Attr.FirstWeapon;
        oldSecondWeapon = m_Attr.SecondWeapon;

    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController = new PlayerStateController(this);
        m_Attr.CurrentHp = m_Attr.m_ShareAttr.MaxHp;
        m_Attr.CurrentMp = m_Attr.m_ShareAttr.Magic;
        m_Attr.CurrentArmor = m_Attr.m_ShareAttr.Armor;
        m_Attr.HurtArmorRecoveryTimer = m_Attr.m_ShareAttr.HurtArmorRecoveryTime;
        m_Skill = SkillFactory.Instance.GetSkill(m_Attr.CurrentSkillType, this);
        m_Pet = PlayerFactory.Instance.GetPlayerPet(ModelContainer.Instance.GetModel<MemoryModel>().PetType,this);
        GameMediator.Instance.GetController<PlayerController>().AddPet(m_Pet);
        if (m_Attr.FirstWeapon == null)//本来没有一把武器会出问题
        {
            m_Attr.FirstWeapon = WeaponFactory.Instance.GetPlayerWeapon(m_Attr.m_ShareAttr.IdleWeapon, this);
            m_Attr.FirstWeapon.isBeingUsed = true;
        }
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        m_Attr.HurtArmorRecoveryTimer += Time.deltaTime;
        m_Attr.HurtInvincibleTimer += Time.deltaTime;
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        m_StateController.GameUpdate();
        m_Skill.OnUpdate();
        if (m_Attr.HurtArmorRecoveryTimer > m_Attr.m_ShareAttr.HurtArmorRecoveryTime)
        {
            m_Attr.ArmorRecoveryTimer += Time.deltaTime;
            if (m_Attr.ArmorRecoveryTimer > m_Attr.m_ShareAttr.ArmorRecoveryTime)
            {
                m_Attr.ArmorRecoveryTimer = 0;
                if (m_Attr.CurrentArmor < m_Attr.m_ShareAttr.Armor)
                {
                    m_Attr.CurrentArmor += 1;
                }
            }
        }
        if (oldFirstWeapon != m_Attr.FirstWeapon)
        {
            oldFirstWeapon = m_Attr.FirstWeapon;
            OnFirstWeaponChange();
        }
        if (oldSecondWeapon != m_Attr.SecondWeapon)
        {
            oldSecondWeapon = m_Attr.SecondWeapon;
            OnSecondWeaponChange();
        }
        WeaponControl();
        currentEnemy = GetClosestEnemy();
        if (currentEnemy != lastAimedEnemy)
        {
            OnAimedEnemyChange();
            lastAimedEnemy = currentEnemy;
        }
        if (InputUtility.Instance.GetKeyDown(KeyAction.SwapWeapon))
        {
            SwapWeapon();
        }
    }
    private void WeaponControl()
    {
        if (m_Attr.SecondWeapon?.isBeingUsed == true || m_Attr.FirstWeapon?.isBeingUsed == true)
        {
            if (m_Input.isMouseControl)
            {
                mouseDir = m_Input.MouseWorldPos - GetUsedWeapon().GetRotOriginPos();
            }
            else
            {
                if (currentEnemy == null)
                {
                    mouseDir = m_Input.MouseWorldPos;
                }
                else
                {
                    mouseDir = (Vector2)currentEnemy.gameObject.transform.position - GetUsedWeapon().GetRotOriginPos();
                }

            }

            MouseAngle = Vector2.Angle(Vector2.right, mouseDir);
            //to avoid bullet still in ground when the MouseWorldPos is Vector.zero
            if (m_Input.MouseWorldPos == Vector2.zero)
            {
                mouseDir = Quaternion.Euler(0, 0, MouseAngle) * Vector2.right;
            }
            if (mouseDir.normalized.x > 0.02f)
            {
                isLeft = false;
            }
            else if (mouseDir.normalized.x < -0.02f)
            {
                isLeft = true;
            }
            GetUsedWeapon().RotateWeapon(mouseDir);
            GetUsedWeapon().ControlWeapon(m_Input.isAttackKeyDown, mouseDir);
        }
    }
    public override void UnderAttack(int damage)
    {
        m_Attr.HurtArmorRecoveryTimer = 0;
        m_Attr.HurtInvincibleTimer = 0;
        if (m_Attr.CurrentArmor > 0)
        {
            if (m_Attr.CurrentArmor - damage >= 0)
            {
                m_Attr.CurrentArmor -= damage;
            }
            else
            {
                m_Attr.CurrentHp -= damage - m_Attr.CurrentArmor;
                m_Attr.CurrentArmor = 0;
            }
        }
        else
        {
            m_Attr.CurrentHp -= damage;
        }
        if (m_Attr.CurrentHp < 0)
        {
            m_Attr.CurrentHp = 0;
        }
    }
    public virtual void UnderWeaponAttack(EnemyWeaponShareAttribute attr)
    {
        m_Attr.HurtArmorRecoveryTimer = 0;
        m_Attr.HurtInvincibleTimer = 0;
        if (m_Attr.CurrentArmor > 0)
        {
            if (m_Attr.CurrentArmor - attr.Damage >= 0)
            {
                m_Attr.CurrentArmor -= attr.Damage;
            }
            else
            {
                m_Attr.CurrentHp -= attr.Damage - m_Attr.CurrentArmor;
                m_Attr.CurrentArmor = 0;
            }
        }
        else
        {
            m_Attr.CurrentHp -= attr.Damage;
        }
        if (m_Attr.CurrentHp < 0)
        {
            m_Attr.CurrentHp = 0;
        }
    }
    public virtual void UnderEnemyAttack(EnemyAttribute attr)
    {
        m_Attr.HurtArmorRecoveryTimer = 0;
        m_Attr.HurtInvincibleTimer = 0;
        if (m_Attr.CurrentArmor > 0)
        {
            if (m_Attr.CurrentArmor - attr.m_ShareAttr.Damage >= 0)
            {
                m_Attr.CurrentArmor -= attr.m_ShareAttr.Damage;
            }
            else
            {
                m_Attr.CurrentHp -= attr.m_ShareAttr.Damage - m_Attr.CurrentArmor;
                m_Attr.CurrentArmor = 0;
            }
        }
        else
        {
            m_Attr.CurrentHp -= attr.m_ShareAttr.Damage;
        }
        if (m_Attr.CurrentHp < 0)
        {
            m_Attr.CurrentHp = 0;
        }
    }
    public void AddMagicPower(int num)
    {
        m_Attr.CurrentMp += num;
        m_Attr.CurrentMp = Mathf.Clamp(m_Attr.CurrentMp, 0, m_Attr.m_ShareAttr.Magic);
    }
    protected virtual void OnFirstWeaponChange()
    {
        //m_Attr.FirstWeapon.OnEnter();
    }
    protected virtual void OnSecondWeaponChange()
    {
        //m_Attr.SecondWeapon.OnEnter();
    }
    public virtual void AddWeapon(PlayerWeaponType type)
    {
        IPlayerWeapon weapon = WeaponFactory.Instance.GetPlayerWeapon(type, this);
        if (m_Attr.FirstWeapon == null)
        {
            m_Attr.FirstWeapon = weapon;

        }
        else if (m_Attr.SecondWeapon == null)
        {
            m_Attr.SecondWeapon = weapon;
            m_Attr.SecondWeapon.gameObject.SetActive(false);
        }
        else
        {
            if (m_Attr.FirstWeapon.isBeingUsed)
            {
                WeaponFactory.Instance.GetPlayerWeaponObj(m_Attr.FirstWeapon.m_Attr.Type, gameObject.transform.position);
                Object.Destroy(m_Attr.FirstWeapon.gameObject);
                m_Attr.FirstWeapon = weapon;
                UseWeapon(m_Attr.FirstWeapon);

            }
            else
            {
                WeaponFactory.Instance.GetPlayerWeaponObj(m_Attr.SecondWeapon.m_Attr.Type, gameObject.transform.position);
                Object.Destroy(m_Attr.SecondWeapon.gameObject);
                m_Attr.SecondWeapon = weapon;
                UseWeapon(m_Attr.SecondWeapon);

            }
        }
    }
    private void UseWeapon(IPlayerWeapon weapon)
    {
        weapon.isBeingUsed = true;
        weapon.gameObject.SetActive(true);
        IPlayerWeapon anotherWeapon = GetAnotherWeapon(weapon);
        if (anotherWeapon != null)
        {
            anotherWeapon.OnExit();
            anotherWeapon.isBeingUsed = false;
            if (anotherWeapon.gameObject != null)
            {
                anotherWeapon.gameObject.SetActive(false);
            }
        }
    }
    private IPlayerWeapon GetAnotherWeapon(IPlayerWeapon weapon)
    {
        if (weapon == m_Attr.FirstWeapon)
        {
            return m_Attr.SecondWeapon;
        }
        if (weapon == m_Attr.SecondWeapon)
        {
            return m_Attr.FirstWeapon;
        }
        return null;
    }
    private void SwapWeapon()
    {
        if (m_Attr.FirstWeapon == null || m_Attr.SecondWeapon == null)
        {
            return;
        }
        if (m_Attr.FirstWeapon.isBeingUsed)
        {
            UseWeapon(m_Attr.SecondWeapon);
        }
        else
        {
            UseWeapon(m_Attr.FirstWeapon);
        }
    }
    public IPlayerWeapon GetUsedWeapon()
    {
        if (m_Attr.FirstWeapon != null)
        {
            if (m_Attr.FirstWeapon.isBeingUsed)
            {
                return m_Attr.FirstWeapon;
            }
        }

        if (m_Attr.SecondWeapon != null)
        {
            if (m_Attr.SecondWeapon.isBeingUsed)
            {
                return m_Attr.SecondWeapon;
            }
        }
        return null;
    }
    public override void Resurrection()
    {
        base.Resurrection();
        m_Attr.CurrentHp = m_Attr.m_ShareAttr.MaxHp;
        m_Attr.CurrentMp = m_Attr.m_ShareAttr.Magic;
        m_Attr.CurrentArmor = m_Attr.m_ShareAttr.Armor;
        m_Attr.HurtArmorRecoveryTimer = m_Attr.m_ShareAttr.HurtArmorRecoveryTime;
    }
    public Vector2 GetDirection()
    {
        return mouseDir;
    }
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, mouseDir));
    }
    private IEnemy GetClosestEnemy()
    {
        float min = 10;
        GameObject o = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector2.Distance(gameObject.transform.position, obj.transform.position) < min)
            {
                min = Vector2.Distance(gameObject.transform.position, obj.transform.position);
                o = obj;
            }
        }
        if (o == null)
        {
            return null;
        }
        return o.GetComponent<Symbol>().GetCharacter() as IEnemy;
    }
    private void OnAimedEnemyChange()
    {
        lastAimedEnemy?.SetFootCircleActive(false);
        currentEnemy?.SetFootCircleActive(true);
    }
    public void EnterBattleScene()
    {
        EffectFactory.Instance.GetEffect(EffectType.AppearLight, gameObject.transform.position).AddToController();
        m_Pet?.EnterBattleScene();
        gameObject.SetActive(true);
    }
}

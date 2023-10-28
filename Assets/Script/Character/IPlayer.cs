using System.Linq;
using UnityEngine;

public abstract class IPlayer : ICharacter
{
    public new PlayerAttribute m_Attr { get => base.m_Attr as PlayerAttribute; protected set => base.m_Attr = value; }
    protected PlayerStateController m_StateController;
    public PlayerControlInput m_Input;
    private IPlayerPet m_Pet;
    public IPlayerSkill m_Skill { get; protected set; }
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
        m_Attr.Weapons.Clear();
        foreach(PlayerWeaponType type in m_Attr.WeaponTypes)
        {
            m_Attr.Weapons.Add(WeaponFactory.Instance.GetPlayerWeapon(type, this));
        }
        if (m_Attr.Weapons.Count!=0)
        {
            UseWeapon(m_Attr.Weapons[0]);
        }
        m_Attr.isRun = true;
    }
    protected override void OnInit()
    {
        base.OnInit();
        GameMediator.Instance.GetSystem<TalentSystem>().AddTalen(TalentType.Precision, this);
        GameMediator.Instance.GetSystem<TalentSystem>().AddTalen(TalentType.BulletRebound, this);
        GameMediator.Instance.GetSystem<TalentSystem>().AddTalen(TalentType.BulletPenetrate, this);
        m_Attr.CurrentHp = m_Attr.m_ShareAttr.MaxHp;
        m_Attr.CurrentMp = m_Attr.m_ShareAttr.Magic;
        m_Attr.CurrentArmor = m_Attr.m_ShareAttr.Armor;
        m_Pet = PlayerFactory.Instance.GetPlayerPet(ModelContainer.Instance.GetModel<MemoryModel>().PetType, this);
        GameMediator.Instance.GetController<PlayerController>().AddPet(m_Pet);
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController = new PlayerStateController(this);
        m_Attr.HurtArmorRecoveryTimer = m_Attr.m_ShareAttr.HurtArmorRecoveryTime;
        m_Skill = SkillFactory.Instance.GetSkill(m_Attr.CurrentSkillType, this) as IPlayerSkill;
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        m_StateController.GameUpdate();
        m_Skill.OnUpdate();
        m_Attr.HurtArmorRecoveryTimer += Time.deltaTime;
        m_Attr.HurtInvincibleTimer += Time.deltaTime;
        m_Attr.SkillCoolTimer += Time.deltaTime;
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
        if (m_Attr.Weapons.Any(weapon=>weapon.isBeingUsed))
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
    public virtual void AddWeapon(PlayerWeaponType type)
    {
        if (m_Attr.Weapons.Count == 3)
        {
            GameMediator.Instance.GetSystem<BackpackSystem>().AddWeapon(type);
        }
        else
        {
            m_Attr.WeaponTypes.Add(type);
            IPlayerWeapon weapon = WeaponFactory.Instance.GetPlayerWeapon(type, this);
            if (m_Attr.Weapons.Count != 0)
            {
                weapon.gameObject.SetActive(false);
            }
            m_Attr.Weapons.Add(weapon);
        }
    }
    private void UseWeapon(IPlayerWeapon weapon)
    {
        IPlayerWeapon w = GetUsedWeapon();
        if(w!=null)
        {
            w.OnExit();
            w.isBeingUsed = false;
        }
        foreach(IPlayerWeapon item in m_Attr.Weapons)
        {
            if(item!=weapon)
            {
                item.gameObject.SetActive(false);
            }
        }
        weapon.isBeingUsed = true;
        weapon.gameObject.SetActive(true);
    }
    public void RemoveWeapon(PlayerWeaponType weapon)
    {
        if(weapon==GetUsedWeapon().m_Attr.Type)
        {
            int i;
            for (i = 0; i < m_Attr.Weapons.Count; i++)
            {
                if (m_Attr.Weapons[i].m_Attr.Type == weapon)
                {
                    m_Attr.Weapons.RemoveAt(i);
                    break;
                }
            }
            if(i<m_Attr.Weapons.Count)
            {
                UseWeapon(m_Attr.Weapons[i]);
            }
            else if(i-1<m_Attr.Weapons.Count&&i-1>=0)
            {
                UseWeapon(m_Attr.Weapons[i-1]);
            }
        }
        else
        {
            for (int i = 0; i < m_Attr.Weapons.Count; i++)
            {
                if (m_Attr.Weapons[i].m_Attr.Type == weapon)
                {
                    m_Attr.Weapons.RemoveAt(i);
                    break;
                }
            }
        }
    }
    private void SwapWeapon()
    {
        if (m_Attr.Weapons.Count <= 1) return;
        int index=0;
        for(int i=0;i<m_Attr.Weapons.Count;i++)
        {
            if (m_Attr.Weapons[i].isBeingUsed)
            {
                index = i;
            }
        }
        UseWeapon(m_Attr.Weapons[(index + 1) % m_Attr.Weapons.Count]);
    }
    public IPlayerWeapon GetUsedWeapon()
    {
        foreach (IPlayerWeapon weapon in m_Attr.Weapons)
        {
            if (weapon.isBeingUsed)
            {
                return weapon;
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
    public bool CanUseSkill()
    {
        return (m_Attr.SkillCoolTimer > m_Skill.m_Attr.m_ShareAttr.SkillCoolTime) ? true : false;
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class IEnemy : ICharacter
{
    public new EnemyAttribute m_Attr { get => base.m_Attr as EnemyAttribute; protected set => base.m_Attr = value; }
    public IEnemyWeapon m_Weapon { get; protected set; }
    public Room m_Room;
    protected EnemyStateController m_StateController;
    protected GameObject DecHpPoint;
    protected GameObject Exclamation;
    protected TextMeshProUGUI m_TextTotleDamage;
    private GameObject FootCircle;
    private MaterialPropertyBlock block;
    private Vector2 weaponDir;
    private float DamageAccumulateTimer;

    public IEnemy(GameObject obj) : base(obj)
    {
        m_StateController = new EnemyStateController(this);
        block = new MaterialPropertyBlock();
        m_Animator = gameObject?.GetComponent<Animator>();
        m_rb = gameObject.GetComponent<Rigidbody2D>();
        DecHpPoint = gameObject.transform.Find("DecHpPoint")?.gameObject;
        Exclamation = gameObject.transform.Find("Exclamation")?.gameObject;
        FootCircle = gameObject.transform.Find("FootCircle")?.gameObject;
        m_TextTotleDamage = gameObject.transform.Find("TextTotleDamage")?.GetChild(0).GetComponent<TextMeshProUGUI>();
        EventCenter.Instance.RegisterObserver<Room>(EventType.OnPlayerEnterBattleRoom, (room) =>
        {
            if(m_Room==room)
            {
                m_Attr.isRun = true;
            }
        });
    }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        if(m_Attr.m_ShareAttr.isElite)
        {
            transform.localScale= Vector3.one*1.5f;
        }
        m_Attr.CurrentHp = m_Attr.m_ShareAttr.MaxHp;
        DamageAccumulateTimer = 2f;
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        DamageAccumulateTimer += Time.deltaTime;
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
        m_StateController.GameUpdate();
        if(m_TextTotleDamage!=null)
        {
            if (DamageAccumulateTimer < 2)
            {
                m_TextTotleDamage.gameObject.SetActive(true);
                m_TextTotleDamage.gameObject.transform.rotation = Quaternion.identity;
            }
            else
            {
                m_TextTotleDamage.gameObject.SetActive(false);
                m_TextTotleDamage.text = "0";
            }
        }

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
        if(m_Room!=null)
        {
            m_Room.CurrentEnemyNum -= 1;
        }
        Exclamation?.SetActive(false);
        m_TextTotleDamage?.gameObject.SetActive(false);
        Object.Destroy(m_Weapon?.gameObject);
        m_Animator.SetBool("isDie", true);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
        if (gameObject.transform.Find("Collider"))
        {
            gameObject.transform.Find("Collider").GetComponent<CapsuleCollider2D>().isTrigger = true;
        }
        gameObject.tag = "Untagged";
        m_StateController.StopCurrentState();
        if (m_rb != null)
        {
            m_rb.velocity = Vector2.zero;
        }
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            ItemFactory.Instance.GetItem(ItemType.EnergyBall, (Vector2)gameObject.transform.position + Random.insideUnitCircle * 2).AddToController();
        }
        if (Random.Range(0, 2) == 0)
        {
            ItemFactory.Instance.GetCoin(CoinType.Coppers, (Vector2)gameObject.transform.position + Random.insideUnitCircle * 2).AddToController();
        }
        ShouldBeRemove = true;
    }
    public override void UnderAttack(int damage)
    {
        if(m_Attr.isRun)
        {
            base.UnderAttack(damage);
        }
        DamageAccumulateTimer = 0;
        if(m_TextTotleDamage!=null)
        {
            m_TextTotleDamage.text = (int.Parse(m_TextTotleDamage.text) + damage).ToString();
        }
        DecHp effect = ItemPool.Instance.GetItem(EffectType.DecHp, DecHpPoint.transform.position) as DecHp;
        effect.SetTextValue(damage);
        effect.DoAnimation();
        HitEffect();
    }
    public void AddWeapon(IEnemyWeapon weapon)
    {
        m_Weapon = weapon;
    }
    private void HitEffect()
    {
        gameObject.GetComponent<SpriteRenderer>().GetPropertyBlock(block);
        block.SetColor("_Color", Color.white);
        gameObject.GetComponent<SpriteRenderer>().SetPropertyBlock(block);
        CoroutinePool.Instance.StartCoroutine(ResumeColor());
    }
    private IEnumerator ResumeColor()
    {
        yield return new WaitForSeconds(1f / 12f);
        gameObject.GetComponent<SpriteRenderer>().GetPropertyBlock(block);
        block.SetColor("_Color", Color.black);
        gameObject.GetComponent<SpriteRenderer>().SetPropertyBlock(block);
    }
    public void SetFootCircleActive(bool isopen)
    {
        FootCircle?.SetActive(isopen);
    }

}

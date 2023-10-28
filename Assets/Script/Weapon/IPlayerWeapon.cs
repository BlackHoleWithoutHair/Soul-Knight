using UnityEngine;
public class IPlayerWeapon : IWeapon
{
    public PlayerWeaponShareAttribute m_Attr;
    protected GameObject SparkPoint;
    protected BindableProperty<bool> isAttackKeyDown = new BindableProperty<bool>(false);
    private Animator m_Animator;
    
    protected Vector2 aimDir;
    public bool isBeingUsed;
    public IPlayerWeapon(GameObject obj, ICharacter character) : base(obj, character)
    {
        gameObject = obj;
        m_Character = character;
        isAttackKeyDown.Register((val) =>
        {
            if (val == true)
            {
                (m_Character.m_Attr as PlayerAttribute).SpeedDecreaseFac += m_Attr.SpeedDecrease;
            }
            else
            {
                (m_Character.m_Attr as PlayerAttribute).SpeedDecreaseFac -= m_Attr.SpeedDecrease;
            }
        });
    }
    protected override void OnInit()
    {
        base.OnInit();
        SparkPoint = UnityTool.Instance.GetGameObjectFromChildren(gameObject, "SparkPoint");
        m_Animator = UnityTool.Instance.GetComponentFromChild<Animator>(gameObject, m_Attr.Type.ToString());
        m_Attr.CriticalRate += (m_Character.m_Attr as PlayerAttribute).m_ShareAttr.Critical;
    }
    public override void OnExit()
    {
        base.OnExit();
        isAttackKeyDown.Value = false;
    }
    protected override void OnFire()
    {
        (m_Character.m_Attr as PlayerAttribute).CurrentMp -= m_Attr.MagicSpend;
    }
    protected virtual IPlayerBullet CreateBullet(PlayerBulletType type,PlayerWeaponShareAttribute attr, int i=0)
    {
        IPlayerBullet bullet = null;
        if (FirePoint!=null)
        {
             bullet= ItemPool.Instance.GetPlayerBullet(type, attr, this, FirePoint.transform.position, GetShotRot(i)) as IPlayerBullet;
        }
        else
        {
            bullet = ItemPool.Instance.GetPlayerBullet(type, attr, this, Vector2.zero, Quaternion.identity) as IPlayerBullet;
        }
        return bullet;
    }
    protected virtual IPlayerBullet CreateBullet(PlayerBulletType type, PlayerWeaponShareAttribute attr,Vector2 pos, int i = 0)
    {
        IPlayerBullet bullet = ItemPool.Instance.GetPlayerBullet(type, attr, this, pos, GetShotRot(i)) as IPlayerBullet;
        return bullet;
    }
    protected virtual Quaternion GetShotRot(int i=0)
    {
        ITalent talent = GameMediator.Instance.GetSystem<TalentSystem>().GetTalent(TalentType.Precision, m_Character as IPlayer);
        if(talent!=null)
        {
            int scatering = (talent as Precision).GetWeaponScatering(m_Attr);
            return Quaternion.Euler(0,0,i*(talent as Precision).GetWeaponBaseAngle(m_Attr))* Quaternion.Euler(0, 0, Random.Range(-scatering, scatering)) * Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, aimDir));
        }
        else
        {
            return Quaternion.Euler(0, 0, Random.Range(-m_Attr.ScatteringRate, m_Attr.ScatteringRate)) * Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, aimDir));
        }
    }
    public void ControlWeapon(bool isAttackKeyDown, Vector2 aimDir)
    {
        this.isAttackKeyDown.Value = isAttackKeyDown;
        this.aimDir = aimDir;
    }
    protected void ShowFireSpark(BulletColorType color)
    {
        Spark spark = ItemPool.Instance.GetItem(EffectType.Spark, SparkPoint.transform.position) as Spark;
        spark.SetColor(color);
        spark.SetRotation(SparkPoint.transform.rotation);
        spark.SetParent(SparkPoint.transform);
        spark.AddToController();
    }
    protected void PlayRecoilAnim()
    {
        m_Animator.SetTrigger("isShot");
    }
}

using UnityEngine;

public class IEnemyWeapon : IWeapon
{
    public EnemyWeaponShareAttribute m_Attr;
    private bool isFire;
    private bool isBeforeFireStartUpdate;
    private bool isBeforeFireStart;
    public IEnemyWeapon(GameObject obj, ICharacter character) : base(obj, character) { }
    protected Quaternion GetShotRotation()
    {
        return RotOrigin.transform.rotation;
    }
    protected virtual void OnBeforeFireStart() { }//需要前置动作时重写，如弓的蓄力瞄准
    protected virtual void OnBeforeFireUpdate()
    {
        if (!isBeforeFireStart)
        {
            isBeforeFireStart = true;
            OnBeforeFireStart();
        }
    }
    protected override void OnFire()
    {
        base.OnFire();
        isBeforeFireStartUpdate = false;
        isBeforeFireStart = false;
        isFire = false;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isBeforeFireStartUpdate)
        {
            OnBeforeFireUpdate();
        }
        if (isFire)
        {
            OnFire();
        }
    }

    public void StartBeforeFireUpdate()
    {
        isBeforeFireStartUpdate = true;
    }
    public void Fire()
    {
        isFire = true;
    }
}

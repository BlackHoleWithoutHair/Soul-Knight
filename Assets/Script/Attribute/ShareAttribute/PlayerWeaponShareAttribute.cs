[System.Serializable]
public class PlayerWeaponShareAttribute : IWeaponShareAttribute
{
    public PlayerWeaponType Type;
    public WeaponCategory Category;
    public QualityType Quality;
    public int Damage;
    public int MagicSpend;
    public int CriticalRate;
    public int ScatteringRate;
    public int BaseAngle;
    public float SpeedDecrease;
}

using System.Collections.Generic;

public class BackpackSystem:AbstractSystem
{
    public List<PlayerWeaponType> Weapons { get; protected set; }
    protected override void OnInit()
    {
        base.OnInit();
        Weapons= new List<PlayerWeaponType>(); 
    }
    public void AddWeapon(PlayerWeaponType weapon)
    {
        Weapons.Add(weapon);
    }
    public void RemoveWeapon(PlayerWeaponType type)
    {
        Weapons.Remove(type);
    }
}

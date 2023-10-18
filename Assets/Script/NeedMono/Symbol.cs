using UnityEngine;

public class Symbol : MonoBehaviour
{
    private ICharacter character;
    private IWeapon weapon;
    public void SetCharacter(ICharacter character)
    {
        this.character = character;
    }
    public ICharacter GetCharacter()
    {
        return character;
    }
    public void SetWeapon(IWeapon weapon)
    {
        this.weapon = weapon;
    }
    public IWeapon GetWeapon()
    {
        return weapon;
    }
}

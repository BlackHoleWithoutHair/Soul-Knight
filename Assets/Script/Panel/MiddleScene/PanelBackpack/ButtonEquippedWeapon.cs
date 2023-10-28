using UnityEngine;
using UnityEngine.UI;

public class ButtonEquippedWeapon:IBackpackButtonWeapon
{
    public ButtonEquippedWeapon(IPanel panel,GameObject obj):base(panel,obj) { }
    protected override void OnInit()
    {
        base.OnInit();

        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            ButtonEquip.gameObject.SetActive(false);
            ButtonUnEquip.gameObject.SetActive(true);
            ShowInfo();
        });
    }

}

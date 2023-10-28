using UnityEngine;
using UnityEngine.UI;

public class ButtonUnEquippedWeapon:IBackpackButtonWeapon
{
    public ButtonUnEquippedWeapon(IPanel panel,GameObject obj) : base(panel, obj) { }
    protected override void OnInit()
    {
        base.OnInit();
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            ButtonEquip.gameObject.SetActive(true);
            ButtonUnEquip.gameObject.SetActive(true);
            ShowInfo();
        });
    }

}

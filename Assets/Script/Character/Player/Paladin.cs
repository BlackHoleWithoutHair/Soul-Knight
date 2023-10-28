using UnityEngine;

public class Paladin : IPlayer
{
    public Paladin(GameObject obj, PlayerAttribute attr) : base(obj, attr) { }
    protected override void OnCharacterStart()
    {
        base.OnCharacterStart();
        m_StateController.SetOtherState(typeof(PaladinIdleState));
    }
    protected override void OnCharacterUpdate()
    {
        base.OnCharacterUpdate();
    }
    protected override void OnCharaterDieUpdate()
    {
        base.OnCharaterDieUpdate();
        m_StateController.GameUpdate();
    }
    private void SetWeaponLayer(IWeapon weapon)
    {
        weapon.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
        weapon.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 10;
        weapon.gameObject.transform.GetChild(0).localPosition += new Vector3(0.4f, 0, 0);
    }
}

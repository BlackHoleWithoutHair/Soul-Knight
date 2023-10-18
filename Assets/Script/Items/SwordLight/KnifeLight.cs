using UnityEngine;

public class KnifeLight : IKnifeLight
{
    public KnifeLight(GameObject obj, Quaternion rot, PlayerWeaponShareAttribute attr) : base(obj, rot, attr)
    {
        type = KnifeLightType.KnifeLight;
    }
}

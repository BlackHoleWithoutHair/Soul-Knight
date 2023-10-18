using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponScriptableObject : ScriptableObject
{
    public TextAsset textAsset;
    public List<PlayerWeaponShareAttribute> WeaponShareAttrs;
    private void OnValidate()
    {
        UnityTool.Instance.WriteDataToListFromTextAssest(WeaponShareAttrs, textAsset);
    }
}

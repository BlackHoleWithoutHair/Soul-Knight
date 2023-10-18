using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWeaponData", menuName = "ScriptableObjects/EnemyWeaponData")]
public class EnemyWeaponScriptableObject : ScriptableObject
{
    public TextAsset textAsset;
    public List<EnemyWeaponShareAttribute> EnemyWeaponShareAttrs;
    private void OnValidate()
    {
        UnityTool.Instance.WriteDataToListFromTextAssest(EnemyWeaponShareAttrs, textAsset);
    }
}

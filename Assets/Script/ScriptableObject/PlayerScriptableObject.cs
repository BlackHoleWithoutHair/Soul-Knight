using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerScriptableObject : ScriptableObject
{
    public TextAsset textAsset;
    public List<PlayerShareAttr> PlayerShareAttrs;
    private void OnValidate()
    {
        UnityTool.Instance.WriteDataToListFromTextAssest(PlayerShareAttrs, textAsset);
    }
}

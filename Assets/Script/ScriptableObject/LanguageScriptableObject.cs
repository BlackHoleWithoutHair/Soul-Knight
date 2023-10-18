using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageData", menuName = "ScriptableObjects/LanguageData")]
public class LanguageScriptableObject : ScriptableObject
{
    public TextAsset textAsset;
    public List<LanguageModel> languageDatas;
    private void OnValidate()
    {
        UnityTool.Instance.WriteLanguageDataFromTextToList(languageDatas, textAsset);
    }
}

using UnityEngine;

public class LanguageCommand : Singleton<LanguageCommand>
{
    private LanguageCommand() { }
    public string GetTranslation(string key)
    {
        foreach (LanguageModel model in ProxyResourceFactory.Instance.Factory.GetScriptableObject<LanguageScriptableObject>().languageDatas)
        {
            if (model.strList[0].CompareTo(key) == 0)
            {
                if (ArchiveQuery.Instance.GetLanguage() == LanguageType.English)
                {
                    return model.strList[0];
                }
                else
                {
                    return model.strList[1];
                }
            }
        }
        Debug.Log("LanguageCommand GetLanguage return null");
        return key;
    }
}

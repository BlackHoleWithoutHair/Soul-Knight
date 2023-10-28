using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;


public class BindableProperty<T> where T : IEquatable<T>
{
    private T _mValue = default(T);

    public T Value
    {
        get => _mValue;
        set
        {
            if (value.Equals(_mValue)) return;
            _mValue = value;
            OnValueChanged?.Invoke(_mValue);
        }
    }

    private Action<T> OnValueChanged;
    public BindableProperty(T val)
    {
        _mValue = val;
    }
    public void Register(Action<T> a)
    {
        OnValueChanged += a;
    }
    public static implicit operator T(BindableProperty<T> property)
    {
        return property.Value;
    }
}
public class UnityTool
{
    private static UnityTool instance;
    public static UnityTool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UnityTool();
            }
            return instance;
        }
    }
    private GameObject m_Canvas;
    private UnityTool()
    {
        m_Canvas = GameObject.Find("MainCanvas");
        EventCenter.Instance.RegisterObserver(EventType.OnSceneChangeComplete, () =>
        {
            m_Canvas = GameObject.Find("MainCanvas");
        });
    }
    public GameObject GetGameObjectFromCanvas(string name)
    {
        foreach (Transform obj in m_Canvas.GetComponentsInChildren<Transform>(true))
        {
            if (obj.name == name)
            {
                return obj.gameObject;
            }
        }
        Debug.Log("UnityTool GetGameObjectFromCanvas(" + name + ") return null");
        return null;
    }
    public GameObject GetGameObjectFromChildren(GameObject parent, string name, bool inActive)
    {
        foreach (Transform obj in parent.GetComponentsInChildren<Transform>(inActive))
        {
            if (obj.name == name)
            {
                return obj.gameObject;
            }
        }
        Debug.Log("UnityTool GetGameObjectFromChildren(" + parent.name + " " + name + ") return null");
        return null;
    }
    public GameObject GetGameObjectFromChildren(GameObject parent, string name)
    {
        return GetGameObjectFromChildren(parent, name, true);
    }
    public T GetComponentFromChild<T>(GameObject parent, string name)
    {
        return GetGameObjectFromChildren(parent, name).GetComponent<T>();
    }
    public GameObject GetMainCanvas()
    {
        return m_Canvas;
    }
    public void ReFindCanvas()
    {
        m_Canvas = GameObject.Find("MainCanvas");
    }
    public void ParabolaMove(Rigidbody2D rb, bool isLeft, float width, float height)
    {

        float x = Math.Abs(width * Mathf.Sqrt(2 * rb.gravityScale * 9.8f * height) / (4 * height));
        float y = Mathf.Sqrt(2 * 9.8f * rb.gravityScale * height);
        if (isLeft)
        {
            rb.velocity = new Vector2(-x, y);
        }
        else
        {
            rb.velocity = new Vector2(x, y);
        }
    }
    public void SetTextColor(TextMeshProUGUI text, QualityType quality)
    {
        switch (quality)
        {
            case QualityType.White:
                text.color = Color.white; break;
            case QualityType.Green:
                text.color = new Color(61f / 255f, 226f / 255f, 90 / 255); break;
            case QualityType.Blue:
                text.color = new Color(14f / 255f, 160f / 255f, 255f / 255f); break;
            case QualityType.Purple:
                text.color = new Color(190f / 255f, 7f / 255, 201f / 255f); break;
            case QualityType.Orange:
                text.color = new Color(255f / 255f, 143f / 255f, 1f / 255f); break;
            case QualityType.Red:
                text.color = new Color(255f / 255f, 26f / 255f, 26f / 255f); break;
        }
    }
    public Color GetBulletColor(BulletColorType type)
    {
        switch (type)
        {
            case BulletColorType.White:
                return Color.white;
            case BulletColorType.Red:
                return new Color(253f / 255f, 78f / 255f, 38f / 255f);

            case BulletColorType.Orange:
                return Color.magenta;

            case BulletColorType.Yellow:
                return Color.yellow;

            case BulletColorType.Green:
                return new Color(164f / 255f, 254f / 255f, 59f / 255f);

            case BulletColorType.Cyan:
                return new Color(0, 234f / 255f, 1);

            case BulletColorType.Blue:
                return new Color(0, 17f / 255f, 248f / 255f);

            case BulletColorType.Purple:
                return new Color(157f / 255f, 0, 253f / 255f);
            case BulletColorType.Magenta:
                return Color.magenta;
            
            default: return Color.white;
        }
    }
    public object ChangeType(string s, Type type)//字符串转指定类型
    {
        if (s == "TRUE")
        {
            return true;
        }
        if (s == "FALSE")
        {
            return false;
        }
        if (typeof(Enum).IsAssignableFrom(type))
        {
            //Debug.Log(type.ToString() + " " + s);

            return Enum.Parse(type, s);
        }
        return Convert.ChangeType(s, type);
    }
    private class ListInfo
    {
        public object list;
        public string name;
        public Type type;
        public MethodInfo AddMethod;
        public MethodInfo ClearMethod;
    }
    public void WriteDataToListFromTextAssest<T>(List<T> list, TextAsset textAsset) where T : new()
    {
        if (!textAsset)
        {
            return;
        }
        list.Clear();
        Type type = typeof(T);
        Type lastType = null;
        List<ListInfo> listOfListInfo = new List<ListInfo>();
        string[] lineText = textAsset.text.Replace("\r", "").Split('\n');
        string[] fieldName = lineText[0].Split(',');
        foreach (string s in fieldName)
        {
            FieldInfo info = type.GetField(s);
            if (typeof(System.Collections.IList).IsAssignableFrom(info.FieldType))
            {
                if (lastType == null)
                {
                    lastType = info.FieldType;
                    listOfListInfo.Add(GetListInfo(s, info));
                }
                if (info.FieldType != lastType)
                {
                    lastType = info.FieldType;
                    listOfListInfo.Add(GetListInfo(s, info));
                }
            }
        }
        for (int i = 1; i < lineText.Length; i++)
        {
            if (lineText[i] == "") continue;
            string[] rows = lineText[i].Split(',');
            if (rows[0] == "") continue;
            T obj = new T();
            //clear the list in listinfo every line
            foreach (ListInfo listInfo in listOfListInfo)
            {
                listInfo.list = Activator.CreateInstance(typeof(List<>).MakeGenericType(listInfo.type.GenericTypeArguments));
            }
            for (int j = 0; j < rows.Length; j++)
            {
                FieldInfo info = type.GetField(fieldName[j]);
                if (info == null) continue;
                //if the field is type of list ,assign value to the list in listInfo
                if (typeof(System.Collections.IList).IsAssignableFrom(info.FieldType))
                {
                    foreach (ListInfo listInfo in listOfListInfo)
                    {
                        if (info.FieldType == listInfo.type)
                        {
                            listInfo.AddMethod.Invoke(listInfo.list, new object[] { ChangeType(rows[j], info.FieldType.GenericTypeArguments[0]) });
                        }
                    }
                }
                else
                {
                    info.SetValue(obj, ChangeType(rows[j], info.FieldType));
                }

            }
            if (listOfListInfo.Count != 0)
            {
                foreach (ListInfo listInfo in listOfListInfo)
                {

                    type.GetField(listInfo.name).SetValue(obj, listInfo.list);
                }
            }
            list.Add(obj);
        }
    }
    public void WriteEnemyDistributionFromTextAssetToList(List<EnemyDistribution> list, TextAsset textAsset)
    {
        if (textAsset == null)
        {
            return;
        }
        list.Clear();
        string[] textLine = textAsset.text.Split('\n');
        string[] textType = textLine[0].Split(",");
        for (int i = 1; i < textLine.Length; i++)
        {
            if (textLine[i] == "") return;
            string[] textRow = textLine[i].Split(",");
            EnemyDistribution distribution = new EnemyDistribution();
            for (int j = 1; j < textRow.Length; j++)
            {
                distribution.stage = int.Parse(textRow[0]);
                if ((bool)ChangeType(textRow[j], typeof(bool)))
                {
                    distribution.types.Add(Enum.Parse<EnemyType>(textType[j]));
                }
            }
            list.Add(distribution);
        }
    }
    public void WriteCompositionDataFromTextToList(List<CompositionData> list, TextAsset textAsset)
    {
        if (textAsset == null)
        {
            return;
        }
        list.Clear();
        string[] textLine = textAsset.text.Replace("\r", "").Split('\n');
        string[] textMaterialType = textLine[0].Split(",");
        for (int i = 1; i < textLine.Length; i++)
        {
            if (textLine[i] == "") continue;
            string[] textRow = textLine[i].Split(",");
            CompositionData data = new CompositionData();
            bool isExist = true;
            for (int j = 1; j < textRow.Length; j++)
            {
                if (Enum.TryParse(textRow[0], out PlayerWeaponType type))
                {
                    data.weaponType = type;
                }
                else
                {
                    isExist = false;
                    break;
                }
                if (textRow[j] != "")
                {
                    MaterialInfo info = new MaterialInfo();
                    info.materialType = Enum.Parse<MaterialType>(textMaterialType[j]);
                    info.num = int.Parse(textRow[j]);
                    data.materialInfos.Add(info);
                }
            }
            if (isExist)
            {
                list.Add(data);
            }

        }
    }
    public void WriteLanguageDataFromTextToList(List<LanguageModel> list, TextAsset textAsset)
    {
        if (textAsset == null)
        {
            return;
        }
        list.Clear();
        string[] textLine = textAsset.text.Replace("\r", "").Split('\n');
        string[] LanguageType = textLine[0].Split(",");
        for (int i = 1; i < textLine.Length; i++)
        {
            if (textLine[i] == "") continue;
            string[] textRow = textLine[i].Split(",");
            LanguageModel data = new LanguageModel();
            for (int j = 0; j < textRow.Length; j++)
            {
                if (textRow[j] != "")
                {
                    data.strList.Add(textRow[j]);
                }
            }
            if (data.strList.Count != 0)
            {
                list.Add(data);
            }
        }
    }
    private ListInfo GetListInfo(string name, FieldInfo info)
    {
        ListInfo listInfo = new ListInfo();
        listInfo.list = Activator.CreateInstance(typeof(List<>).MakeGenericType(info.FieldType.GenericTypeArguments));
        listInfo.name = name;
        listInfo.type = info.FieldType;
        listInfo.AddMethod = info.FieldType.GetMethod("Add");
        listInfo.ClearMethod = info.FieldType.GetMethod("Clear");
        return listInfo;
    }
    public void DestroyChildrenExceptFirstChild(Transform trans, bool isActive = true)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            if (i > 0)
            {
                if (isActive)
                {
                    if (trans.GetChild(i).gameObject.activeSelf)
                    {
                        UnityEngine.Object.Destroy(trans.GetChild(i).gameObject);
                    }
                }
                else
                {
                    UnityEngine.Object.Destroy(trans.GetChild(i).gameObject);
                }
            }
        }
    }
    public void ClearResidualChild(Transform parent, int beginAt)
    {
        if (beginAt==0)
        {
            parent.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            for (int i = beginAt; i < parent.childCount; i++)
            {
                UnityEngine.Object.Destroy(parent.GetChild(i).gameObject);
            }
        }
    }
    public T DeepCopyByReflection<T>(T obj)
    {
        if (obj is string || obj.GetType().IsValueType)
            return obj;

        object retval = Activator.CreateInstance(obj.GetType());
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        foreach (var field in fields)
        {
            try
            {
                field.SetValue(retval, DeepCopyByReflection(field.GetValue(obj)));
            }
            catch { }
        }

        return (T)retval;
    }
}

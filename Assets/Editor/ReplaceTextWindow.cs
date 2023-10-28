#if UNITY_EDITOR
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 需要指定字体的路径 名字   路径放到Resources文件夹中
/// </summary>
public class MyTool : EditorWindow
{
    //替换场景内的所有字体
    [MenuItem("FontTools/替换场景中所有text字体")]
    public static void ChangeFont_Scene()
    {
        //加载目标字体  "目标字体的名字"      
        TMP_FontAsset targetFont = Resources.Load<TMP_FontAsset>("simsun SDF");
        //获取场景所有激活物体
        //GameObject[] objs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        //获取场景所有物体
        GameObject[] allObj = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        TextMeshProUGUI tmpText;
        int textCount = 0;
        for (int i = 0; i < allObj.Length; i++)
        {
            //带有Text组件的GameObject，替换字体
            tmpText = allObj[i].GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                textCount++;
                tmpText.font = targetFont;
                //在此扩展，可以给添加外边框，也可以根据需求进行其他操作
                //allObj[i].AddComponent<Outline>();
            }
        }
        Debug.Log("<color=yellow> 当前场景共有：物体 </color>" + allObj.Length + "<color=yellow> 个，Text组件 </color>" + textCount + "<color=green> 个 </color>");
    }
    //替换资源文件夹中全部Prefab的字体
    [MenuItem("FontTools/替换预设物中所有text字体")]
    public static void ChangeFont_Prefab()
    {
        TMP_FontAsset targetFont = Resources.Load<TMP_FontAsset>("simsun SDF");
        List<TextMeshProUGUI[]> textList = new List<TextMeshProUGUI[]>();
        //获取Asset文件夹下所有Prefab的GUID
        string[] ids = AssetDatabase.FindAssets("t:Prefab");
        string tmpPath;
        GameObject tmpObj;
        TextMeshProUGUI[] tmpArr;
        for (int i = 0; i < ids.Length; i++)
        {
            tmpObj = null;
            tmpArr = null;
            //根据GUID获取路径
            tmpPath = AssetDatabase.GUIDToAssetPath(ids[i]);
            if (!string.IsNullOrEmpty(tmpPath))
            {
                //根据路径获取Prefab(GameObject)
                tmpObj = AssetDatabase.LoadAssetAtPath(tmpPath, typeof(GameObject)) as GameObject;
                if (tmpObj != null)
                {
                    //获取Prefab及其子物体孙物体.......的所有Text组件
                    tmpArr = tmpObj.GetComponentsInChildren<TextMeshProUGUI>();
                    if (tmpArr != null && tmpArr.Length > 0)
                        textList.Add(tmpArr);
                }
            }
        }
        //替换所有Text组件的字体
        int textCount = 0;
        for (int i = 0; i < textList.Count; i++)
        {
            for (int j = 0; j < textList[i].Length; j++)
            {
                textCount++;
                textList[i][j].font = targetFont;
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("<color=yellow> 当前ProJect共有：Prefab </color>" + ids.Length + "<color=yellow> 个，带有Text组件Prefab </color>" + textList.Count + "<color=green> 个，Text组件 </color>" + textCount + "<color=green> 个 </color>");
    }
}
#endif
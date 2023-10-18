#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
/// <summary>
/// ��Ҫָ�������·�� ����   ·���ŵ�Resources�ļ�����
/// </summary>
public class MyTool : EditorWindow
{
    //�滻�����ڵ���������
    [MenuItem("FontTools/�滻����������text����")]
    public static void ChangeFont_Scene()
    {
        //����Ŀ������  "Ŀ�����������"      
        TMP_FontAsset targetFont = Resources.Load<TMP_FontAsset>("simsun SDF");
        //��ȡ�������м�������
        //GameObject[] objs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        //��ȡ������������
        GameObject[] allObj = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        TextMeshProUGUI tmpText;
        int textCount = 0;
        for (int i = 0; i < allObj.Length; i++)
        {
            //����Text�����GameObject���滻����
            tmpText = allObj[i].GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                textCount++;
                tmpText.font = targetFont;
                //�ڴ���չ�����Ը������߿�Ҳ���Ը������������������
                //allObj[i].AddComponent<Outline>();
            }
        }
        Debug.Log("<color=yellow> ��ǰ�������У����� </color>" + allObj.Length + "<color=yellow> ����Text��� </color>" + textCount + "<color=green> �� </color>");
    }
    //�滻��Դ�ļ�����ȫ��Prefab������
    [MenuItem("FontTools/�滻Ԥ����������text����")]
    public static void ChangeFont_Prefab()
    {
        TMP_FontAsset targetFont = Resources.Load<TMP_FontAsset>("simsun SDF");
        List<TextMeshProUGUI[]> textList = new List<TextMeshProUGUI[]>();
        //��ȡAsset�ļ���������Prefab��GUID
        string[] ids = AssetDatabase.FindAssets("t:Prefab");
        string tmpPath;
        GameObject tmpObj;
        TextMeshProUGUI[] tmpArr;
        for (int i = 0; i < ids.Length; i++)
        {
            tmpObj = null;
            tmpArr = null;
            //����GUID��ȡ·��
            tmpPath = AssetDatabase.GUIDToAssetPath(ids[i]);
            if (!string.IsNullOrEmpty(tmpPath))
            {
                //����·����ȡPrefab(GameObject)
                tmpObj = AssetDatabase.LoadAssetAtPath(tmpPath, typeof(GameObject)) as GameObject;
                if (tmpObj != null)
                {
                    //��ȡPrefab����������������.......������Text���
                    tmpArr = tmpObj.GetComponentsInChildren<TextMeshProUGUI>();
                    if (tmpArr != null && tmpArr.Length > 0)
                        textList.Add(tmpArr);
                }
            }
        }
        //�滻����Text���������
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
        Debug.Log("<color=yellow> ��ǰProJect���У�Prefab </color>" + ids.Length + "<color=yellow> ��������Text���Prefab </color>" + textList.Count + "<color=green> ����Text��� </color>" + textCount + "<color=green> �� </color>");
    }
}
#endif
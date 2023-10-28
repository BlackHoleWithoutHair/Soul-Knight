using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class TilemapReplaceWindow : EditorWindow
{
    private TilemapReplaceWindow()
    {
        this.titleContent = new GUIContent("TilemapReplaceWindow");
    }
    [MenuItem("TilemapTools/ReplaceWindow")]
    private static void ShowWindow()
    {
        GetWindow(typeof(TilemapReplaceWindow));
    }
    private string path;
    private string objName;
    private GameObject obj;
    private TileBase tile;
    private void OnGUI()
    {
        path = EditorGUILayout.TextField("�ļ���·��", path);
        objName = EditorGUILayout.TextField("���滻������", objName);
        //obj=(GameObject)EditorGUILayout.ObjectField("Ŀ������", obj, typeof(GameObject), true);
        tile = (TileBase)EditorGUILayout.ObjectField("�滻��ͼ��", tile, typeof(TileBase), true);
        if (GUILayout.Button("ok"))
        {
            foreach (GameObject obj in Resources.LoadAll<GameObject>(path))
            {
                Tilemap tilemap = obj.transform.GetChild(0).Find(objName).GetComponent<Tilemap>();
                BoundsInt bounds = tilemap.cellBounds;
                TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
                for (int x = 0; x < bounds.size.x; x++)
                {
                    for (int y = 0; y < bounds.size.y; y++)
                    {
                        TileBase tile = allTiles[x + y * bounds.size.x];
                        if (tile != null)
                        {
                            tilemap.SetTile(new Vector3Int(x + bounds.min.x, y + bounds.min.y), this.tile);
                            Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                        }
                        else
                        {
                            Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                        }
                    }
                }
            }

        }
        

        if(GUILayout.Button("Apply"))
        {
            //EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}

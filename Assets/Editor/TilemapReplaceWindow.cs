using UnityEditor;
using UnityEngine;
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
        //method 1
        path = EditorGUILayout.TextField("文件夹路径", path);
        objName = EditorGUILayout.TextField("被替换物体名", objName);
        tile = (TileBase)EditorGUILayout.ObjectField("替换的图块", tile, typeof(TileBase), true);
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

        obj = (GameObject)EditorGUILayout.ObjectField("目标物体", obj, typeof(GameObject), true);

        if (GUILayout.Button("ok"))
        {
            Tilemap tilemap = obj.GetComponent<Tilemap>();
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
        if (GUILayout.Button("Apply"))
        {
            //EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }

}

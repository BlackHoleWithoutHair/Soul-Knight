namespace Edgar.Unity.Assets.Edgar.Editor.Grid3D
{
    public class EditorUtilsGrid3D
    {
        //    #if OndrejNepozitekEdgar
        //    [MenuItem("Edgar debug/Fix 3D models")]
        //    public static void Fix3DModels()
        //    {
        //        var guids = AssetDatabase.FindAssets("", new[] { "Assets/Edgar/Examples/Grid3D/Dungeon1/Models" });

        //        foreach (var guid in guids.Distinct())
        //        {
        //            var path = AssetDatabase.GUIDToAssetPath(guid);

        //            if (!path.EndsWith(".fbx"))
        //            {
        //                continue;
        //            }

        //            var gameObjectFromModel = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        //            var gameObjectInstance = PrefabUtility.InstantiatePrefab(gameObjectFromModel) as GameObject;
        //            var filename = Path.GetFileNameWithoutExtension(path);
        //            Directory.CreateDirectory("Assets/Edgar/Examples/Grid3D/Dungeon1/Prefabs");
        //            PrefabUtility.SaveAsPrefabAsset(gameObjectInstance, $"Assets/Edgar/Examples/Grid3D/Dungeon1/Prefabs/{filename}.prefab");
        //            Object.DestroyImmediate(gameObjectInstance);
        //        }
        //    }

        //    [MenuItem("Edgar debug/Fix models inside room template")]
        //    public static void FixModelsInsideRoomTemplate()
        //    {
        //        var currentPath = "Assets/Edgar/Examples/Grid3D/Dungeon1/Room templates/Bedroom.prefab";
        //        var newPath = "Assets/Edgar/Examples/Grid3D/Dungeon1/Room templates/A.prefab";
        //        //currentPath = newPath;
        //        FixModelsInsideRoomTemplate(currentPath, newPath);
        //    }

        //    [MenuItem("Edgar debug/Fix models inside all prefabs")]
        //    public static void FixModelsInsideAllPrefabs()
        //    {
        //        var guids = AssetDatabase.FindAssets("", new[] { "Assets/Edgar/Examples/Grid3D/Dungeon1" });

        //        foreach (var guid in guids.Distinct())
        //        {
        //            var path = AssetDatabase.GUIDToAssetPath(guid);

        //            if (!path.EndsWith(".prefab"))
        //            {
        //                continue;
        //            }

        //            if (path.StartsWith("Assets/Edgar/Examples/Grid3D/Dungeon1/Prefabs/"))
        //            {
        //                continue;
        //            }

        //            Debug.Log($"----> {path}");
        //            FixModelsInsideRoomTemplate(path, path);
        //        }
        //    }

        //    public static void FixModelsInsideRoomTemplate(string path, string newPath)
        //    {
        //        var prefabRoot = PrefabUtility.LoadPrefabContents(path);

        //        FixModelsInsideGameObject(prefabRoot.transform);

        //        PrefabUtility.SaveAsPrefabAsset(prefabRoot, newPath);
        //        PrefabUtility.UnloadPrefabContents(prefabRoot);
        //    }

        //    private static void FixModelsInsideGameObject(Transform transform)
        //    {
        //        var isPrefab = PrefabUtility.GetNearestPrefabInstanceRoot(transform.gameObject) == transform.gameObject;

        //        //Debug.Log($"{transform.name}, {isPrefab}");

        //        if (isPrefab)
        //        {
        //            var originalModelPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(transform.gameObject);
        //            var modelPath = AssetDatabase.GetAssetPath(originalModelPrefab);

        //            if (modelPath.EndsWith(".fbx") == false)
        //            {
        //                return;
        //            }

        //            // TODO: object isActive is not respected
        //            var prefabPath = modelPath.Replace(".fbx", ".prefab").Replace("Models", "Prefabs");
        //            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        //            var prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        //            prefabInstance.transform.parent = transform.parent;
        //            prefabInstance.transform.localPosition = transform.localPosition;
        //            prefabInstance.transform.localRotation = transform.localRotation;
        //            prefabInstance.transform.localScale = transform.localScale;

        //            Object.DestroyImmediate(transform.gameObject);
        //            Debug.Log($"Original prefab: {originalModelPrefab.name}, path: {modelPath}");
        //        }
        //        else
        //        {
        //            foreach (var child in transform.Cast<Transform>().ToList())
        //            {
        //                FixModelsInsideGameObject(child);
        //            }
        //        }
        //    }
        //    #endif
    }
}
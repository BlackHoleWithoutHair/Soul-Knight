using System.Linq;
using UnityEditor;
#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif
using UnityEngine;

namespace Edgar.Unity
{
    public class RoomTemplateSaveHandlerGrid3D
    {
        private static RoomTemplateSaveHandlerGrid3D instance;

        [InitializeOnLoadMethod]
        private static void Load()
        {
            instance = new RoomTemplateSaveHandlerGrid3D();
            PrefabStage.prefabSaving += instance.OnSaving;
        }

        private void OnSaving(GameObject go)
        {
            // Check if the game object is a room template
            var roomTemplateSettings = go.GetComponent<RoomTemplateSettingsGrid3D>();
            if (roomTemplateSettings == null)
            {
                return;
            }

            // Check that the outline should be computed inside editor
            var generatorSettings = roomTemplateSettings.GeneratorSettings;
            if (generatorSettings == null || generatorSettings.OutlineComputationMode != RoomTemplateOutlineComputationModeGrid3D.InsideEditor)
            {
                roomTemplateSettings.Outline = null;
                return;
            }

            RecalculateOutline(roomTemplateSettings);
        }

        private static void RecalculateOutline(RoomTemplateSettingsGrid3D roomTemplateSettings)
        {
            Debug.Log($"Computing room template outline for: {roomTemplateSettings.name}");
            roomTemplateSettings.Outline = roomTemplateSettings
                .ComputeOutline()
                .GetPoints()
                .Select(x => new SerializableVector3Int(x.X, x.Y, 0))
                .ToList();
        }

        public static void RecalculateOutlines(GeneratorSettingsGrid3D generatorSettings, bool remove)
        {
            var guids = AssetDatabase.FindAssets("t:Prefab");

            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);

                var contentsRoot = PrefabUtility.LoadPrefabContents(assetPath);
                var roomTemplateSettings = contentsRoot.GetComponent<RoomTemplateSettingsGrid3D>();

                if (roomTemplateSettings != null && roomTemplateSettings.GeneratorSettings == generatorSettings)
                {
                    if (remove)
                    {
                        roomTemplateSettings.Outline = null;
                    }
                    else
                    {
                        RecalculateOutline(roomTemplateSettings);
                    }

                    PrefabUtility.SaveAsPrefabAsset(contentsRoot, assetPath);
                }

                PrefabUtility.UnloadPrefabContents(contentsRoot);
            }
        }
    }
}
using Edgar.Unity;
using Edgar.Unity.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Edgar.Editor.Grid3D
{
    [CustomEditor(typeof(GeneratorSettingsGrid3D))]
    public class GeneratorSettingsGrid3DInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var generatorSettings = (GeneratorSettingsGrid3D)target;

            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;

            DrawDefaultInspector();

            serializedObject.Update();

            EditorGUILayout.Space();

            if (EdgarSettings.instance.Grid3D.DefaultGeneratorSettings == generatorSettings)
            {
                EditorGUILayout.HelpBox($"Is default: {true}\n\nThis generator settings object is currently set as the default one when creating new room templates and doors prefabs.", MessageType.Info);
                if (GUILayout.Button("Reset default generator settings"))
                {
                    EdgarSettings.instance.Grid3D.DefaultGeneratorSettings = null;
                    EdgarSettings.instance.Save();
                }
            }
            else
            {
                EditorGUILayout.HelpBox($"Is default: {false}\n\nYou can mark this generator settings object to be the default one when creating new room templates and doors prefabs.", MessageType.Info);
                if (GUILayout.Button("Mark as default generator settings"))
                {
                    EdgarSettings.instance.Grid3D.DefaultGeneratorSettings = generatorSettings;
                    EdgarSettings.instance.Save();
                }
            }

            EditorGUILayout.Space();

            // TODO(Grid3D): How to handle changes of computation mode? Should we always recalculate outlines after every change?
            if (generatorSettings.OutlineComputationMode == RoomTemplateOutlineComputationModeGrid3D.InsideEditor)
            {
                EditorGUILayout.HelpBox($"This instance of generator settings is configured so that room template outlines are computed only in editor. This is recommended only for advanced users.\n\nThe operations below might take some time depending on the size of your project.", MessageType.Warning);

                if (GUILayout.Button("Recalculate outlines"))
                {
                    RoomTemplateSaveHandlerGrid3D.RecalculateOutlines(generatorSettings, false);
                }

                if (GUILayout.Button("Remove outlines"))
                {
                    RoomTemplateSaveHandlerGrid3D.RecalculateOutlines(generatorSettings, true);
                }
            }


            EditorGUIUtility.labelWidth = 0;
        }
    }
}
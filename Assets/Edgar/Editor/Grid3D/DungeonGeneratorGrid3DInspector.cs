using UnityEditor;
using UnityEngine;

namespace Edgar.Unity.Editor.Grid3D
{
    [CustomEditor(typeof(DungeonGeneratorGrid3D), true)]
    public class DungeonGeneratorGrid3DInspector : UnityEditor.Editor
    {
        private ReorderableList customPostProcessingTasksList;

        public void OnEnable()
        {
            customPostProcessingTasksList = new ReorderableList(new UnityEditorInternal.ReorderableList(serializedObject,
                serializedObject.FindProperty(nameof(DungeonGeneratorGrid3D.CustomPostProcessingTasks)),
                true, true, true, true), "Custom post-processing tasks");
        }

        public override void OnInspectorGUI()
        {
            var generator = (DungeonGeneratorGrid3D)target;

            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2f;

            EditorGUILayout.LabelField("Input config", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DungeonGeneratorGrid3D.InputType)));
            switch (generator.InputType)
            {
                case DungeonGeneratorInputTypeGrid2D.CustomInput:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DungeonGeneratorGrid3D.CustomInput)));
                    break;
                case DungeonGeneratorInputTypeGrid2D.FixedLevelGraph:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DungeonGeneratorGrid3D.FixedLevelGraphConfig)));
                    break;
            }

            EditorGUILayout.LabelField("Generator config", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DungeonGeneratorGrid3D.GeneratorConfig)));

            if (generator.GeneratorConfig.GeneratorSettings == null)
            {
                EditorGUILayout.HelpBox($"Please assign the {nameof(DungeonGeneratorConfigGrid3D.GeneratorSettings)} field.", MessageType.Error);
            }

            EditorGUILayout.LabelField("Post-processing config", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DungeonGeneratorGrid3D.PostProcessingConfig)));

            if (generator.DisableCustomPostProcessing)
            {
                EditorGUILayout.HelpBox($"Custom post-processing tasks are temporarily disabled. Uncheck the \"{nameof(DungeonGeneratorGrid3D.DisableCustomPostProcessing)}\" checkbox to enable them again.", MessageType.Warning);
            }
            else
            {
                customPostProcessingTasksList.DoLayoutList();
            }

            EditorGUILayout.LabelField("Other", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DungeonGeneratorGrid3D.UseRandomSeed)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DungeonGeneratorGrid3D.RandomGeneratorSeed)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DungeonGeneratorGrid3D.GenerateOnStart)));

            EditorGUILayout.HelpBox("If you have problems with the performance of the generator, you can enable diagnostics what will run after a level is generated and print results to the console. Do not use in production.", MessageType.Info);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DungeonGeneratorBaseGrid2D.EnableDiagnostics)));

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DungeonGeneratorGrid3D.DisableCustomPostProcessing)));

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate"))
            {
                generator.Generate();
            }

            EditorGUIUtility.labelWidth = 0;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
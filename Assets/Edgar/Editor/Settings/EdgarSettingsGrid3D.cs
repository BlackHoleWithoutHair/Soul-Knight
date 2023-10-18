using System;
using UnityEditor;
using UnityEngine;

namespace Edgar.Unity.Editor
{
    [Serializable]
    public class EdgarSettingsGrid3D
    {
        public GeneratorSettingsGrid3D DefaultGeneratorSettings;

        internal class Inspector : EdgarSettingsInspectorBase
        {
            public Inspector(SerializedObject serializedObject) : base(serializedObject, nameof(EdgarSettings.Grid3D))
            {
            }

            public override void OnGUI()
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                Show = EditorGUILayout.Foldout(Show, "Grid3D settings");
                if (Show)
                {
                    EditorGUILayout.PropertyField(Property(nameof(DefaultGeneratorSettings)));
                }
                GUILayout.EndVertical();
            }
        }
    }
}
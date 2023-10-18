using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Edgar.Unity.Editor
{
    [CustomEditor(typeof(LevelGraph))]
    public class LevelGraphInspector : UnityEditor.Editor
    {
        private bool defaultRoomTemplatesFoldout;
        private bool corridorRoomTemplatesFoldout;

        private List<Type> derivedRoomTypes;
        private List<Type> derivedConnectionTypes;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var foldoutStyle = new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold };

            defaultRoomTemplatesFoldout = EditorGUILayout.Foldout(defaultRoomTemplatesFoldout, "Default room templates", foldoutStyle);

            if (defaultRoomTemplatesFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty(nameof(LevelGraph.DefaultIndividualRoomTemplates)),
                    new GUIContent("Room Templates"),
                    true);
                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty(nameof(LevelGraph.DefaultRoomTemplateSets)),
                    new GUIContent("Room Templates Sets"),
                    true);
                EditorGUI.indentLevel--;
            }

            corridorRoomTemplatesFoldout = EditorGUILayout.Foldout(corridorRoomTemplatesFoldout, "Corridor room templates", foldoutStyle);

            if (corridorRoomTemplatesFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(
                        nameof(LevelGraph.CorridorIndividualRoomTemplates)),
                    new GUIContent("Room Templates"),
                    true);
                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty(nameof(LevelGraph.CorridorRoomTemplateSets)),
                    new GUIContent("Room Templates Sets"),
                    true);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(nameof(LevelGraph.IsDirected)),
                new GUIContent("Is Directed"),
                true);

            EditorGUILayout.LabelField("Custom room and connection types", EditorStyles.boldLabel);

            var derivedRoomTypes = GetDerivedRoomTypes();
            var currentRoomType = serializedObject.FindProperty(nameof(LevelGraph.RoomType)).stringValue;
            var selectedRoomIndex = derivedRoomTypes.FindIndex(x => x.FullName == currentRoomType);
            selectedRoomIndex = selectedRoomIndex == -1 ? derivedRoomTypes.IndexOf(typeof(Room)) : selectedRoomIndex;
            var roomOptions = derivedRoomTypes.Select(x => $"{x.Name} ({x.Namespace})").ToArray();
            selectedRoomIndex = EditorGUILayout.Popup("Room type", selectedRoomIndex, roomOptions);
            serializedObject.FindProperty(nameof(LevelGraph.RoomType)).stringValue = derivedRoomTypes[selectedRoomIndex].FullName;

            var derivedConnectionTypes = GetDerivedConnectionTypes();
            var currentConnectionType = serializedObject.FindProperty(nameof(LevelGraph.ConnectionType)).stringValue;
            var selectedConnectionIndex = derivedConnectionTypes.FindIndex(x => x.FullName == currentConnectionType);
            selectedConnectionIndex = selectedConnectionIndex == -1 ? derivedConnectionTypes.IndexOf(typeof(Connection)) : selectedConnectionIndex;
            var connectionOptions = derivedConnectionTypes.Select(x => $"{x.Name} ({x.Namespace})").ToArray();
            selectedConnectionIndex = EditorGUILayout.Popup("Connection type", selectedConnectionIndex, connectionOptions);
            serializedObject.FindProperty(nameof(LevelGraph.ConnectionType)).stringValue = derivedConnectionTypes[selectedConnectionIndex].FullName;

            if (derivedRoomTypes[selectedRoomIndex] == typeof(Room) && derivedConnectionTypes[selectedConnectionIndex] == typeof(Connection))
            {
                EditorGUILayout.HelpBox(
                    "Default room or connection types are selected. It is not possible to change this easily after you add rooms and connections to the graph.",
                    MessageType.Warning);
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Open graph editor"))
            {
                OpenWindow((LevelGraph)target);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private List<Type> GetDerivedRoomTypes()
        {
            if (derivedRoomTypes == null)
            {
                derivedRoomTypes = ProUtils.FindDerivedTypes(typeof(RoomBase));
            }

            return derivedRoomTypes;
        }

        private List<Type> GetDerivedConnectionTypes()
        {
            if (derivedConnectionTypes == null)
            {
                derivedConnectionTypes = ProUtils.FindDerivedTypes(typeof(ConnectionBase));
            }

            return derivedConnectionTypes;
        }

        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var assetPath = AssetDatabase.GetAssetPath(instanceID);
            var levelGraph = AssetDatabase.LoadAssetAtPath<LevelGraph>(assetPath);

            if (levelGraph != null)
            {
                OpenWindow(levelGraph);

                return true;
            }

            return false;
        }

        private static void OpenWindow(LevelGraph levelGraph)
        {
            var type = Type.GetType("UnityEditor.GameView,UnityEditor");
            var window = EditorWindow.GetWindow<LevelGraphEditor>("Graph editor", type);
            window.Initialize(levelGraph);
            window.Show();
        }
    }
}
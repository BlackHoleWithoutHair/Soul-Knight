using System.Text;
using Edgar.Unity.Diagnostics;
using UnityEditor;
#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif
using UnityEngine;

namespace Edgar.Unity.Editor
{
    [CustomEditor(typeof(RoomTemplateSettingsGrid3D))]
    public class RoomTemplateSettingsGrid3DInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            var roomTemplate = (RoomTemplateSettingsGrid3D)target;
            var validityCheck = RoomTemplateDiagnosticsGrid3D.CheckAll(roomTemplate.gameObject);

            if (!validityCheck.HasErrors)
            {
                EditorGUILayout.HelpBox("The room template is valid.", MessageType.Info);
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine("There are some problems with the room template:");

                var errors = string.Join("\n", validityCheck.Errors);
                sb.Append(errors);

                EditorGUILayout.HelpBox(sb.ToString(), MessageType.Error);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            RemoveOnSceneGUIDelegate();
            AddOnSceneGUIDelegate();
        }

        private void OnSceneGUIPersistent(SceneView sceneView)
        {
            if (target == null || PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                RemoveOnSceneGUIDelegate();
                return;
            }

            ShowStatus();
        }

        private void ShowStatus()
        {
            var roomTemplate = (RoomTemplateSettingsGrid3D)target;
            var originalBackground = GUI.backgroundColor;

            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(10, 10, 180, 100));
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Label("Room template status", EditorStyles.boldLabel);

            var hasComponents = RoomTemplateDiagnosticsGrid3D.CheckComponents(roomTemplate.gameObject);
            var isOutlineValid = hasComponents.IsSuccessful && roomTemplate.ComputeOutline() != null;
            var outlineText = isOutlineValid ? "valid" : "<color=#870526ff>invalid</color>";
            var areDoorsValid = false;
            var doorsText = "N/A";

            if (isOutlineValid)
            {
                var doorsCheck = RoomTemplateDiagnosticsGrid3D.CheckDoors(roomTemplate.gameObject);
                areDoorsValid = !doorsCheck.HasErrors;
                doorsText = !doorsCheck.HasErrors ? "valid" : "<color=#870526ff>invalid</color>";
            }

            GUILayout.Label($"Outline: <b>{outlineText}</b>", new GUIStyle(EditorStyles.label) { richText = true });
            GUILayout.Label($"Doors: <b>{doorsText}</b>", new GUIStyle(EditorStyles.label) { richText = true });

            if (!isOutlineValid || !areDoorsValid)
            {
                GUILayout.Label($"<size=9>See the Room template settings component for details</size>", new GUIStyle(EditorStyles.label) { richText = true, wordWrap = true });
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
            Handles.EndGUI();

            GUI.backgroundColor = originalBackground;
        }

        private void AddOnSceneGUIDelegate()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUIPersistent;
#else
            SceneView.onSceneGUIDelegate += OnSceneGUIPersistent;
#endif
        }

        private void RemoveOnSceneGUIDelegate()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUIPersistent;
#else
            SceneView.onSceneGUIDelegate -= OnSceneGUIPersistent;
#endif
        }
    }
}
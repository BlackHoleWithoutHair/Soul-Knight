using UnityEditor;
using UnityEngine;

namespace Edgar.Unity.Editor.Grid3D
{
    [CustomEditor(typeof(DoorHandlerGrid3D))]
    public class DoorHandlerGrid3DInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var doorHandler = (DoorHandlerGrid3D)target;

            var doorPrefab = doorHandler.gameObject;
            var hierarchyRoot = doorPrefab.transform.root.gameObject;
            var isInsideDoorPrefab = doorPrefab == hierarchyRoot;

            DrawDefaultInspector();

            HandleRotations(doorHandler);

            if (doorHandler.GeneratorSettings == null)
            {
                EditorGUILayout.HelpBox($"Please assign the {nameof(doorHandler.GeneratorSettings)} field.", MessageType.Error);
            }
            else if (!isInsideDoorPrefab)
            {
                HandleMisaligned(doorHandler);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void HandleRotations(DoorHandlerGrid3D doorHandler)
        {
            if (doorHandler.IsDirectionValid())
            {
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Rotate -90"))
                {
                    doorHandler.Rotate90(false);
                    SceneView.RepaintAll();
                    EditorUtility.SetDirty(target);
                }

                if (GUILayout.Button("Rotate +90"))
                {
                    doorHandler.Rotate90(true);
                    SceneView.RepaintAll();
                    EditorUtility.SetDirty(target);
                }

                GUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.HelpBox($"The rotation of this door is out of sync with the internal rotation. Please always use the Rotate buttons when rotating the door. Click the button below to sync the rotation.", MessageType.Error);

                if (GUILayout.Button("Sync rotation"))
                {
                    doorHandler.SyncRotation();
                    SceneView.RepaintAll();
                    EditorUtility.SetDirty(target);
                }
            }
        }

        private void HandleMisaligned(DoorHandlerGrid3D doorHandler)
        {
            var generatorSettings = doorHandler.GeneratorSettings;
            var doorPositionInterpolated = generatorSettings.LocalToCellInterpolated(doorHandler.transform.localPosition);
            var doorPositionSnapped = GridUtilsGrid3D.SnapInterpolatedToCell(doorPositionInterpolated);

            if (GridUtilsGrid3D.IsSnappedToCell(doorPositionInterpolated, doorPositionSnapped))
            {
                return;
            }

            var closestSnapCell = GridUtilsGrid3D.SnapInterpolatedToCellRound(doorPositionInterpolated);
            var closestSnapLocal = generatorSettings.CellToLocal(closestSnapCell);

            EditorGUILayout.HelpBox($"This door is not correctly positioned on the grid. The closest valid position is {closestSnapLocal}.", MessageType.Error);

            if (GUILayout.Button("Snap to grid"))
            {
                doorHandler.transform.localPosition = closestSnapLocal;
                SceneView.RepaintAll();
                EditorUtility.SetDirty(target);
            }
        }
    }
}
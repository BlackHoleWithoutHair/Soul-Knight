using Edgar.Geometry;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Edgar.Unity.Editor
{
    public class HybridDoorModeInspector : ManualDoorModeInspectorBase
    {
        public HybridDoorModeInspector(SerializedObject serializedObject, DoorsGrid2D doors, SerializedProperty serializedProperty) : base(serializedObject, doors, serializedProperty)
        {
        }

        protected override void DrawAllDoors()
        {
            var gameObject = doors.transform.gameObject;
            var grid = gameObject.GetComponentInChildren<Grid>();

            for (var i = 0; i < doors.HybridDoorModeData.DoorLines.Count; i++)
            {
                var doorLine = doors.HybridDoorModeData.DoorLines[i];
                var color = doorLine.Socket != null ? doorLine.Socket.GetColor() : Color.red;
                var label = $"Id: {i}";

                if (doorLine.Direction != DoorDirection.Undirected)
                {
                    label += doorLine.Direction == DoorDirection.Entrance ? "\nIn" : "\nOut";
                }

                DoorsInspectorUtils.DrawDoorLine(doorLine, grid, color, label);
            }
        }

        protected override void RemoveDoor(Vector3Int position)
        {
            for (int i = doors.HybridDoorModeData.DoorLines.Count - 1; i >= 0; i--)
            {
                var door = doors.HybridDoorModeData.DoorLines[i];
                var orthogonalLine = new OrthogonalLine(door.From, door.To);

                if (orthogonalLine.Contains(position) != -1)
                {
                    Undo.RecordObject(doors, "Deleted door position");
                    doors.HybridDoorModeData.DoorLines.RemoveAt(i);
                    EditorUtility.SetDirty(doors);
                }
            }
        }

        protected override void DrawPreview(Vector3Int from, Vector3Int to)
        {
            var gameObject = doors.transform.gameObject;
            var grid = gameObject.GetComponentInChildren<Grid>();
            var length = doors.HybridDoorModeData.DefaultLength;
            var doorLine = new DoorLineGrid2D()
            {
                From = from,
                To = to,
                Length = length,
            };

            var color = Color.red;

            DoorsInspectorUtils.DrawDoorLine(doorLine, grid, color);
        }

        protected override void AddDoor(Vector3Int from, Vector3Int to)
        {
            var length = doors.HybridDoorModeData.DefaultLength;
            var doorLine = new DoorLineGrid2D()
            {
                From = from,
                To = to,
                Length = length,
                Socket = doors.DefaultSocket,
                Direction = doors.DefaultDirection,
            };
            var line = new OrthogonalLineGrid2D(from.ToCustomIntVector2(), to.ToCustomIntVector2());

            if (doors.HybridDoorModeData.DoorLines.Any(x => x == doorLine))
            {
                return;
            }

            if (line.Length >= length - 1)
            {
                Undo.RecordObject(doors, "Added door positions");

                doors.HybridDoorModeData.DoorLines.Add(doorLine);

                EditorUtility.SetDirty(doors);
            }
        }

        protected override SerializedProperty GetDoorsListProperty()
        {
            return serializedProperty.FindPropertyRelative(nameof(HybridDoorModeDataGrid2D.DoorLines));
        }

        protected override void DeleteAllDoors()
        {
            Undo.RecordObject(doors, "Delete all door positions");

            doors.HybridDoorModeData.DoorLines.Clear();

            EditorUtility.SetDirty(doors);
        }

        protected override void ShowAdditionalFields()
        {
            EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative(nameof(HybridDoorModeDataGrid2D.DefaultLength)), new GUIContent("New door length"));
        }
    }
}
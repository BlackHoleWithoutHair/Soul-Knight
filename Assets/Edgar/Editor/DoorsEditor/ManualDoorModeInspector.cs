using UnityEditor;
using UnityEngine;

namespace Edgar.Unity.Editor
{
    public class ManualDoorModeInspector : ManualDoorModeInspectorBase
    {
        public ManualDoorModeInspector(SerializedObject serializedObject, DoorsGrid2D doors, SerializedProperty serializedProperty) : base(serializedObject, doors, serializedProperty)
        {
        }

        protected override SerializedProperty GetDoorsListProperty()
        {
            return serializedProperty.FindPropertyRelative(nameof(ManualDoorModeDataGrid2D.DoorsList));
        }

        protected override void DeleteAllDoors()
        {
            Undo.RecordObject(doors, "Delete all door positions");

            doors.ManualDoorModeData.DoorsList.Clear();

            EditorUtility.SetDirty(doors);
        }

        protected override void DrawAllDoors()
        {
            var gameObject = doors.transform.gameObject;
            var grid = gameObject.GetComponentInChildren<Grid>();

            for (var i = 0; i < doors.ManualDoorModeData.DoorsList.Count; i++)
            {
                var door = doors.ManualDoorModeData.DoorsList[i];
                var color = door.Socket != null ? door.Socket.GetColor() : Color.red;
                var label = $"Id: {i}";

                if (door.Direction != DoorDirection.Undirected)
                {
                    label += door.Direction == DoorDirection.Entrance ? "\nIn" : "\nOut";
                }

                DrawDoor(grid, door.From.RoundToUnityIntVector3(), door.To.RoundToUnityIntVector3(), color, label);
            }
        }

        protected override void RemoveDoor(Vector3Int position)
        {
            for (int i = doors.ManualDoorModeData.DoorsList.Count - 1; i >= 0; i--)
            {
                var door = doors.ManualDoorModeData.DoorsList[i];
                var orthogonalLine = new OrthogonalLine(door.From.RoundToUnityIntVector3(), door.To.RoundToUnityIntVector3());

                if (orthogonalLine.Contains(position) != -1)
                {
                    Undo.RecordObject(doors, "Deleted door position");
                    doors.ManualDoorModeData.DoorsList.RemoveAt(i);
                    EditorUtility.SetDirty(doors);
                }
            }
        }

        protected override void DrawPreview(Vector3Int from, Vector3Int to)
        {
            var gameObject = doors.transform.gameObject;
            var grid = gameObject.GetComponentInChildren<Grid>();
            DrawDoor(grid, from, to, Color.red);
        }

        private void DrawDoor(Grid grid, Vector3Int from, Vector3Int to, Color color, string label = null)
        {
            var length = new OrthogonalLine(from, to).Length;
            var doorLine = new DoorLineGrid2D()
            {
                From = from,
                To = to,
                Length = length,
            };

            DoorsInspectorUtils.DrawDoorLine(doorLine, grid, color, label);
        }

        protected override void AddDoor(Vector3Int from, Vector3Int to)
        {
            var newDoor = new DoorGrid2D()
            {
                From = from,
                To = to,
                Socket = doors.DefaultSocket,
                Direction = doors.DefaultDirection,
            };

            if (!doors.ManualDoorModeData.DoorsList.Contains(newDoor))
            {
                Undo.RecordObject(doors, "Added door position");

                doors.ManualDoorModeData.DoorsList.Add(newDoor);

                EditorUtility.SetDirty(doors);
            }
        }
    }
}
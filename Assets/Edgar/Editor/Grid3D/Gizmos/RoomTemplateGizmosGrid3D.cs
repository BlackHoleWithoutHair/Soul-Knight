using System;
using System.Collections.Generic;
using System.Linq;
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
    public static class RoomTemplateGizmosGrid3D
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void DrawRoomTemplateOutline(RoomTemplateSettingsGrid3D roomTemplate, GizmoType gizmoType)
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                return;
            }

            if (RoomTemplateDiagnosticsGrid3D.CheckComponents(roomTemplate.gameObject).HasErrors)
            {
                return;
            }

            var blocks = RoomTemplateLoaderGrid3D.GetBlocks(roomTemplate.gameObject, roomTemplate);
            var outline = roomTemplate.ComputeOutline();

            if (outline == null)
            {
                return;
            }

            var minHeight = (float)blocks.Min(x => x.y);
            var minHeightVector = new Vector3(0, minHeight, 0);
            var minHeightVectorTransformed = roomTemplate.GeneratorSettings.CellToLocal(minHeightVector);
            minHeight = minHeightVectorTransformed.y;

            var originalColor = Handles.color;
            var color = Color.yellow;
            Handles.color = color;

            var lines = outline.GetLines();
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var point1 = (Vector3)line.From.To3DSpace();
                var point2 = (Vector3)line.To.To3DSpace();

                point1 = roomTemplate.GeneratorSettings.CellToLocal(point1);
                point2 = roomTemplate.GeneratorSettings.CellToLocal(point2);

                point1.y = minHeight;
                point2.y = minHeight;

                var offsetScale = 0.075f;
                var perpendicularOffset = (Vector3)line.GetDirectionVector().RotateAroundCenter(-90).To3DSpace() * offsetScale;
                var directionOffset = (Vector3)line.GetDirectionVector().To3DSpace() * offsetScale;

                Handles.DrawLine(point1, point2);

                var nextLine = lines[(i + 1) % lines.Count];
                var nextLineConvex = line.GetDirectionVector().RotateAroundCenter(90) == nextLine.GetDirectionVector();
                var point2WithOffset = nextLineConvex
                    ? point2 + perpendicularOffset + directionOffset
                    : point2 + perpendicularOffset - directionOffset;

                var previousLine = lines[i == 0 ? lines.Count - 1 : i - 1];
                var previousLineConvex = previousLine.GetDirectionVector().RotateAroundCenter(90) == line.GetDirectionVector();
                var point1WithOffset = previousLineConvex
                    ? point1 + perpendicularOffset - directionOffset
                    : point1 + perpendicularOffset + directionOffset;

                Handles.DrawLine(point1WithOffset, point2WithOffset);
            }

            Handles.color = originalColor;
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
        private static void DrawDoors(DoorHandlerGrid3D doorHandler, GizmoType gizmoType)
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                return;
            }

            if (doorHandler.GeneratorSettings == null)
            {
                return;
            }

            var generatorSettings = doorHandler.GeneratorSettings;

            var doorPrefab = doorHandler.gameObject;
            var hierarchyRoot = doorPrefab.transform.root.gameObject;
            var isInsideDoorPrefab = doorPrefab == hierarchyRoot;

            var door = doorHandler.GetDoorLine(false);

            var doorPositionInterpolated = generatorSettings.LocalToCellInterpolated(doorHandler.transform.localPosition);
            var doorPositionSnapped = GridUtilsGrid3D.SnapInterpolatedToCell(doorPositionInterpolated);
            var doorPositionSnappedOffset = doorPositionInterpolated - doorPositionSnapped;

            DrawTileBounds(door, generatorSettings, doorPositionSnappedOffset);
            DrawOutlineWall(door, generatorSettings, doorPositionSnappedOffset);
            DrawInAndOutArrow(door, generatorSettings, doorHandler.Direction);

            if (isInsideDoorPrefab)
            {
                DrawBlockerPosition(door, generatorSettings, doorPositionSnappedOffset);
            }
        }

        private static void DrawInAndOutArrow(DoorLineGrid3D door, GeneratorSettingsGrid3D generatorSettings, DoorDirection doorDirection)
        {
            if (doorDirection == DoorDirection.Undirected)
            {
                return;
            }

            var originalColor = Handles.color;
            Handles.color = GetDoorColor(door.Socket, 0.45f);

            var gridBounds = door.Get3DBounds();
            var localCenter = generatorSettings.CellToLocal(gridBounds.center + (gridBounds.size.y / 2f) * Vector3.up);

            var center = localCenter;
            var unitLength = Math.Min(generatorSettings.CellSize.x, generatorSettings.CellSize.z);

            var direction = unitLength * (Vector3)door.GetFacingDirection();

            if (doorDirection == DoorDirection.Entrance)
            {
                direction *= -1;
            }

            var perpendicular = unitLength * (Vector3)door.DirectionVector;

            var rectangleDirection = 0.2f * direction;
            var rectanglePerpendicular = 0.1f * perpendicular;

            var triangleDirection = 0.2f * direction;
            var trianglePerpendicular = 0.25f * perpendicular;

            center -= 0.5f * rectangleDirection;

            var points = new List<Vector3>
            {
                center - rectangleDirection - rectanglePerpendicular,
                center + rectangleDirection - rectanglePerpendicular,
                center + rectangleDirection + rectanglePerpendicular,
                center - rectangleDirection + rectanglePerpendicular
            };
            Handles.DrawAAConvexPolygon(points.ToArray());
            center += 0.99f * rectangleDirection;
            Handles.DrawAAConvexPolygon(center - trianglePerpendicular, center + triangleDirection, center + trianglePerpendicular);

            Handles.color = originalColor;
        }

        private static void DrawBlockerPosition(DoorLineGrid3D door, GeneratorSettingsGrid3D generatorSettings, Vector3 offset)
        {
            var oneBlockDoor = new DoorLineGrid3D(door.From, door.From, 1, door.Height, door.DoorHandler, door.DirectionVector, door.Socket, door.Direction);

            var bounds = oneBlockDoor.Get3DBounds();
            var smallerBounds = new Bounds(bounds.center, bounds.size - new Vector3(0.01f, 0.01f, 0.01f));

            var originalGizmoColor = Gizmos.color;

            Gizmos.color = new Color(0, 1, 0, 0.45f);
            Gizmos.DrawWireCube(generatorSettings.CellToLocal(smallerBounds.center + offset), generatorSettings.CellToLocal(smallerBounds.size));
            Gizmos.color = originalGizmoColor;
        }

        private static void DrawTileBounds(DoorLineGrid3D door, GeneratorSettingsGrid3D generatorSettings, Vector3 offset)
        {
            var bounds = door.Get3DBounds();
            var smallerBounds = new Bounds(bounds.center, bounds.size - new Vector3(0.01f, 0.01f, 0.01f));

            var originalGizmoColor = Gizmos.color;

            Gizmos.color = GetDoorColor(door.Socket, 0.35f);
            Gizmos.DrawCube(generatorSettings.CellToLocal(smallerBounds.center + offset), generatorSettings.CellToLocal(smallerBounds.size));
            Gizmos.color = originalGizmoColor;
        }

        private static void DrawOutlineWall(DoorLineGrid3D door, GeneratorSettingsGrid3D generatorSettings, Vector3 offset, float depth = 0.1f)
        {
            var bounds = door.Get3DBounds(false);
            var facingDirection = (Vector3)door.GetFacingDirection();
            var positiveDirection = facingDirection.x < 0 || facingDirection.z < 0 ? -facingDirection : facingDirection;

            var center = bounds.center + facingDirection / 2 + facingDirection * depth / 2;
            var size = bounds.size - positiveDirection * (1 - depth);

            var originalGizmoColor = Gizmos.color;
            Gizmos.color = GetDoorColor(door.Socket, 0.75f);
            Gizmos.DrawCube(generatorSettings.CellToLocal(center + offset), generatorSettings.CellToLocal(size));
            Gizmos.color = originalGizmoColor;
        }

        //private static void DrawFacingDirection(DoorGrid3D door, float depth = 0.1f)
        //{
        //    var bounds = door.Get3DBounds();
        //    var facingDirection = (Vector3)door.GetFacingDirection();

        //    var center = bounds.center + facingDirection / 2;

        //    var originalGizmoColor = Gizmos.color;
        //    Gizmos.color = new Color(1, 0, 0, 0.5f);
        //    Gizmos.DrawLine(center, center + facingDirection / 4);
        //    Gizmos.color = originalGizmoColor;
        //}

        private static Color GetDoorColor(DoorSocketBase socket, float opacity = 1)
        {
            if (socket == null)
            {
                return new Color(1, 0, 0, opacity);
            }

            var color = socket.GetColor();

            return new Color(color.r, color.g, color.b, opacity);
        }
    }
}
using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Grid2D;
using Edgar.GraphBasedGenerator.Grid2D.Exceptions;
using UnityEngine;

namespace Edgar.Unity.Diagnostics
{
    public static class RoomTemplateDiagnosticsGrid3D
    {
        /// <summary>
        /// Tries to compute a room template from a given game object and returns the result.
        /// </summary>
        /// <param name="roomTemplate"></param>
        /// <returns></returns>
        public static ActionResult CheckAll(GameObject roomTemplate)
        {
            RoomTemplateLoaderGrid3D.TryGetRoomTemplate(roomTemplate, null, true, false, out var _, out var result);
            return result;
        }

        /// <summary>
        /// Checks that the room template has all the necessary components.
        /// </summary>
        /// <param name="roomTemplate"></param>
        /// <returns></returns>
        public static ActionResult CheckComponents(GameObject roomTemplate)
        {
            var result = new ActionResult();

            var roomTemplateSettings = roomTemplate.GetComponent<RoomTemplateSettingsGrid3D>();
            if (roomTemplateSettings == null)
            {
                result.AddError($"The {nameof(RoomTemplateSettingsGrid3D)} component is missing on the room template game object.");
            }
            else
            {
                if (roomTemplateSettings.GeneratorSettings == null)
                {
                    result.AddError($"The {nameof(RoomTemplateSettingsGrid3D.GeneratorSettings)} field must be assigned.");
                }
            }

            return result;
        }

        /// <summary>
        /// Checks the doors of the room template.
        /// </summary>
        /// <param name="outline"></param>
        /// <param name="doorMode"></param>
        /// <returns></returns>
        public static ActionResult CheckDoors(PolygonGrid2D outline, IDoorModeGrid2D doorMode)
        {
            var result = new ActionResult();

            try
            {
                var doors = doorMode.GetDoors(outline);

                if (doors.Count == 0)
                {
                    result.AddError($"There are no doors.");
                }
            }
            catch (DoorLineOutsideOfOutlineException)
            {
                result.AddError(
                    $"It seems like some of the doors are not located on the outline of the room template or are incorrectly rotated.");
            }
            catch (DuplicateDoorPositionException)
            {
                result.AddError("There are duplicate/overlapping door lines with the same door length and socket.");
            }

            return result;
        }

        /// <summary>
        /// Checks the doors of the room template.
        /// </summary>
        /// <param name="roomTemplate"></param>
        /// <returns></returns>
        public static ActionResult CheckDoors(GameObject roomTemplate)
        {
            var roomTemplateSettings = roomTemplate.GetComponent<RoomTemplateSettingsGrid3D>();
            var outline = roomTemplateSettings.ComputeOutline();

            if (!RoomTemplateLoaderGrid3D.TryGetDoors(roomTemplate, outline, out var doorLoadingResult))
            {
                return doorLoadingResult.ActionResult;
            }

            return CheckDoors(outline, doorLoadingResult.DoorMode);
        }
    }
}
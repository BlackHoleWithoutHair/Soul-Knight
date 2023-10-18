using Edgar.GraphBasedGenerator.Grid2D;
using Edgar.Unity.Diagnostics;

namespace Edgar.Unity
{
    /// <summary>
    /// Helpers class that encapsulates the result of loading doors of a room template.
    /// </summary>
    public class DoorLoadingResultGrid3D
    {
        public IDoorModeGrid2D DoorMode { get; }

        public bool HasDifferentElevations { get; }

        public ActionResult ActionResult { get; }

        public DoorLoadingResultGrid3D(IDoorModeGrid2D doorMode, bool hasDifferentElevations, ActionResult actionResult)
        {
            DoorMode = doorMode;
            HasDifferentElevations = hasDifferentElevations;
            ActionResult = actionResult;
        }
    }
}
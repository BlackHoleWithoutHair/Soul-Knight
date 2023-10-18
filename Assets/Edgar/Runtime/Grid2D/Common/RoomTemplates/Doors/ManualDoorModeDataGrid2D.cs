using Edgar.GraphBasedGenerator.Grid2D;
using System;
using System.Collections.Generic;

namespace Edgar.Unity
{
    [Serializable]
    public class ManualDoorModeDataGrid2D : IDoorModeDataGrid2D
    {
        public List<DoorGrid2D> DoorsList = new List<DoorGrid2D>();

        public IDoorModeGrid2D GetDoorMode(DoorsGrid2D doorsComponent)
        {
            var doors = new List<GraphBasedGenerator.Grid2D.DoorGrid2D>();

            foreach (var door in DoorsList)
            {
                // TODO: ugly
                var doorLine = new GraphBasedGenerator.Grid2D.DoorGrid2D(
                    door.From.RoundToUnityIntVector3().ToCustomIntVector2(),
                    door.To.RoundToUnityIntVector3().ToCustomIntVector2(),
                    door.Socket,
                    DoorsGrid2D.GetDoorType(door.Direction)
                );

                doors.Add(doorLine);
            }

            return new ManualDoorModeGrid2D(doors);
        }
    }
}
using System.Collections.Generic;

namespace Edgar.Unity
{
    public class DoorLineInfoGrid3D : DoorLineInfoBase<DoorInstanceGrid3D, DoorLineGrid3D>
    {
        public DoorHandlerGrid3D DoorHandler { get; }

        public DoorLineInfoGrid3D(DoorLineGrid3D doorLine, SerializableVector3Int direction, List<DoorInstanceGrid3D> usedDoors, DoorHandlerGrid3D doorHandler) : base(doorLine, direction, usedDoors)
        {
            DoorHandler = doorHandler;
        }

        protected override OrthogonalLine GetLine(DoorInstanceGrid3D doorInstance)
        {
            return doorInstance.DoorLine;
        }
    }
}
using System;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Class containing information about a door of a room.
    /// This class always represent a door that was used in the currently generated level.
    /// </summary>
    [Serializable]
    public class DoorInstanceGrid3D
    {
        /// <summary>
        /// Line containing all points of the door.
        /// The line is in the local coordinates relative to the room template prefab.
        /// </summary>
        public OrthogonalLine DoorLine => doorLine;

        [SerializeField]
        private OrthogonalLine doorLine;

        /// <summary>
        /// To which room is the room that contains this door connected.
        /// </summary>
        /// <remarks>
        /// This property can be null if the door is not used.
        /// </remarks>
        public RoomBase ConnectedRoom => connectedRoom;

        [SerializeField]
        private RoomBase connectedRoom;

        /// <summary>
        /// To which room instance is the room that contains this door connected.
        /// </summary>
        /// <remarks>
        /// This property is not serialized. Unfortunately, objects in Unity are serialized
        /// by value and that would make Unity try to serialize the whole graph.
        ///
        /// This property can be null if the door is not used.
        /// </remarks>
        public RoomInstanceGrid3D ConnectedRoomInstance => connectedRoomInstance;

        [NonSerialized]
        private RoomInstanceGrid3D connectedRoomInstance;

        /// <summary>
        /// Socket of the door. Null if there was no socket assigned.
        /// </summary>
        public DoorSocketBase Socket => socket;

        [SerializeField]
        private DoorSocketBase socket;

        /// <summary>
        /// Direction of the door (entrance/exit).
        /// </summary>
        public DoorDirection Direction => direction;

        [SerializeField]
        private DoorDirection direction;

        /// <summary>
        /// Corresponding door handler.
        /// </summary>
        public DoorHandlerGrid3D DoorHandler => doorHandler;

        [SerializeField]
        private DoorHandlerGrid3D doorHandler;

        public DoorInstanceGrid3D(RoomBase connectedRoom, RoomInstanceGrid3D connectedRoomInstance, DoorSocketBase socket, DoorDirection direction, DoorHandlerGrid3D doorHandler, OrthogonalLine doorLine)
        {
            this.connectedRoom = connectedRoom;
            this.connectedRoomInstance = connectedRoomInstance;
            this.socket = socket;
            this.direction = direction;
            this.doorHandler = doorHandler;
            this.doorLine = doorLine;
        }
    }
}
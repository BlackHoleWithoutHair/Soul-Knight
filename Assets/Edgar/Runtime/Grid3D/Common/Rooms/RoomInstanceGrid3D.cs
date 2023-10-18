using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Edgar.Unity
{
    [Serializable]
    public class RoomInstanceGrid3D
    {
        /// <summary>
        /// The room associated with this room instance.
        /// </summary>
        /// <remarks>
        /// The value may be null for rooms that were created on the fly (e.g. corridors) and were
        /// serialized and later deserialized, because Unity cannot serialize such ScriptableObjects
        /// outside of Unity without creating corresponding asset files.
        /// </remarks>
        public RoomBase Room => room;

        [SerializeField]
        private RoomBase room;

        /// <summary>
        /// Whether the room instance corresponds to a Room or to a Corridor.
        /// </summary>
        public bool IsCorridor => isCorridor;

        [SerializeField]
        private bool isCorridor;

        /// <summary>
        /// If this is a corridor room, this property contains the corresponding connection.
        /// Otherwise it is null.
        /// </summary>
        public ConnectionBase Connection => connection;

        [SerializeField]
        private ConnectionBase connection;

        /// <summary>
        /// Room template that was selected for a given room.
        /// </summary>
        /// <remarks>
        /// This is the original saved asset used in the level graph.
        /// </remarks>
        public GameObject RoomTemplatePrefab => roomTemplatePrefab;

        [SerializeField]
        private GameObject roomTemplatePrefab;

        /// <summary>
        /// Instance of the RoomTemplatePrefab that is correctly positioned.
        /// </summary>
        /// <remarks>
        /// This is a new instance of a corresponding room template.
        /// It is moved to a correct position and transformed/rotated.
        /// </remarks>
        public GameObject RoomTemplateInstance => roomTemplateInstance;

        [SerializeField]
        private GameObject roomTemplateInstance;

        /// <summary>
        /// Position of the room relative to the generated layout.
        /// </summary>
        public Vector3Int Position => position;

        [SerializeField]
        private Vector3Int position;

        /// <summary>
        /// List of doors together with the information to which room they are connected.
        /// </summary>
        public List<DoorInstanceGrid3D> Doors => doors;

        [SerializeField]
        private List<DoorInstanceGrid3D> doors;

        public List<DoorLineInfoGrid3D> DoorLines => doorLines;

        [SerializeField]
        private List<DoorLineInfoGrid3D> doorLines;

        /// <summary>
        /// The polygon that was used as the outline of the room.
        /// </summary>
        /// <remarks>
        /// The polygon is stored as a collection of points rather than tiles.
        /// The polygon is already correctly positioned, it is therefore not
        /// needed to add the position of the room to its points.
        /// </remarks>
        public Polygon2D OutlinePolygon => outlinePolygon;

        [SerializeField]
        private Polygon2D outlinePolygon;

        public RoomInstanceGrid3D(RoomBase room, bool isCorridor, ConnectionBase connection, GameObject roomTemplatePrefab, GameObject roomTemplateInstance, Vector3Int position, Polygon2D outlinePolygon)
        {
            this.room = room;
            this.connection = connection;
            this.roomTemplatePrefab = roomTemplatePrefab;
            this.roomTemplateInstance = roomTemplateInstance;
            this.position = position;
            this.outlinePolygon = outlinePolygon;
            this.isCorridor = isCorridor;
        }

        internal void SetDoors(List<DoorLineInfoGrid3D> doorLines)
        {
            this.doors = doorLines.SelectMany(x => x.UsedDoors).ToList();
            this.doorLines = doorLines;
        }

        internal void UpdatePosition(Vector3Int position)
        {
            this.position = position;
        }
    }
}
using Edgar.GraphBasedGenerator.Common.Doors;
using Edgar.GraphBasedGenerator.Grid2D;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    ///     Doors MonoBehaviour that is used to define doors for room templates.
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu("Edgar/Grid2D/Doors (Grid2D)")]
    public class DoorsGrid2D : VersionedMonoBehaviour
    {
        [Obsolete]
        public int DistanceFromCorners = 1;

        [Obsolete]
        public int DoorLength = 1;

        [Obsolete]
        public List<DoorGrid2D> DoorsList = new List<DoorGrid2D>();

        [Obsolete]
        public DoorSocketBase Socket;

        [HideInInspector]
        public DoorMode SelectedMode;

        public HybridDoorModeDataGrid2D HybridDoorModeData;

        public ManualDoorModeDataGrid2D ManualDoorModeData;

        public SimpleDoorModeDataGrid2D SimpleDoorModeData;

        public DoorSocketBase DefaultSocket;

        public DoorDirection DefaultDirection = DoorDirection.Undirected;

        public IDoorModeGrid2D GetDoorMode()
        {
            if (SelectedMode == DoorMode.Manual)
            {
                return ManualDoorModeData.GetDoorMode(this);
            }

            if (SelectedMode == DoorMode.Simple)
            {
                return SimpleDoorModeData.GetDoorMode(this);
            }

            if (SelectedMode == DoorMode.Hybrid)
            {
                return HybridDoorModeData.GetDoorMode(this);
            }

            throw new ArgumentException("Invalid door mode selected");
        }

        protected override int OnUpgradeSerializedData(int version)
        {
            if (version == 1)
            {
#pragma warning disable 612
                SimpleDoorModeData.DistanceFromCorners = DistanceFromCorners;
                SimpleDoorModeData.DoorLength = DoorLength;
                SimpleDoorModeData.Socket = Socket;
                ManualDoorModeData.DoorsList = DoorsList;
#pragma warning restore 612
            }

            return 2;
        }

        internal static DoorType GetDoorType(DoorDirection direction)
        {
            var type = DoorType.Undirected;

            switch (direction)
            {
                case DoorDirection.Entrance:
                    type = DoorType.Entrance;
                    break;
                case DoorDirection.Exit:
                    type = DoorType.Exit;
                    break;
            }

            return type;
        }

        public enum DoorMode
        {
            Simple = 0,
            Manual = 1,
            Hybrid = 2,
        }
    }
}
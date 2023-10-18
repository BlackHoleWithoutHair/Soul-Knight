using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// The default implementation of a connection between two rooms in a level graph.
    /// </summary>
    public class Connection : ConnectionBase
    {
        /// <summary>
        /// Room templates assigned to the connection.
        /// </summary>
        public List<GameObject> RoomTemplates = new List<GameObject>();

        public override List<GameObject> GetRoomTemplates()
        {
            return RoomTemplates;
        }
    }
}
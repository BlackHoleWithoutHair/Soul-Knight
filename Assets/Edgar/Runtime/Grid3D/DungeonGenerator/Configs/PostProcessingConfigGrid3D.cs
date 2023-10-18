using System;

namespace Edgar.Unity
{
    /// <summary>
    /// Configuration of builtin post-processing logic.
    /// </summary>
    [Serializable]
    public class PostProcessingConfigGrid3D
    {
        public bool CenterLevel = true;

        public bool ProcessConnectorsAndBlockers = true;

        /// <summary>
        /// How to handle connectors and blockers.
        /// </summary>
        [ConditionalHide(nameof(ProcessConnectorsAndBlockers))]
        public ConnectorsModeGrid3D AddConnectors = ConnectorsModeGrid3D.RoomsOnly;

        [ConditionalHide(nameof(ProcessConnectorsAndBlockers))]
        public bool AddBlockers = true;
    }
}
using System.Collections.Generic;

namespace Edgar.Unity.Diagnostics
{
    public static class GeneratorDiagnosticsGrid3D
    {
        public static List<IDiagnosticResult> Run(DungeonGeneratorPayloadGrid3D payload)
        {
            var results = new List<IDiagnosticResult>();

            results.AddRange(Run(payload.LevelDescription));
            results.Add(new TimeoutLength().Run(payload.DungeonGenerator.GeneratorConfig.Timeout));
            results.Add(new MinimumRoomDistance().Run(payload.DungeonGenerator.GeneratorConfig.MinimumRoomDistance));

            return results;
        }

        public static List<IDiagnosticResult> Run(LevelDescriptionGrid3D levelDescription)
        {
            var results = new List<IDiagnosticResult>();

            results.Add(new DifferentLengthsOfDoors().Run(levelDescription));
            results.Add(new WrongManualDoors().Run(levelDescription));
            results.Add(new NumberOfCycles().Run(levelDescription));
            results.Add(new NumberOfRooms().Run(levelDescription));

            return results;
        }
    }
}
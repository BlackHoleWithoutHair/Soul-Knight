using System.Collections;

namespace Edgar.Unity
{
    public abstract class PipelineTaskGrid3D : IPipelineTask<DungeonGeneratorPayloadGrid3D>
    {
        DungeonGeneratorPayloadGrid3D IPipelineTask<DungeonGeneratorPayloadGrid3D>.Payload { get; set; }

        internal DungeonGeneratorPayloadGrid3D Payload
        {
            get => ((IPipelineTask<DungeonGeneratorPayloadGrid3D>)this).Payload;
            set => ((IPipelineTask<DungeonGeneratorPayloadGrid3D>)this).Payload = value;
        }

        public abstract IEnumerator Process();
    }
}
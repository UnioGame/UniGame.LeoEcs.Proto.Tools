namespace UniGame.LeoEcs.Bootstrap
{
    using GameFlow.Runtime;
    using Leopotam.EcsProto;

    public interface IEcsService : IGameService
    {

        ProtoWorld World { get; }
        
        public void SetDefaultWorld(ProtoWorld world);

    }
}
namespace UniGame.LeoEcs.Bootstrap
{
    using Leopotam.EcsProto;

    public interface ILeoEcsGizmosSystem
    {
        void RunGizmosSystem(IProtoSystems systems);
    }
}
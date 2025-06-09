namespace UniGame.LeoEcs.Bootstrap
{
    using Leopotam.EcsLite;

    public interface ILeoEcsSystem : IEcsSystem
    {
        bool Enabled { get; }
    }
}
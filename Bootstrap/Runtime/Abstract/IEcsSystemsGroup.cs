namespace UniGame.LeoEcs.Bootstrap
{
    using System.Collections.Generic;
    using Leopotam.EcsLite;

    public interface IEcsSystemsGroup : IEcsFeature
    {
        IReadOnlyList<IEcsSystem> EcsSystems { get; }
    }
}
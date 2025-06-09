namespace UniGame.LeoEcs.Bootstrap
{
    using System.Collections.Generic;
    using Leopotam.EcsLite;

    public interface IEcsSystemsGroup : ILeoEcsFeature
    {
        IReadOnlyList<IEcsSystem> EcsSystems { get; }
    }
}
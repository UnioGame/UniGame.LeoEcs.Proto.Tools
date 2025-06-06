namespace UniGame.Ecs.Shared
{
    using System;
    using global::UniGame.LeoEcs.Shared.Extensions;
    using LeoEcs.Bootstrap.Runtime.Attributes;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    /// <summary>
    /// create target component on entity by triggered component
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class SelfConvertComponentSystem<TTrigger,TTarget> : IProtoRunSystem
        where TTrigger : struct
        where TTarget : struct
    {
        private ProtoWorld _world;
        private ProtoItExc _filter = It.Chain<TTrigger>().Exc<TTarget>().End();
        private ProtoPool<TTarget> _targetPool;

        public void Run()
        {
            foreach (var entity in _filter)
                _targetPool.Add(entity);
        }
    }
}
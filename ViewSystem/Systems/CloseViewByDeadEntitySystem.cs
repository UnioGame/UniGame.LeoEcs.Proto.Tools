namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Aspects;
    using Bootstrap.Runtime.Attributes;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Extensions;
    using UniGame.ViewSystem.Runtime;

    /// <summary>
    /// close View if entity is dead
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class CloseViewByDeadEntitySystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;
        
        private ProtoIt _filter = It
            .Chain<ViewEntityLifeTimeComponent>()
            .End();

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var lifeTimeComponent = ref _viewAspect.LifeTime.Get(entity);
                if (lifeTimeComponent.Value.Unpack(_world, out _))
                    continue;

                var view = lifeTimeComponent.View;
                if (!view.IsTerminated && view.Status.CurrentValue != ViewStatus.Closed)
                    view.Close();

                if (_viewAspect.LifeTime.Has(entity))
                    _viewAspect.LifeTime.Del(entity);
            }
        }
    }
}
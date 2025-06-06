namespace Game.Ecs.UI.EndGameScreens.Systems
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Modules.UnioModules.UniGame.LeoEcsLite.LeoEcs.ViewSystem.Components;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.ViewSystem.Components;
    using UniGame.LeoEcs.ViewSystem.Extensions;
    using UniGame.ViewSystem.Runtime;

    /// <summary>
    /// request to show view in container
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class ShowViewInContainerWhenSystem<TEvent,TView> : IProtoInitSystem, IProtoRunSystem
        where TEvent : struct
        where TView : IView
    {
        private ViewContainerSystemData _data;
        private ProtoWorld _world;
        private ProtoIt _eventFilter;

        private ProtoPool<ContainerViewMarker<TView>> _markerPool;

        public ShowViewInContainerWhenSystem(ViewContainerSystemData data)
        {
            _data = data;
        }
        
        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            var mask = _data.FilterMask ?? _world.Filter<TEvent>().End();
            _eventFilter = mask;
            _markerPool = _world.GetPool<ContainerViewMarker<TView>>();
        }

        public void Run()
        {
            foreach (var eventEntity in _eventFilter)
            {
                if(_data.Single && _markerPool.Has(eventEntity)) continue;

                var owner = _data.Owner;
                if (!owner.Unpack(_world, out var ownerEntity) && _data.OwnViewBySource)
                    owner = _world.PackEntity(eventEntity);

                _world.MakeViewInContainerRequest(
                    _data.View, 
                    _data.UseBusyContainer, owner,
                    _data.Tag, _data.ViewName,_data.StayWorld);

                if (_data.Single) _markerPool.GetOrAddComponent(eventEntity);
            }
        }
    }
}
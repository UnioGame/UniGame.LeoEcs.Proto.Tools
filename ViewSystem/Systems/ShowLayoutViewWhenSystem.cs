namespace Game.Ecs.UI.EndGameScreens.Systems
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.ViewSystem.Aspects;
    using UniGame.LeoEcs.ViewSystem.Components;
    using UniGame.ViewSystem.Runtime;
    using UniModules.UniCore.Runtime.Utils;
    using UniModules.UniGame.UiSystem.Runtime;
    
    /// <summary>
    /// await target event and create view
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class ShowSingleLayoutViewWhen<TEvent,TView> : IProtoRunSystem
        where TEvent : struct
        where TView : IView
    {
        private string _viewLayoutType;
        private ProtoWorld _world;
        private ViewAspect _viewAspect;

        private ProtoItExc _eventFilter = It
            .Chain<TEvent>()
            .Exc<SingleViewMarkerComponent<TView>>()
            .End();

        public ShowSingleLayoutViewWhen(string viewLayoutType)
        {
            _viewLayoutType = viewLayoutType;
        }

        public void Run()
        {
            foreach (var eventEntity in _eventFilter)
            {
                var requestEntity = _world.NewEntity();
                ref var requestComponent = ref _viewAspect.CreateLayoutView.Add(requestEntity);
                ref var markerComponent = ref _world.AddComponent<SingleViewMarkerComponent<TView>>(eventEntity);

                requestComponent.View = typeof(TView).Name;
                requestComponent.LayoutType = _viewLayoutType;
            }
        }
    }
            
    /// <summary>
    /// await target event and create view
    /// </summary>
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class ShowLayoutViewWhenSystem<TEvent,TView> : IProtoRunSystem
        where TEvent : struct
        where TView : IView
    {
        private string _viewLayoutType;
        private ProtoWorld _world;
        private ViewAspect _viewAspect;
        
        private ProtoIt _eventFilter = It
            .Chain<TEvent>()
            .End();

        public ShowLayoutViewWhenSystem(string viewLayoutType)
        {
            _viewLayoutType = viewLayoutType;
        }

        public void Run()
        {
            foreach (var eventEntity in _eventFilter)
            {
                var requestEntity = _world.NewEntity();
                ref var requestComponent = ref _viewAspect.CreateLayoutView.Add(requestEntity);

                requestComponent.View = typeof(TView).Name;
                requestComponent.LayoutType = _viewLayoutType;
            }
        }
    }
    
    /// <summary>
    /// await target event and create view
    /// </summary>
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class ShowLayoutViewWhenSystem<TView> : IProtoRunSystem
        where TView : IView
    {
        private string _viewLayoutType;
        private ProtoWorld _world;
        private ViewAspect _viewAspect;
        private EcsFilter _eventFilter;

        public ShowLayoutViewWhenSystem(EcsFilter eventFilter,string viewLayoutType)
        {
            _eventFilter = eventFilter;
            _viewLayoutType = viewLayoutType;
        }

        public void Run()
        {
            foreach (var eventEntity in _eventFilter)
            {
                var requestEntity = _world.NewEntity();
                ref var requestComponent = ref _viewAspect.CreateLayoutView.Add(requestEntity);

                requestComponent.View = typeof(TView).Name;
                requestComponent.LayoutType = _viewLayoutType;
            }
        }
    }
    
    /// <summary>
    /// await target event and create view
    /// </summary>
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class ShowLayoutViewWhenSystem<TEvent1,TEvent2,TView> : IProtoRunSystem
        where TEvent1 : struct
        where TEvent2 : struct
        where TView : IView
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;
        private ViewType _viewLayoutType;
        
        private ProtoIt _eventFilter = It
            .Chain<TEvent1>()
            .Inc<TEvent2>()
            .End();

        public ShowLayoutViewWhenSystem(ViewType viewLayoutType = ViewType.Window)
        {
            _viewLayoutType = viewLayoutType;
        }

        public void Run()
        {
            foreach (var eventEntity in _eventFilter)
            {
                var requestEntity = _world.NewEntity();
                ref var requestComponent = ref _viewAspect.CreateLayoutView.Add(requestEntity);

                requestComponent.View = typeof(TView).Name;
                requestComponent.LayoutType = _viewLayoutType.ToStringFromCache();
            }
        }
    }
}
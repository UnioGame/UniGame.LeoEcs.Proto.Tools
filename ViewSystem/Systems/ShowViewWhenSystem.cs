namespace Game.Ecs.UI.EndGameScreens.Systems
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Converter.Runtime;
    using UniGame.LeoEcs.Shared.Extensions;
    using UniGame.LeoEcs.ViewSystem.Aspects;
    using UniGame.LeoEcs.ViewSystem.Components;
    using UniGame.ViewSystem.Runtime;

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
    public class ShowViewWhenSystem<TView> : IProtoRunSystem
        where TView : IView
    {
        private ViewRequestData _data;
        private ProtoWorld _world;
        private ViewAspect _viewAspect;
        private EcsFilter _eventFilter;

        public ShowViewWhenSystem(EcsFilter eventFilter, ViewRequestData data)
        {
            _eventFilter = eventFilter;
            _data = data;
        }

        public void Run()
        {
            foreach (var eventEntity in _eventFilter)
            {
                var requestEntity = _world.NewEntity();

                ref var requestComponent = ref _viewAspect.CreateView.Add(requestEntity);

                var parent = _data.Parent;
                if (parent)
                {
                    var ecsConverter = parent.gameObject.GetComponent<ProtoEcsMonoConverter>();
                    if (ecsConverter && ecsConverter.IsPlayingAndReady)
                        requestComponent.Owner = ecsConverter.PackedEntity;
                }

                requestComponent.Parent = parent;
                requestComponent.ViewId = typeof(TView).Name;
                requestComponent.LayoutType = requestComponent.LayoutType;
                requestComponent.Tag = requestComponent.Tag;
                requestComponent.ViewName = requestComponent.ViewName;
                requestComponent.StayWorld = requestComponent.StayWorld;
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
    public class ShowViewWhenSystem<TEvent, TView> : IProtoRunSystem
        where TEvent : struct
        where TView : IView
    {
        private ViewRequestData _data;
        private ProtoWorld _world;
        private ViewAspect _viewAspect;

        private ProtoIt _eventFilter = It
            .Chain<TEvent>()
            .End();

        public ShowViewWhenSystem(ViewRequestData data)
        {
            _data = data;
        }

        public void Run()
        {
            foreach (var eventEntity in _eventFilter)
            {
                var requestEntity = _world.NewEntity();
                ref var requestComponent = ref _viewAspect.CreateView.Add(requestEntity);


                var parent = _data.Parent;
                if (parent)
                {
                    var ecsConverter = parent.gameObject.GetComponent<ProtoEcsMonoConverter>();
                    if (ecsConverter && ecsConverter.IsPlayingAndReady)
                        requestComponent.Owner = ecsConverter.PackedEntity;
                }

                requestComponent.Parent = parent;
                requestComponent.ViewId = typeof(TView).Name;
                requestComponent.LayoutType = requestComponent.LayoutType;
                requestComponent.Tag = requestComponent.Tag;
                requestComponent.ViewName = requestComponent.ViewName;
                requestComponent.StayWorld = requestComponent.StayWorld;
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
    public class ShowViewWhenSystem<TEvent1, TEvent2, TView> : IProtoRunSystem
        where TEvent1 : struct
        where TEvent2 : struct
        where TView : IView
    {
        private ViewRequestData _data;
        private ProtoWorld _world;
        private ViewAspect _viewAspect;

        private ProtoIt _eventFilter = It
            .Chain<TEvent1>()
            .Inc<TEvent2>()
            .End();

        public ShowViewWhenSystem(ViewRequestData data)
        {
            _data = data;
        }

        public void Run()
        {
            foreach (var eventEntity in _eventFilter)
            {
                var requestEntity = _world.NewEntity();
                ref var requestComponent = ref _viewAspect.CreateView.Add(requestEntity);


                var parent = _data.Parent;
                if (parent)
                {
                    var ecsConverter = parent.gameObject.GetComponent<ProtoEcsMonoConverter>();
                    if (ecsConverter && ecsConverter.IsPlayingAndReady)
                        requestComponent.Owner = ecsConverter.PackedEntity;
                }

                requestComponent.Parent = parent;
                requestComponent.ViewId = typeof(TView).Name;
                requestComponent.LayoutType = requestComponent.LayoutType;
                requestComponent.Tag = requestComponent.Tag;
                requestComponent.ViewName = requestComponent.ViewName;
                requestComponent.StayWorld = requestComponent.StayWorld;
            }
        }
    }
}
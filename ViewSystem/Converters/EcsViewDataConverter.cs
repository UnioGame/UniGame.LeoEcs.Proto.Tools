﻿namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using System;
    using Components;
    using Converter.Runtime;
    using Converter.Runtime.Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Proto.Components;
    using Shared.Extensions;
    using Sirenix.OdinInspector;
    using UiSystem.Runtime;
    using UniGame.ViewSystem.Runtime;
    using UniModules.UniCore.Runtime.DataFlow;
    using UniModules.UniCore.Runtime.Utils;
    using UniRx;
    using UnityEngine;

    [Serializable]
    public class EcsViewDataConverter<TData> :
        GameObjectConverter,
        IConverterEntityDestroyHandler
        where TData : class, IViewModel
    {
        [TitleGroup("settings")]
        public EcsViewSettings settings = new EcsViewSettings();
        
        [TitleGroup("runtime")]
        public int entity;
        
        #region private fields

        private ProtoWorld _world;
        private ProtoPackedEntity _viewPackedEntity;
        private IUiView<TData> _view;
        private LifeTimeDefinition _entityLifeTime;

        #endregion
        
        public bool IsEnabled => true;
        
        public string Name => GetType().Name;

        public void SetUp(EcsViewSettings overrideSettings)
        {
            settings = overrideSettings;
        }
        
        protected override void OnApply(GameObject target, ProtoWorld world, ProtoEntity targetEntity)
        {
            //reset lifetime
            _entityLifeTime ??= new LifeTimeDefinition();
            _entityLifeTime.Release();
            
            _view = target.GetComponent<IUiView<TData>>();
            if (_view == null) return;

            entity = (int)targetEntity;
            
            _world = world;
            _viewPackedEntity = world.PackEntity(entity);
            
            ref var typeIdComponent = ref world.GetOrAddComponent<TypeIdComponent>(entity);
            typeIdComponent.Value = (uint) typeof(TData).GetTypeId();
            
            ref var dataComponent = ref world.GetOrAddComponent<ViewComponent<TData>>(entity);
            ref var viewComponent = ref world.GetOrAddComponent<ViewComponent>(entity);
            ref var viewStatusComponent = ref world.GetOrAddComponent<ViewStatusComponent>(entity);

            viewStatusComponent.Status = _view.Status.Value;
            viewComponent.View = _view;
            viewComponent.Type = _view.GetType();

            _view.OnViewModelChanged
                .Subscribe(OnViewModelChanged)
                .AddTo(_entityLifeTime);

            if (_view.Model != null)
                OnViewModelChanged(_view.Model);
            
            if (settings.addChildOrderComponent)
            {
                ref var childOrderComponent = ref world.GetOrAddComponent<ViewOrderComponent>(entity);
                childOrderComponent.Value = target.transform.GetSiblingIndex();
            }

            //follow entity lifetime  and close view if entity is dead
            if (settings.followEntityLifeTime)
            {
                var lifeTimeEntity = world.NewEntity();
                ref var lifeTimeComponent = ref world
                    .AddComponent<ViewEntityLifeTimeComponent>(lifeTimeEntity);
                lifeTimeComponent.View = _view;
                lifeTimeComponent.Value = _viewPackedEntity;
            }
        }
        
        private void OnViewModelChanged(IViewModel model)
        {
            if(!_viewPackedEntity.Unpack(_world,out var viewEntity))
                return;

            if (settings.addUpdateRequestOnCreate)
            {
                _world.GetOrAddComponent<UpdateViewRequest>(entity);
            }
            
            ref var modelComponent = ref _world
                .GetOrAddComponent<ViewModelComponent>(viewEntity);
            
            modelComponent.Model = model;
        }

        public void OnEntityDestroy(ProtoWorld world, ProtoEntity viewEntity)
        {
            entity = -1;
            
            _entityLifeTime?.Release();
            _world = null;
            _viewPackedEntity = default;
        }
    }
}
namespace UniGame.LeoEcs.ViewSystem
{
    using System;
    using System.Runtime.CompilerServices;
    using Aspects;
    using Bootstrap;
    using Cysharp.Threading.Tasks;
    using Components;
    using UniGame.ViewSystem.Runtime;
    using Bootstrap.Runtime.Attributes;
    using Converter.Runtime;
    using Core.Runtime;
    using Game.Modules.leoecs.proto.tools.Ownership.Aspects;
    using Game.Modules.leoecs.proto.tools.Ownership.Extensions;
    using Layouts.Aspects;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Runtime.Rx.Runtime.Extensions;
    using Shared.Extensions;
    using UiSystem.Runtime;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// System for creating views based on requested data.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class CreateViewSystem : IProtoRunSystem, IProtoInitSystem
    {
        private ProtoWorld _world;

        private ViewAspect _viewAspect;
        private ViewLayoutAspect _viewLayoutAspect;
        private OwnershipAspect _ownershipAspect;

        private IGameViewSystem _viewSystem;
        private IContext _context;

        private ProtoIt _createFilter = It
            .Chain<CreateViewRequest>()
            .End();

        public void Init(IProtoSystems systems)
        {
            _world = systems.GetWorld();
            _viewSystem = _world.GetGlobal<IGameViewSystem>();
            _context = _world.GetGlobal<IContext>();
        }

        public void Run()
        {
            foreach (var entity in _createFilter)
            {
                ref var request = ref _viewAspect.CreateView.Get(entity);

                CreateViewByRequest(request).Forget();
            }
        }

        public async UniTask CreateViewByRequest(CreateViewRequest request)
        {
            var viewType = request.ViewId;
            var layoutId = request.LayoutType;
            var modelType = _viewSystem.ModelTypeMap.GetViewModelType(viewType);
            var model = await _viewSystem.CreateViewModel(_context, modelType);

            var requestLayout = layoutId;

            if (string.IsNullOrEmpty(layoutId) ||
                GameViewSystem.NoneType.Equals(layoutId, StringComparison.OrdinalIgnoreCase))
                requestLayout = string.Empty;

            if (!string.IsNullOrEmpty(requestLayout) &&
                !_viewSystem.HasLayout(requestLayout))
            {
#if UNITY_EDITOR
                Debug.LogError($"Try to create view {viewType} with layout {layoutId} but layout not found");
#endif
                return;
            }

            var view = requestLayout switch
            {
                "" => await _viewSystem
                    .Create(model, request.ViewId, request.Tag, request.Parent, request.ViewName, request.StayWorld),
                _ => await _viewSystem
                    .OpenView(model, request.ViewId, requestLayout, request.Tag, request.ViewName),
            };

            var entity = await UpdateViewEntity(view, request);
            if ((int)entity < 0) return;

            var packedEntity = _world.PackEntity(entity);
            if (!packedEntity.Unpack(_world, out var viewEntity)) return;

            UpdateViewEntityComponent(entity, model, request);
        }

        private async UniTask<ProtoEntity> UpdateViewEntity(IView view, CreateViewRequest request)
        {
            var viewEntity = (ProtoEntity)(-1);
            var viewObject = view.GameObject;
            if (!viewObject) return viewEntity;

            if (request.Target.Unpack(_world, out var targetEntity))
                viewEntity = targetEntity;

            var converter = viewObject.GetComponent<ILeoEcsMonoConverter>();
            if (converter != null && (int)viewEntity < 0)
            {
                if ((int)converter.Entity > 0) return converter.Entity;
                if (!converter.AutoCreate) return viewEntity;

                await UniTask.WaitWhile(() => (int)converter.Entity < 0);
                viewEntity = converter.Entity;
            }
            else
            {
                viewEntity = (int)viewEntity < 0 ? _world.NewEntity() : viewEntity;
                viewObject.ConvertGameObjectToEntity(_world, viewEntity);
            }

            request.Owner.AddChild(viewEntity, _world);
            
            return viewEntity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateViewEntityComponent(ProtoEntity viewEntity, IViewModel model, CreateViewRequest request)
        {
            ref var modelComponent = ref _viewAspect.Model.GetOrAdd(viewEntity);
            modelComponent.Model = model;
            
            ref var parent = ref request.Parent;
            if (!parent)
            {
                return;
            }

            ref var parentComponent = ref _viewLayoutAspect.Parent.GetOrAddComponent(viewEntity);
            parentComponent.Value = parent;
        }
    }
}
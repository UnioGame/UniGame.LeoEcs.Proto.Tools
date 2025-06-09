namespace UniGame.LeoEcs.ViewSystem
{
    using System;
    using Aspects;
    using Bootstrap.Runtime.Attributes;
    using Components;
    using Converter.Runtime.Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Components;

    /// <summary>
    /// System for updating the view order of entities.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif

    [Serializable]
    [ECSDI]
    public class UpdateViewOrderSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;

        private ProtoIt _viewFilter = It
            .Chain<ViewComponent>()
                .Inc<TransformComponent>()
                .Inc<ViewOrderComponent>()
                .End();

        public void Run()
        {
            foreach (var entity in _viewFilter)
            {
                ref var orderComponent = ref _viewAspect.Order.Get(entity);
                ref var transformComponent = ref _viewAspect.Transform.Get(entity);

                var transform = transformComponent.Value;
                var order = transform.GetSiblingIndex();
                orderComponent.Value = order;
            }
        }
    }
}
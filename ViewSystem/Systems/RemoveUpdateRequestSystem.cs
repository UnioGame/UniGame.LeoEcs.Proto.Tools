namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Aspects;
    using Components;
    using Bootstrap.Runtime.Attributes;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;

    /// <summary>
    /// System for removes entities with update requests.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class RemoveUpdateRequestSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;

        private ProtoIt _filter = It
            .Chain<UpdateViewRequest>()
            .End();

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var updateComponent = ref _viewAspect.UpdateView.Get(entity);

                if (updateComponent.counter <= 0)
                {
                    updateComponent.counter += 1;
                    continue;
                }

                _viewAspect.UpdateView.Del(entity);
            }
        }
    }
}
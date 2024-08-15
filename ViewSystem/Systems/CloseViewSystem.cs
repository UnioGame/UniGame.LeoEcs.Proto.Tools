namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Aspects;
    using Bootstrap.Runtime.Attributes;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;

    /// <summary>
    /// System for closing views.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class CloseViewSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private ViewAspect _viewAspect;
        
        private ProtoIt _closeFilter = It
            .Chain<CloseViewSelfRequest>()
            .Inc<ViewComponent>()
            .End();
        
        public void Run()
        {
            foreach (var entity in _closeFilter)
            {
                ref var viewComponent = ref _viewAspect.View.Get(entity);
                viewComponent.View.Close();
                break;
            }
        }
    }
}
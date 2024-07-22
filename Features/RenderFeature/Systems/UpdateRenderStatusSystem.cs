namespace UniGame.LeoEcs.Shared.Components
{
    using System;
    using Bootstrap.Runtime.Attributes;
    using Leopotam.EcsProto;
    using Extensions;
    using Leopotam.EcsProto.QoL;

    /// <summary>
    /// update render components
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class UpdateRenderStatusSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private RendererAspect _rendererAspect;
        
        private ProtoIt _filter = It
            .Chain<RendererComponent>()
            .End();

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var renderComponent = ref _rendererAspect.Render.Get(entity);

                var render = renderComponent.Value;
                if (render.enabled)
                {
                    _rendererAspect.Enabled.GetOrAddComponent(entity);
                }
                else
                {
                    _rendererAspect.Enabled.TryRemove(entity);
                }
                
                if (render.isVisible)
                {
                    _rendererAspect.Visible.GetOrAddComponent(entity);
                }
                else
                {
                    _rendererAspect.Visible.TryRemove(entity);
                }
            }
        }
    }
}
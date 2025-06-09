namespace UniGame.LeoEcs.ViewSystem.Layouts.Aspects
{
    using System;
    using Components;
    using Game.Modules.UnioModules.UniGame.LeoEcsLite.LeoEcs.ViewSystem.Components;
    using Bootstrap;
    using Leopotam.EcsProto;

    /// <summary>
    /// View Layout Aspect
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class ViewLayoutAspect : EcsAspect
    {
        public ProtoPool<ViewLayoutComponent> Layout;
        public ProtoPool<ViewParentComponent> Parent;
        
        //operations
        public ProtoPool<RegisterViewLayoutSelfRequest> Register;
        public ProtoPool<RemoveViewLayoutSelfRequest> Remove;
    }
}
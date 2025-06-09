namespace UniGame.LeoEcs.ViewSystem.Aspects
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Bootstrap;

    /// <summary>
    /// view system aspect
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class ViewSystemAspect : EcsAspect
    {
        public ProtoPool<ViewServiceComponent> ViewService;
        
        //requests
        public ProtoPool<CloseAllViewsRequest> CloseAllViews;
        public ProtoPool<CloseViewByTypeRequest> CloseViewByType;
        public ProtoPool<CloseTargetViewByTypeRequest> CloseTargetViewByType;
        public ProtoPool<CreateViewRequest> CreateView;
    }
}
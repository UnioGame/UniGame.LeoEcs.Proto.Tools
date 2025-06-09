namespace UniGame.LeoEcs.ViewSystem.Aspects
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Bootstrap;

    /// <summary>
    /// view container aspect
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class ViewContainerAspect : EcsAspect
    {
        public ProtoPool<ViewContainerBusyComponent> Busy;
        public ProtoPool<ViewContainerComponent> ContainerView;
        
        //requests
        public ProtoPool<CreateViewInContainerRequest> CreateInContainer;
    }
}
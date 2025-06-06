namespace UniGame.LeoEcs.Bootstrap.Runtime.Abstract
{
    using System;
    using Game.Ecs.Core.Components;
    using LeoEcsLite.LeoEcs.Shared.Components;
    using Leopotam.EcsProto;
    using Proto.Components;
    using Shared.Components;

    /// <summary>
    /// shared components
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class SharedAspect : EcsAspect
    {
        public ProtoPool<ActiveComponent> Active;
        public ProtoPool<HashComponent> Hash;
        public ProtoPool<NameComponent> Name;
        public ProtoPool<TypeIdComponent> TypeId;
        public ProtoPool<NewEntityComponent> NewEntity;
    }
}
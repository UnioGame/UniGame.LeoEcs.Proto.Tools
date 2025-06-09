namespace UniGame.LeoEcs.Proto.Game.Modules.unigame.leoecs.proto.tools.Converter.Runtime.Aspects
{
    using System;
    using LeoEcs.Converter.Runtime.Components;
    using LeoEcsLite.LeoEcs.Shared.Components;
    using Leopotam.EcsProto;
    using Bootstrap;

    /// <summary>
    /// components of ecs converter
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public class EcsConverterAspect : EcsAspect
    {
        public ProtoPool<ObjectConverterComponent> Converter;
        public ProtoPool<ParentEntityComponent> Parent;
    }
}
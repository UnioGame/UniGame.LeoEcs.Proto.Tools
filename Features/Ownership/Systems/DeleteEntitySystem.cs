namespace Game.Modules.leoecs.proto.tools.Ownership.Systems
{
    using System;
    using Ecs.Core.Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Proto.Filters.Game.Modules.leoecs.proto.tools.LifeTime.Components;
    using UniGame.LeoEcs.Shared.Extensions;
    
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class DeleteEntitySystem : IProtoRunSystem
    {
        private ProtoWorld _world;

        private ProtoIt _deleteEntityFilter = It
            .Chain<DeleteEntityNextComponent>()
            .Inc<PrepareToDeathComponent>()
            .End();

        public void Run()
        {
            foreach (var entity in _deleteEntityFilter)
            {
                _world.DelEntity(entity);
            }
        }
    }
}
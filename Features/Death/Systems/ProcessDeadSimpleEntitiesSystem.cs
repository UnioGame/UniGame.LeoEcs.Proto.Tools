namespace Game.Ecs.Core.Death.Systems
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Components;

    /// <summary>
    /// System for removing dead entities from the world.
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public sealed class ProcessDeadSimpleEntitiesSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        
        private ProtoItExc _filter = It
            .Chain<DeadEvent>()
            .Exc<TransformComponent>()
            .End();
        
        public void Run()
        {
            foreach (var entity in _filter)
            {
                _world.DelEntity(entity);
            }
        }
    }
}
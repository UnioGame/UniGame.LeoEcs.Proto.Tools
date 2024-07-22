namespace Game.Ecs.Time.Systems
{
    using System;
    using Aspects;
    using Components;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Service;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;
    using UnityEngine;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class UpdateEntityTimeSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private GameTimeAspect _timeAspect;
        
        private ProtoIt _filter = It
            .Chain<EntityGameTimeComponent>()
            .End();
        
        public void Run()
        {
            GameTime.Time = Time.time;
            GameTime.DeltaTime = Time.deltaTime;
            GameTime.RealTime = Time.realtimeSinceStartup;
            
            foreach (var entity in _filter)
            {
                ref var timeComponent = ref _timeAspect.Time.Get(entity);
                timeComponent.Value += GameTime.DeltaTime;
            }
        }
    }
}

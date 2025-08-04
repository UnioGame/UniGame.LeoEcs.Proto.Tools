namespace Game.Ecs.Core.Death.Systems
{
    using System;
    using Aspects;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;
    using UniGame.LeoEcs.Shared.Extensions;

    /// <summary>
    /// ADD DESCRIPTION HERE
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class ProcessBlockDeathSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private DestroyAspect _destroyAspect;

        private ProtoIt _blockFilter = It
            .Chain<DontKillComponent>()
            .End();
        
        public void Run()
        {
            foreach (var blockedEntity in _blockFilter)
            {
                ref var blockedComponent = ref _destroyAspect.DontKill.Get(blockedEntity);
                
                if(blockedComponent.blockers > 0)
                    continue;

                _destroyAspect.DontKill.Del(blockedEntity);
                _destroyAspect.Kill.Add(blockedEntity);
            }
        }
    }
}
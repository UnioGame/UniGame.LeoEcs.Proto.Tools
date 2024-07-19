namespace Game.Ecs.Core.Systems
{
    using System;
    using Components;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;
    using UniGame.LeoEcs.Bootstrap.Runtime.Attributes;

    /// <summary>
    /// one round trip entity lifetime
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public class KillMeNextTimeHandleSystem : IProtoRunSystem
    {
        private ProtoWorld _world;
        private LifeTimeAspect _lifeTimeAspect;
        
        private ProtoIt _filter = It
            .Chain<KillMeNextTimeComponent>()
            .End();

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var component = ref _lifeTimeAspect.KillMeNextTime.Get(entity);
                if (component.Value)
                {
                    _world.DelEntity(entity);
                }
                else
                {
                    component.Value = true;
                }
            }
        }
    }
}
namespace UniGame.LeoEcs.Bootstrap.Runtime.Aspects
{
    using System;
    using System.Collections.Generic;
    using Attributes;
    using Core.Runtime.SerializableType;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniModules.UniCore.Runtime.Utils;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    [Serializable]
    [ECSDI]
    public abstract class TypePoolAspect : ProtoAspectInject
    {
        public List<SType> componentTypes = new();

        public override void Init(ProtoWorld world)
        {
            if (world.HasAspect(this.GetType()))
                return; 
            
            foreach (Type poolType in componentTypes)
            {
                if(world.HasPool(poolType))continue;
                var pool = poolType.CreateWithDefaultConstructor() as IProtoPool;
                if(pool == null) continue;
                world.AddPool(pool);
            }
            
            base.Init(world);
        }
    }
}
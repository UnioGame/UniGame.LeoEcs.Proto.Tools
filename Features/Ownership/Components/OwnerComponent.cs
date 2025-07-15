namespace UniGame.Proto.Ownership
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using UniGame.LeoEcs.Proto;
    using Unity.Collections;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct OwnerComponent : IProtoAutoReset<OwnerComponent>
    {
        public NativeList<ProtoPackedEntity> Children;
        
        public void AddChild(ProtoPackedEntity packedEntity)
        {
            Children.Add(packedEntity);
        }

        public void RemoveChild(ProtoPackedEntity packedEntity)
        {
            for (var i = 0; i < Children.Length; i++)
            {
                if (Children[i] != packedEntity) continue;
                Children.RemoveAt(i);
                return;
            }
        }
        
        public bool HasChild(ProtoPackedEntity packedEntity)
        {
            return Children.IsCreated && Children.Contains(packedEntity);
        }
        
        public void SetHandlers(IProtoPool<OwnerComponent> pool) => pool.SetResetHandler(AutoReset);
        
        public static void AutoReset(ref OwnerComponent c)
        {
            if (c.Children.IsCreated)
                c.Children.Dispose();
            c.Children = new NativeList<ProtoPackedEntity>(0, Allocator.Persistent);
        }
    }
}
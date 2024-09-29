namespace Game.Modules.leoecs.proto.tools.Ownership.Components
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto.QoL;
    using Unity.Collections;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct OwnerComponent : IEcsAutoReset<OwnerComponent>
    {
        public NativeList<ProtoPackedEntity> Children;
        
        public void AddChild(ProtoPackedEntity packedEntity)
        {
            Children.Add(packedEntity);
        }

        public void RemoveChild(ProtoPackedEntity packedEntity)
        {
            for (int i = 0; i < Children.Length; i++)
            {
                if (Children[i] == packedEntity)
                {
                    Children.RemoveAt(i);
                    return;
                }
            }
        }
        
        public bool HasChild(ProtoPackedEntity packedEntity)
        {
            return Children.IsCreated && Children.Contains(packedEntity);
        }
        
        public void AutoReset(ref OwnerComponent c)
        {
            if (c.Children.IsCreated)
            {
                c.Children.Clear();
                return;
            }
            
            c.Children = new NativeList<ProtoPackedEntity>(12, Allocator.Persistent);
        }
    }
}
namespace Game.Ecs.Core.Components
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Unity.Collections;
    using UnityEngine.Serialization;

    /// <summary>
    /// owner entity
    /// </summary>
#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [Serializable]
    public struct OwnerComponent : IEcsAutoReset<OwnerComponent>
    {
        private NativeList<ProtoPackedEntity> _children; 
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.OnInspectorGUI]
#endif
        public ProtoPackedEntity Value;


        public void AddChild(ref ProtoEntity entity)
        {
            
        }
        
        public void AutoReset(ref OwnerComponent c)
        {
            if (_children.IsCreated)
            {
                foreach (var packedEntity in _children)
                {
                    
                }
            }
        }
    }
}
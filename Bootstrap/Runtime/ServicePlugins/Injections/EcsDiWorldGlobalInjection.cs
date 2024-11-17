namespace UniGame.LeoEcs.Bootstrap.Runtime.PostInitialize
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Leopotam.EcsProto;
    using Shared.Extensions;

    [Serializable]
    public class EcsDiWorldGlobalInjection : IEcsDiInjection
    {
        public void ApplyInjection(
            IProtoSystems ecsSystems, 
            FieldInfo field, 
            object target, 
            IReadOnlyList<IEcsDiInjection> injections)
        {
            if(target == null) return;
            
            var world = ecsSystems.World();
            var fieldType = field.FieldType;
            
            if(field.GetValue(target) != null) return;
            var value = world.GetGlobal(fieldType);
            if(value == null) return;
            
            field.SetValue(target, value);
        }

        
        public void Initialize()
        {
            
        }

        public void PostInject()
        {
            
        }
    }
}
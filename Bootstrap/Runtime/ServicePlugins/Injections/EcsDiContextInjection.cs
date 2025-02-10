namespace UniGame.LeoEcs.Bootstrap.Runtime.PostInitialize
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Core.Runtime;
    using Leopotam.EcsProto;
    using Shared.Extensions;

    [Serializable]
    public class EcsDiContextInjection : IEcsDiInjection
    {
        public void ApplyInjection(
            IProtoSystems ecsSystems, 
            FieldInfo field, 
            object target, 
            IReadOnlyList<IEcsDiInjection> injections)
        {
            if(target == null) return;
            
            var context = ecsSystems.GetService<IContext>();
            if(context == null) return;
            
            var fieldType = field.FieldType;
            
            if(field.GetValue(target) != null) return;
            var value = context.Get(fieldType);
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
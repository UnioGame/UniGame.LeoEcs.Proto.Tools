namespace UniGame.LeoEcs.Bootstrap.Runtime.PostInitialize
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Leopotam.EcsProto;

    [Serializable]
    public class EcsDiItInjection : IEcsDiInjection
    {
        private Type _itType = typeof(IProtoIt);
        
        public void ApplyInjection(
            IProtoSystems ecsSystems, 
            FieldInfo field, 
            object target, 
            IReadOnlyList<IEcsDiInjection> injections)
        {
            var world = ecsSystems.World();
            var fieldType = field.FieldType;
            
            if (!_itType.IsAssignableFrom(fieldType)) return;

            var it = field.GetValue (target) as IProtoIt;
            if (it == null) return;
            
            field.SetValue (target, it.Init (world));
        }
        
        public void Initialize()
        {
            
        }

        public void PostInject()
        {
            
        }
    }
}
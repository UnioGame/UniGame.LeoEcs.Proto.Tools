namespace UniGame.LeoEcs.Bootstrap.Runtime.PostInitialize
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Leopotam.EcsProto;
    using Shared.Extensions;
    using UniGame.Runtime.Utils;

    public class EcsDiPoolInjection : IEcsDiInjection
    {
        private Type _basePoolType = typeof(IProtoPool);
        
        public void Initialize()
        {
            
        }

        public void PostInject()
        {
            
        }
        
        public void ApplyInjection(IProtoSystems ecsSystems,
            FieldInfo field,object target, 
            IReadOnlyList<IEcsDiInjection> injections)
        {
            var world = ecsSystems.GetWorld();
            var fieldType = field.FieldType;
            
            if (!_basePoolType.IsAssignableFrom(fieldType) || fieldType.IsAbstract || fieldType.IsInterface) return;
            
            if (!world.HasPool(fieldType))
            {
                return;
                var poolInstance = fieldType.CreateWithDefaultConstructor();
                world.AddPool(poolInstance as IProtoPool);
            }

            var poolObject = world.Pool(fieldType);
            field.SetValue(target,poolObject);
        }
        
    }
}
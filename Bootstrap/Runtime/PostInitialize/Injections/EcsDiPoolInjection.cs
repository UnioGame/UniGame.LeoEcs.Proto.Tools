﻿namespace UniGame.LeoEcs.Bootstrap.Runtime.PostInitialize
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Leopotam.EcsProto;
    using Shared.Extensions;

    public class EcsDiPoolInjection : IEcsDiInjection
    {
        private const string PoolMethodName = "Pool";
        private MethodInfo _poolMethod;
        private Type _poolType = typeof(ProtoPool<>);
        
        public void ApplyInjection(IProtoSystems ecsSystems,
            FieldInfo field,object target, 
            IReadOnlyList<IEcsDiInjection> injections)
        {
            var world = ecsSystems.GetWorld();
            var fieldType = field.FieldType;
            
            _poolMethod ??= world.GetType().GetMethod(PoolMethodName);
            if(_poolMethod == null) return;
            
            if (!fieldType.IsGenericType) return;

            var isPoolType = fieldType.GetGenericTypeDefinition() == _poolType;
            if(!isPoolType) return;
                
            var fieldValue = field.GetValue(target);
            if(fieldValue!=null) return;
                
            var elementType = fieldType.GetGenericArguments()[0];
            var poolGenericMethod = _poolMethod.MakeGenericMethod(elementType);
            var result = poolGenericMethod.Invoke(world,null);
            
            field.SetValue(target,result);
        }
        
    }
}
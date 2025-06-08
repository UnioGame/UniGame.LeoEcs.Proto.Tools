namespace UniGame.LeoEcs.Bootstrap.Runtime.PostInitialize
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Leopotam.EcsProto;
    using UniGame.Runtime.ReflectionUtils;
    using UniGame.Runtime.Utils;
    using UnityEngine;

    [Serializable]
    public class EcsDiServicesInjection : IEcsDiInjection
    {
        
        public void ApplyInjection(
            IProtoSystems ecsSystems, 
            FieldInfo field, 
            object target, 
            IReadOnlyList<IEcsDiInjection> injections)
        {
            var world = ecsSystems.World();
            var services = ecsSystems.Services();
            var fieldType = field.FieldType;

            if (services.TryGetValue(fieldType, out var injectObj))
            {
                field.SetValue(target, injectObj);
            }
        }

        
        public void Initialize()
        {
            
        }

        public void PostInject()
        {
            
        }
    }
}
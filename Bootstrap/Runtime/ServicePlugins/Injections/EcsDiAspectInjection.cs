namespace UniGame.LeoEcs.Bootstrap.Runtime.PostInitialize
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Leopotam.EcsProto;
    using UniGame.Runtime.ReflectionUtils;
    using UniGame.Runtime.Utils;

    [Serializable]
    public class EcsDiAspectInjection : IEcsDiInjection
    {
        private Type _aspectType = typeof(IProtoAspect);
        private MethodInfo _initializeMethod;
        private MethodInfo _postInitializeMethod;
        
        public void ApplyInjection(
            IProtoSystems ecsSystems, 
            FieldInfo field, 
            object target, 
            IReadOnlyList<IEcsDiInjection> injections)
        {
            var world = ecsSystems.World();
            var fieldType = field.FieldType;
            
            if (!_aspectType.IsAssignableFrom(fieldType)) return;

            var aspectType = fieldType;
            var aspectValue = field.GetValue(target);
            if (aspectValue != null)
            {
                return;
                RegisterAspect(world, aspectValue as IProtoAspect);
            }

            if (!world.HasAspect(fieldType)) return;
            
            var fields = fieldType.GetInstanceFields();
            var value = world.Aspect(fieldType);
            field.SetValue(target,value);
            
            foreach (var fieldInfo in fields)
            {
                foreach (var diInjection in injections)
                    diInjection.ApplyInjection(ecsSystems, fieldInfo, value, injections);
            }
        }

        public IProtoAspect RegisterAspect(ProtoWorld world,Type aspectType)
        {
            if (world.HasAspect(aspectType)) return world.Aspect(aspectType);
            var aspect = aspectType.CreateWithDefaultConstructor() as IProtoAspect;
            return RegisterAspect(world, aspect);
        }
        
        public IProtoAspect RegisterAspect(ProtoWorld world,IProtoAspect aspect)
        {
            if(aspect == null) return null;
            
            var aspectType = aspect.GetType();
            
            if (world.HasAspect(aspect.GetType())) 
                return world.Aspect(aspectType);
            
            aspect.Init(world);
            aspect.PostInit();
            
            return aspect;
        }
        
        public void Initialize()
        {
            
        }

        public void PostInject()
        {
            
        }
    }
}
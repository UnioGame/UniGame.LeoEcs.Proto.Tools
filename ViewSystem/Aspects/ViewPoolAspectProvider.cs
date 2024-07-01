namespace UniGame.LeoEcs.ViewSystem.Aspects
{
    using System;
    using System.Collections.Generic;
    using Bootstrap.Runtime.Abstract;
    using Bootstrap.Runtime.Aspects;
    using Components;
    using Core.Runtime.SerializableType;
    using Leopotam.EcsProto;
    using UniModules.UniCore.Runtime.Utils;
    using Converters;
    
#if UNITY_EDITOR
    using UniGame.ViewSystem.Runtime;
    using UnityEditor;
#endif

    [Serializable]
    public class ViewPoolAspectProvider : IProtoAspectFactory
    {
        public static readonly Type ViewComponentType = typeof(ViewComponent<>);
        
        public List<SType> poolTypes = new();
        
        public IProtoAspect Create()
        {
            var aspect = new ViewPoolAspect
            {
                componentTypes = poolTypes
            };
            return aspect;
        }

#if UNITY_EDITOR
        public void EditorInitialize()
        {
            poolTypes.Clear();
            
            var modelTypes = typeof(IViewModel);
            var poolType = typeof(ProtoPool<>);
            
            var modelTypesList = TypeCache.GetTypesDerivedFrom(modelTypes);
            
            foreach (var modelType in modelTypesList)
            {
                if(modelType.IsInterface || modelType.IsAbstract) continue;
                var genericType = ViewComponentType.MakeGenericType(modelType);
                var poolGenericType = poolType.MakeGenericType(genericType);
                poolTypes.Add(poolGenericType);
            }
        }
#endif
        
    }
    
    [Serializable]
    public class ViewPoolAspect : TypePoolAspect
    {
        
    }
}
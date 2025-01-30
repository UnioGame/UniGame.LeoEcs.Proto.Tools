namespace UniGame.LeoEcs.Bootstrap.Runtime.Aspects
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Leopotam.EcsProto;
    using UnityEngine;

    [Serializable]
    public class WorldAspect : IProtoAspect
    {
        public ProtoWorld world;
        public Dictionary<Type,IProtoAspect> aspects = new();
        
        public IEnumerable<IProtoAspect> Aspects => aspects.Values;
        
        public void AddAspect(IProtoAspect aspect)
        {
            var type = aspect.GetType();
            if (aspects.ContainsKey(type))
                return;
            
            aspects[type] = aspect;
        }
        
        public void Init(ProtoWorld aspectWorld)
        {
            world = aspectWorld;
            var worldAspects = world.Aspects();
            foreach (var aspect in aspects)
            {
                if (worldAspects.ContainsKey(aspect.Key))
                    continue;
                aspect.Value.Init(world);
            }
        }

        public void PostInit()
        {
            var worldAspects = world.Aspects();
            foreach (var aspect in aspects)
            {
#if UNITY_EDITOR
                if (aspect.Value == null)
                {
                    Debug.LogError($"WorldAspect | Aspect: {aspect.Key.Name} is NULL");
                    continue;
                }
#endif
                if (worldAspects.ContainsKey(aspect.Key))
                {
                    worldAspects[aspect.Key].PostInit();
                    continue;
                }
                aspect.Value.PostInit();
            }
        }
    }
}
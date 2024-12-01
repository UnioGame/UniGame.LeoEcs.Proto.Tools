using System;
using System.Threading;
using Leopotam.EcsLite;
using UniGame.LeoEcs.Converter.Runtime.Abstract;
using UnityEngine;

namespace UniGame.LeoEcs.Converter.Runtime
{
    using Leopotam.EcsProto;

    [CreateAssetMenu(menuName = "ECS Proto/Converter/Simple Component Converter",fileName = "Simple Component Converter")]
    public abstract class ComponentConverterAsset : ScriptableObject, IEcsComponentConverter
    {
        public bool enabled = true;

        public bool IsEnabled => enabled;

        public string Name => name;
        
        public abstract void Apply(ProtoWorld world, ProtoEntity entity);
        
        public virtual bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            if (IsSubstring(GetType().Name,searchString))
                return true;
            if (IsSubstring(name,searchString))
                return true;
            return false;
        }

        protected bool IsSubstring(string value, string search)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value.Contains(search, StringComparison.OrdinalIgnoreCase);
        }
    }
}
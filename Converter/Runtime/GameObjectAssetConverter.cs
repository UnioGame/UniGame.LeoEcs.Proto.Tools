namespace UniGame.LeoEcsLite.LeoEcs.Converter.Runtime
{
    using System;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Converter.Runtime;
    using UniGame.LeoEcs.Converter.Runtime.Abstract;
    using UnityEngine;

#if TRI_INSPECTOR
    using TriInspector;
#endif
    
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [CreateAssetMenu(menuName = "ECS Proto/Converter/GameObject Converter",fileName = "GameObject Converter")]
    public class GameObjectAssetConverter : ScriptableObject,IEcsComponentConverter
    {
#if  ODIN_INSPECTOR || TRI_INSPECTOR
        [HideLabel]
        [InlineProperty]
#endif
        public GameObjectConverter converter = new();

        public string Name => GetType().Name;
        
        public bool IsEnabled => converter.IsEnabled;
        
        public void Apply(ProtoWorld world, ProtoEntity entity)
        {
            converter.Apply(world,entity);
        }

        public void Apply(GameObject target, ProtoWorld world, ProtoEntity entity)
        {
            converter.Apply(target,world,entity);
        }

        public bool IsMatch(string searchString)
        {
            if (converter.IsMatch(searchString)) return true;
            if (name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
    }
}
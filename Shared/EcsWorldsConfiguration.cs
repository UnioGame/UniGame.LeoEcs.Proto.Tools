namespace UniGame.LeoEcs.Proto.Shared
{
    using System.Collections.Generic;
    using UnityEngine;

#if UNITY_EDITOR
    using UniModules.Editor;
#endif
    
    [CreateAssetMenu(menuName = "ECS Proto/ECS Worlds", fileName = nameof(EcsWorldsAsset),order = 0)]
    public class EcsWorldsAsset : ScriptableObject
    {
        public List<string> worlds = new();

        
        private static EcsWorldsAsset _worldsAsset;
        
        public static IEnumerable<string> GetCustomWorlds()
        {
#if UNITY_EDITOR
            _worldsAsset ??= AssetEditorTools.GetAsset<EcsWorldsAsset>();
            if(_worldsAsset == null) 
                yield break;
            
            foreach (var world in _worldsAsset.worlds)
            {
                yield return world;
            }
#endif
            yield break;
        }
        
    }
}
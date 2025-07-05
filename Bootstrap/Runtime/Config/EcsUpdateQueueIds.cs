namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using System.Collections.Generic;
    using LeoEcs.Bootstrap.Runtime;
#if UNITY_EDITOR
    using UniModules.Editor;
#endif
    
    public static class EcsUpdateQueueIds
    {
        public static IEnumerable<string> GetUpdateIds()
        {
#if UNITY_EDITOR

            var configuration = AssetEditorTools.GetAsset<EcsConfiguration>();
            if(configuration == null) yield break;
            
            var map = configuration.ecsUpdateMap;
            if(map == null) yield break;
            foreach (var updateQueue in map.updateQueue)
            {
                yield return updateQueue.OrderId;
            }
#endif
            yield break;
        }
        
    }
}
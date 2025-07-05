namespace UniGame.Ecs.Bootstrap.Editor.Menu
{
    using UniGame.Ecs.Bootstrap.Runtime.Config;
    using UniGame.LeoEcs.Bootstrap.Runtime;
    using UnityEngine;
    
#if UNITY_EDITOR
    using UniModules.Editor;
    using UnityEditor;
#endif

    public static class EcsBootAssetEditorMenu
    {
#if UNITY_EDITOR  
        [MenuItem("Assets/Create/ECS Proto/Create Ecs Configuration")]
        public static void CreateEcsServiceMenu()
        {
            var selection = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(selection);

            var source = ScriptableObject.CreateInstance<EcsServiceSource>();
            var features = ScriptableObject.CreateInstance<EcsConfiguration>();
            
            source.name = "Ecs Service Source";
            features.name = "Ecs Features Configuration";
            
            features = features.SaveAsset(path);
            
            source.features = features;
            source.SaveAsset(path);
        }
#endif
    }
}
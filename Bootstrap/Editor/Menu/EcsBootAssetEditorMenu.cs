﻿namespace UniGame.Ecs.Bootstrap.Editor.Menu
{
    using UniGame.Ecs.Bootstrap.Runtime.Config;
    using UniGame.LeoEcs.Bootstrap.Runtime;
    using UniModules.Editor;
    using UnityEditor;
    using UnityEngine;

    public static class EcsBootAssetEditorMenu
    {
        [MenuItem("Assets/Create/UniGame/Ecs Proto/Create Ecs Configuration")]
        public static void CreateEcsServiceMenu()
        {
            var selection = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(selection);

            var source = ScriptableObject.CreateInstance<EcsServiceSource>();
            var features = ScriptableObject.CreateInstance<EcsFeaturesConfiguration>();
            var map = ScriptableObject.CreateInstance<EcsUpdateMapAsset>();
            
            source.name = "Ecs Service Source";
            features.name = "Ecs Features Configuration";
            map.name = "Ecs Update Map";
            
            features = features.SaveAsset(path);
            map = map.SaveAsset(path);
            
            source.features = features;
            source.updatesMap = map;
            source.SaveAsset(path);
        }
        
    }
}
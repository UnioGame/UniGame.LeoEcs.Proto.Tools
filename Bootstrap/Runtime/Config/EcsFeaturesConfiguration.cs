namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using System.Collections.Generic;
    using LeoEcs.Bootstrap.Runtime.Abstract;
    using LeoEcs.Bootstrap.Runtime.PostInitialize;
    using UniModules.UniCore.Runtime.Utils;
    using UnityEngine;
    using UnityEngine.Serialization;

#if UNITY_EDITOR
    using Leopotam.EcsProto;
    using UnityEditor;
    using UniModules.Editor;
#endif
    
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

#if TRI_INSPECTOR
    using TriInspector;
#endif
    
    [CreateAssetMenu(menuName = "ECS Proto/ECS Features Configuration", fileName = nameof(EcsFeaturesConfiguration))]
    public class EcsFeaturesConfiguration : ScriptableObject, IEcsSystemsConfig
    {
#if ODIN_INSPECTOR
       [FoldoutGroup("world config")]
#endif
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [InlineProperty]
        [HideLabel]
#endif
        public WorldConfiguration worldConfiguration = new WorldConfiguration();
        
        public bool enableUnityModule = true;
        
        [Space(8)]
        [SerializeField]
#if ODIN_INSPECTOR
        [ListDrawerSettings(ListElementLabelName = "updateType")]
#endif
        //[Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        public List<EcsConfigGroup> ecsUpdateGroups = new List<EcsConfigGroup>();
        
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [InlineProperty]
        [HideLabel]
#endif  
        public AspectsData aspectsData = new AspectsData();
        
#if ODIN_INSPECTOR 
        [ListDrawerSettings(ListElementLabelName = "@name")]
#endif
        public List<EcsPlugin> plugins = new List<EcsPlugin>()
        {
            new EcsPlugin()
            {
                enabled = true,
                name = nameof(EcsDiPlugin),
                plugin = new EcsDiPlugin()
            },
        };

        public bool EnableUnityModules => enableUnityModule;
        public WorldConfiguration WorldConfiguration => worldConfiguration;

        public AspectsData AspectsData => aspectsData;
        
        public IReadOnlyList<EcsPlugin> Plugins => plugins;
        
        public IReadOnlyList<EcsConfigGroup> FeatureGroups => ecsUpdateGroups;
        
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [Button]
#endif
        public void CollectAspects()
        {
#if UNITY_EDITOR
            var aspectsCollection = aspectsData.aspects;
            aspectsCollection.Clear();
            
            var aspectItems = TypeCache.GetTypesDerivedFrom(typeof(IProtoAspect));
            foreach (var aspectType in aspectItems)
            {
                if(aspectType.IsAbstract || aspectType.IsInterface) continue;
                if(!aspectType.HasDefaultConstructor()) continue;
                
                aspectsCollection.Add(new AspectData()
                {
                    enabled = true,
                    name = aspectType.Name,
                    aspectType = aspectType
                });
            }

            aspectsData.factories.Clear();
            var aspectsFactories = TypeCache.GetTypesDerivedFrom(typeof(IProtoAspectFactory));
            foreach (var aspectsFactory in aspectsFactories)
            {
                if(aspectsFactory.IsAbstract || aspectsFactory.IsInterface) continue;
                if(!aspectsFactory.HasDefaultConstructor()) continue;
                
                var factory = aspectsFactory.CreateWithDefaultConstructor() as IProtoAspectFactory;
                if(factory == null) continue;
                
                factory.EditorInitialize();
                aspectsData.factories.Add(factory);
            }
            
            this.MarkDirty();

#endif
        }
        
        
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [Button]
#endif
        public void ResetPlugins()
        {
            plugins = new List<EcsPlugin>()
            {
                new EcsPlugin()
                {
                    enabled = true,
                    name = nameof(EcsDiPlugin),
                    plugin = new EcsDiPlugin()
                },
            };
        }

#if UNITY_EDITOR

        [InitializeOnLoadMethod]
        public static void ReloadEcsConfiguration()
        {
            var assets = AssetEditorTools.GetAssets<EcsFeaturesConfiguration>();
            foreach (var asset in assets)
            {
                if(!asset.aspectsData.autoRegisterAspects)
                    continue;
                
                asset.CollectAspects();
            }
        }
        
#endif
        
    }
}
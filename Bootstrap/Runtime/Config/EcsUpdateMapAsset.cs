using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

#if TRI_INSPECTOR
using TriInspector;
#endif

namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using LeoEcs.Bootstrap;

    [CreateAssetMenu(menuName = "ECS Proto/Systems Update Map", fileName = "Systems Update Map")]
    public class EcsUpdateMapAsset : ScriptableObject
    {
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [InlineProperty]
#endif
        public List<EcsUpdateQueue> updateQueue = new()
        {
            new EcsUpdateQueue()
            {
                OrderId = "Update",
                Factory = new EcsUniTaskUpdateProvider()
                {
                    updateType = EcsPlayerUpdateType.Update
                }
            },
            
            new EcsUpdateQueue()
            {
                OrderId = "Fixed_Update",
                Factory = new EcsUniTaskUpdateProvider()
                {
                    updateType = EcsPlayerUpdateType.FixedUpdate
                }
            },
            
            new EcsUpdateQueue()
            {
                OrderId = "Late_Update",
                Factory = new EcsUniTaskUpdateProvider()
                {
                    updateType = EcsPlayerUpdateType.LateUpdate
                }
            },
        };

#if ODIN_INSPECTOR || TRI_INSPECTOR
        [InlineProperty]
#endif
        [SerializeReference]
        public List<IEcsSystemsPluginProvider> systemsPlugins = new();

        [SerializeReference] public IEcsUpdateOrderProvider defaultFactory = new EcsUniTaskUpdateProvider()
        {
            updateType = EcsPlayerUpdateType.Update
        };
    }
}
namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using System;
    using System.Collections.Generic;
    using LeoEcs.Bootstrap;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public class EcsUpdateMap
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

        [SerializeReference] public IEcsUpdateOrderProvider defaultFactory = new EcsUniTaskUpdateProvider()
        {
            updateType = EcsPlayerUpdateType.Update
        };
    }
}
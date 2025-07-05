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
        public const string UpdateQueueId = "Update";
        public const string FixedQueueId = "Fixed_Update";
        public const string LateQueueId = "Late_Update";
        
#if ODIN_INSPECTOR || TRI_INSPECTOR
        [InlineProperty]
#endif
        public List<EcsUpdateQueue> updateQueue = new()
        {
            new EcsUpdateQueue()
            {
                OrderId = UpdateQueueId,
                Factory = new EcsUniTaskUpdateProvider()
                {
                    updateType = EcsPlayerUpdateType.Update
                }
            },
            
            new EcsUpdateQueue()
            {
                OrderId = FixedQueueId,
                Factory = new EcsUniTaskUpdateProvider()
                {
                    updateType = EcsPlayerUpdateType.FixedUpdate
                }
            },
            
            new EcsUpdateQueue()
            {
                OrderId = LateQueueId,
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
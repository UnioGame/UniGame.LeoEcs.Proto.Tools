﻿using System;
using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

#if TRI_INSPECTOR
    using TriInspector;
#endif

namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using LeoEcs.Bootstrap.Runtime;

    [Serializable]
    public class EcsUniTaskUpdateProvider : IEcsUpdateOrderProvider
    {
        public EcsPlayerUpdateType updateType = EcsPlayerUpdateType.Update;

        public EcsPlayerUpdateType UpdateType => updateType;
        
        public IEcsExecutor Create() => new EcsExecutor(updateType);
    }
}
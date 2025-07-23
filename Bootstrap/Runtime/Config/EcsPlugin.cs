namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using System;
    using LeoEcs.Bootstrap;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Serialization;

    [Serializable]
    public class EcsPlugin
    {
        [FormerlySerializedAs("pluginName")]
        public string name;
        public bool enabled;
        
#if ODIN_INSPECTOR
        [HideLabel]
        [InlineProperty]
#endif
        [SerializeReference]
        public IEcsServicePlugin plugin;
    }
}
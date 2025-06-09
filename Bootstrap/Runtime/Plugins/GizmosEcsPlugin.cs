using System;
using UniGame.LeoEcs.Bootstrap;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniGame.Ecs.Bootstrap.Runtime.Config
{
    using LeoEcs.Bootstrap.Runtime;

    [Serializable]
    public class GizmosEcsPlugin : IEcsSystemsPluginProvider
    {
        public ISystemsPlugin Create()
        {
            var gameObject = new GameObject();
            var gizmos = gameObject.AddComponent<EcsGizmosExecutor>();
            Object.DontDestroyOnLoad(gameObject);
            return gizmos;
        }
    }
}
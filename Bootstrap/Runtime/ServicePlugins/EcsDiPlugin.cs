namespace UniGame.LeoEcs.Bootstrap.Runtime.PostInitialize
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Attributes;
    using Core.Runtime;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Extensions;
    using UniModules.UniCore.Runtime.ReflectionUtils;

    [Serializable]
    public class EcsDiPlugin : IEcsServicePlugin
    {
        private Type _diAttributeType = typeof(ECSDIAttribute);
        private Type _protoDiAttribute = typeof(DIAttribute);
        private List<IEcsDiInjection> _injections = null;

        public EcsDiPlugin()
        {
            _injections = new List<IEcsDiInjection>()
            {
                new EcsDiWorldInjection(),
                new EcsDiPoolInjection(),
                new EcsDiAspectInjection(),
                new EcsDiItInjection(),
                new EcsDiServicesInjection(),
                new EcsDiWorldGlobalInjection(),
            };
        }

        public void PreInit(IContext context)
        {
            foreach (var injection in _injections)
                injection.Initialize();
        }

        public void PostInit()
        {
            foreach (var injection in _injections)
                injection.PostInject();
        }

        public void Init(EcsFeatureSystems ecsSystems)
        {
            var systems = ecsSystems.Systems();
            var len = systems.Len();
            var data = systems.Data();
            
            for (var i = 0; i < len; i++)
            {
                var system = data[i];
                Apply(ecsSystems,system);
            }
        }
        
        public void Apply(IProtoSystems ecsSystems,IProtoSystem system)
        {
            var world = ecsSystems.GetWorld();
            if (world == null) return;
            
            var systemType = system.GetType();
            var isDiSystem = systemType.HasAttribute<ECSDIAttribute>();
            var fields = systemType.GetInstanceFields();
            
            foreach (var field in fields)
            {
                var isDiTarget = isDiSystem || Attribute.IsDefined (field, _diAttributeType);
                if(!isDiTarget) continue;

                foreach (var injection in _injections)
                    injection.ApplyInjection(ecsSystems,field,system,_injections);
            }
        }
        
    }
    
    
}
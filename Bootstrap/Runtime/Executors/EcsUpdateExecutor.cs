namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System;
    using System.Collections.Generic;
    using Bootstrap;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using UnityEngine;


    public class EcsUpdateExecutor : MonoBehaviour,IEcsExecutor
    {
        private bool _isActive;
        private ProtoWorld _world;
        
        private List<IProtoSystems> _systems = new List<IProtoSystems>();
        private IEcsSystem[] _allSystems = Array.Empty<IEcsSystem>();

        public bool IsActive => _isActive;
        
        public void Dispose()
        {
            Stop();
            Destroy(gameObject);
        }

        public void Execute(ProtoWorld world)
        {
            _isActive = true;
            _world = world;
        }

        public void Add(IProtoSystems ecsSystems)
        {
            if (_systems.Contains(ecsSystems))
                return;

            _systems.Add(ecsSystems);
        }

        public void Stop()
        {
            _isActive = false;
        }

        private void Update()
        {
            if (!this) return;
            
            var isActive = _world.IsAlive() && Application.isPlaying && _isActive;
            if (!isActive)
                return;
            
            foreach (var systemValue in _systems)
            {
                systemValue.Run();
            }
        }

        private void Awake()
        {
            _systems ??= new List<IProtoSystems>();
            _allSystems ??= Array.Empty<IEcsSystem>();
        }
    }
}
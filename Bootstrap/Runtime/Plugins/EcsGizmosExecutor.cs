namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System.Collections.Generic;
    using Bootstrap;
    using Leopotam.EcsProto;
    using UnityEngine;

    public class EcsGizmosExecutor : MonoBehaviour,ISystemsPlugin
    {
        private bool _isActive;
        private ProtoWorld _world;
        private GameObject _executor;
        
        private List<IProtoSystems> _systems = new();
        private Slice<IProtoSystem> _allSystems = new();
        private Dictionary<ILeoEcsGizmosSystem,IProtoSystems> _gizmosSystems = new();
        
        public void Dispose()
        {
            _gizmosSystems?.Clear();
            Stop();
#if UNITY_EDITOR
            if (this == null || Application.isPlaying == false || gameObject == null) return;
#endif
            Destroy(gameObject);
        }

        public bool IsActive => _isActive;

        public void Execute(ProtoWorld world)
        {
            _isActive = true;
            _world = world;
        }

        public void Add(IProtoSystems ecsSystems)
        {
#if !UNITY_EDITOR
            return; 
#endif
            if (_systems.Contains(ecsSystems)) return;
            
            _systems.Add(ecsSystems);
            _allSystems = ecsSystems.Systems();
            
            var data = _allSystems.Data();
            var len = _allSystems.Len();
            
            for (int i = 0; i < len; i++)
            {
                var system = data[i];
                if (system is ILeoEcsGizmosSystem gizmosSystem)
                    _gizmosSystems[gizmosSystem] = ecsSystems;
            }
        }

        public void Stop()
        {
            _isActive = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var isActive = this != null &&
                           _world.IsAlive() && 
                           Application.isPlaying && 
                           _isActive;
            
            if (!isActive) return;
            
            foreach (var systemValue in _gizmosSystems)
                systemValue.Key.RunGizmosSystem(systemValue.Value);
        }
#endif

        private void Awake()
        {
            _systems ??= new List<IProtoSystems>();
            _allSystems ??= new Slice<IProtoSystem>();
            _gizmosSystems ??= new Dictionary<ILeoEcsGizmosSystem, IProtoSystems>();
        }
    }
}
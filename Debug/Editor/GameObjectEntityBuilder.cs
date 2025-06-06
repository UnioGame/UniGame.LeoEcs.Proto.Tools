namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Collections.Generic;
    using Leopotam.EcsLite;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Shared.Components;
    using Shared.Extensions;

    [Serializable]
    public class GameObjectEntityBuilder : IEntityEditorViewBuilder
    {
        private ProtoWorld _world;
        private ProtoIt _gameObjectFilter;
        private ProtoPool<GameObjectComponent> _gameObjectPool;

        public void Initialize(ProtoWorld world)
        {
            _world = world;
            _gameObjectFilter = world.Filter<GameObjectComponent>().End();
            _gameObjectPool = _world.GetPool<GameObjectComponent>();
        }

        public void Execute(List<EntityEditorView> views)
        {
            foreach (var view in views)
            {
                foreach (var entity in _gameObjectFilter)
                {
                    if ((ProtoEntity)view.id != entity) continue;
                    ref var gameObjectComponent = ref _gameObjectPool.Get(entity);
                    var gameObject = gameObjectComponent.Value;
                    if(gameObject == null) continue;
                    
                    view.gameObject = gameObject;
                    view.name = gameObject.name;
                }
            }
        }

        public void Execute(EntityEditorView view)
        {
            var entityId =  (ProtoEntity)view.id;
            var packed = _world.PackEntity(entityId);
            if(packed.Unpack(_world, out var entity) == false) return;
            
            if (!_gameObjectPool.Has(entityId)) return;
                
            ref var gameObjectComponent = ref _gameObjectPool.Get(entity);
            var gameObject = gameObjectComponent.Value;

            if (gameObject == null) return;
                    
            view.gameObject = gameObject;
            view.name = gameObject.name;
        }
    }
    
}
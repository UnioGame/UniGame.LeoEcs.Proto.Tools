namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Collections.Generic;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Runtime.ObjectPool;
    using Shared.Components;
    using Shared.Extensions;

    [Serializable]
    public class ComponentsEntityBuilder : IEntityEditorViewBuilder
    {
        private ProtoWorld _world;
        private ProtoPool<NameComponent> _namePool;
        private Slice<object> _components;

        public void Initialize(ProtoWorld world)
        {
            _world = world;
            _namePool = _world.GetPool<NameComponent>();
        }

        public void Execute(EntityEditorView view)
        {
            ref var packedEntity = ref view.packedEntity;
            if (!packedEntity.Unpack(_world, out var entity))
            {
                view.isDead = true;
                return;
            }

            if (_namePool.Has(entity))
            {
                ref var nameComponent = ref _namePool.Get(entity);
                view.name = nameComponent.Value;
            }
            
            _components ??= new Slice<object>();
            _world.EntityComponents((ProtoEntity)view.id, _components);

            var len = _components.Len();
            var data = _components.Data();

            for (var i = 0; i < len; i++)
            {
                var component = data[i];
                if(component == null) continue;
                    
                var componentView = ClassPool.Spawn<ComponentEditorView>();
                componentView.entity = (ProtoEntity)view.id;
                componentView.value = component;
                    
                view.components.Add(componentView);
            }
        }
        
        public void Execute(List<EntityEditorView> views)
        {
            foreach (var view in views)
                Execute(view);
        }
    }
}
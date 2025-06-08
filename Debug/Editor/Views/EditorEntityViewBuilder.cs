namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Collections.Generic;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Runtime.ObjectPool;
    using Shared.Extensions;
    using Sirenix.OdinInspector;
    using Runtime.Utils;
    using UnityEngine;

    [Serializable]
    public class EditorEntityViewBuilder
    {
        [SerializeReference]
        [InlineProperty]
        public List<IEntityEditorViewBuilder> viewBuilders = new List<IEntityEditorViewBuilder>()
        {
            new ComponentsEntityBuilder(),
            new GameObjectEntityBuilder(),
        };

        public void Initialize(ProtoWorld world)
        {
            foreach (var viewBuilder in viewBuilders)
                viewBuilder.Initialize(world);
        }

        public EntityEditorView Create(ProtoEntity entity,ProtoWorld world)
        {
            var view = ClassPool.Spawn<EntityEditorView>();
            view.id = (int)entity;
            view.packedEntity = world.PackEntity(entity);
            view.name = view.id.ToStringFromCache();
            
            var packed = world.PackEntity(entity);
            if(packed.Unpack(world, out _) == false) return view;
            
            foreach (var builder in viewBuilders)
                builder.Execute(view);

            return view;
        }
    }
}
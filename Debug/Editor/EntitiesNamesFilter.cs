namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Buffers;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Runtime.ObjectPool.Extensions;
    using Shared.Components;

    [Serializable]
    public class EntitiesNamesFilter : IProtoWorldSearchFilter
    {
        public Slice<ProtoEntity> entities = new();
        public Slice<object> components = new();

        public EcsFilterData Execute(EcsFilterData filterData)
        {
            entities.Clear();
            
            var world = filterData.world;
            world.AliveEntities(entities);
            
            var len = entities.Len();
            var data = entities.Data();

            for (var i = 0; i < len; i++)
            {
                var entity = data[i];
                if(!IsContainFilteredComponent(entity,filterData))
                    continue;
                filterData.entities.Add(entity);
            }
            
            return filterData;
        }

        public bool IsContainFilteredComponent(
            ProtoEntity entity,
            EcsFilterData filterData)
        {
            var filter = filterData.filter;
            if (string.IsNullOrEmpty(filter)) return true;
            
            var world = filterData.world;
            world.EntityComponents(entity,components);
            var found = false;
            
            var len = components.Len();
            var data = components.Data();

            for (var i = 0; i < len; i++)
            {
                var component = data[i];
                if(component == null)continue;
                
                var name = component.GetType().Name;
                if (name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }

                if (component is GameObjectComponent gameObjectComponent)
                {
                    var gameObject = gameObjectComponent.Value;
                    if (gameObject != null)
                    {
                        name = gameObject.name;
                        if (name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (component is not NameComponent nameComponent) continue;
                
                name = nameComponent.Value;
                
                if (!name.Contains(filter, StringComparison.OrdinalIgnoreCase)) continue;
                
                found = true;
                
                break;
            }
            
            return found;
        }
    }
}
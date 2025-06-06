namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Buffers;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Runtime.ObjectPool.Extensions;


    [Serializable]
    public class FilterEntitiesComponents : IProtoWorldSearchFilter
    {
        public Slice<ProtoEntity> entities = new();
        public Slice<object> components = new();

        public EcsFilterData Execute(EcsFilterData filterData)
        {
            var world = filterData.world;
            entities.Clear();
            
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
            if (filter == null) return false;
            
            var world = filterData.world;
            var count = world.ComponentsCount(entity);
            var found = false;
            
            components.Clear();
            
            world.EntityComponents(entity,components);
            
            var len = components.Len();
            var data = components.Data();
            
            for (var i = 0; i < len; i++)
            {
                var item = data[i];
                var type = item?.GetType();
                
                if(type == null) continue;
                if(!type.Name.Contains(filter,StringComparison.OrdinalIgnoreCase))
                    continue;
                
                found = true;
                break;
            }

            return found;
        }
    }
}
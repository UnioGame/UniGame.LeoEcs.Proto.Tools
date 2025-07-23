namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using Core.Runtime.ObjectPool;
    using Leopotam.EcsProto;
    using UniModules.Editor;
    using Runtime.ReflectionUtils;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    
    [Serializable]
    public class ComponentEditorView : IPoolable,ISearchFilterable
    {
        private const string BoxGroupLabel = "@Label";
        private const string ComponentKey = "Component";
        private const string ComponentGroupValue = "Component/Value";
        private const string ComponentGroupActions = "Component/Actions";
        
        #region inspector
        
        [HideInInspector]
        public ProtoEntity entity;
        
        [HideInInspector]
        public ProtoWorld world;
        
        [HorizontalGroup(ComponentKey, 0.75f)]
        [BoxGroup(ComponentGroupValue,LabelText = BoxGroupLabel)]
        [InlineProperty]
        [HideLabel]
        [OnInspectorGUI(Prepend = nameof(DrawComponentPrepend))]
        [HideReferenceObjectPicker]
        public object value;

        #endregion
        
        public string Label => value == null ? ComponentKey : value.GetType().GetFormattedName();

        public ProtoWorld World => world;

        public bool IsAlive => world != null && world.IsAlive();
        
        [BoxGroup(ComponentGroupActions)]
        [Button(nameof(OpenScript),ButtonSizes.Small, Icon = SdfIconType.Folder2Open)]
        public void OpenScript()
        {
            if (value == null) return;
            value.GetType().OpenScript();
        }
        
        [BoxGroup(ComponentGroupActions)]
        [Button(nameof(ApplyChanges),ButtonSizes.Small, Icon = SdfIconType.Magic)]
        public void ApplyChanges()
        {
            if (value == null || !IsAlive) return;

            var world = World;
            
            var pools = world.Pools();
            var count = pools.Len();
            var data = pools.Data();

            for (var i = 0; i < count; i++)
            {
                var pool = data[i];
                
                if(pool == null)continue;
                if(pool.ItemType() != value.GetType()) continue;
                
                pool.SetRaw(entity,value);
                
                break;
            }
        }
        
        [BoxGroup(ComponentGroupActions)]
        [Button(nameof(Remove),ButtonSizes.Small, Icon = SdfIconType.Eraser)]
        public void Remove()
        {
            if (value == null || !IsAlive) return;

            var world = World;
            var pools = world.Pools();
            var count = pools.Len();
            var data = pools.Data();

            for (int i = 0; i < count; i++)
            {
                var pool = data[i];
                if(pool == null)continue;
                if(pool.ItemType() != value.GetType()) continue;
                pool.Del(entity);
            }
                
        }
        
        public void Release()
        {
            value = null;
        }


        private void DrawComponentPrepend()
        {
            
        }

        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            if (value == null) return false;
            return value.GetType().Name.Contains(searchString, StringComparison.OrdinalIgnoreCase);
        }
    }
}
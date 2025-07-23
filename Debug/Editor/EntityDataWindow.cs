namespace UniGame.LeoEcs.Debug.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using Converter.Runtime.Editor;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Proto;
    using Shared.Extensions;
    using Unity.EditorCoroutines.Editor;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif
    
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
#endif
    
    public class EntityDataWindow 
#if ODIN_INSPECTOR
        : OdinEditorWindow
#else
        : EditorWindow
#endif
    {
        #region statics data

        private static Color buttonColor = new Color(0.2f, 1, 0.6f);

        [MenuItem("ECS Proto/Entity Data Window")]
        [MenuItem("Game/Editors/Entity Data Window")]
        public static EntityDataWindow OpenWindow()
        {
            var window = Create(new EntityEditorData()
            {
                entity = new ProtoEntity(),
            });
            window.Show();
            return window;
        }
    
        public static EntityDataWindow OpenPopupWindow(EntityEditorData entityId)
        {
            var window = Create(entityId);
            window.ShowPopup();
            return window;
        }

        public static EntityDataWindow Create(EntityEditorData entityData)
        {
            var window = GetWindow<EntityDataWindow>();
            window.titleContent.text = "Entity Data Window";
            window.entityId = entityData.entity;
            window.world = entityData.world;
            window.worldId = entityData.worldId;
            window.UpdateView();
            return window;
        }

        #endregion

        #region inspector

        [HideInInspector]
        public ProtoWorld world;

#if ODIN_INSPECTOR
        [OnValueChanged(nameof(UpdateView))]
        [InlineButton(nameof(UpdateView),SdfIconType.Arrow90degLeft,"Refresh")]
#endif
        public ProtoEntity entityId;
        
#if ODIN_INSPECTOR
        [OnValueChanged(nameof(SetAutoUpdate))]
#endif
        public bool autoUpdate = false;
        
#if ODIN_INSPECTOR
        [ValueDropdown(valuesGetter:nameof(GetWorlds), IsUniqueList = true, AppendNextDrawer = true)]
#endif
        public string worldId = string.Empty;

#if ODIN_INSPECTOR
        [TitleGroup("Entity View")]
        [HideLabel]
        [InlineProperty]
#endif
        public EntityEditorView entityView;

        #endregion

        private ProtoPackedEntity _packedEntity;
        private EditorEntityViewBuilder _viewBuilder = new();
        private EditorCoroutine _coroutine;
        private float _updateDelay = 1f;

        public void UpdateView()
        {
            var service = LeoEcsGlobalData.Service;
            if (LeoEcsGlobalData.Service == null) return;
            
            world ??= service.World;

            if (string.IsNullOrEmpty(worldId) == false)
            {
                if (!service.Worlds.TryGetValue(worldId, out var worldData))
                    return;
                world = worldData.World;
            }
            
            if (world == null || world.IsAlive() == false) return;
            
            if (world == null) return;

            _packedEntity = world.PackEntity(entityId);
            if(!_packedEntity.Unpack(world, out var entity))
                return;

            _viewBuilder.Initialize(world,worldId);
            
            entityView = _viewBuilder.Create(entityId, world,worldId);
        }

        public IEnumerable<string> GetWorlds()
        {
            if(LeoEcsGlobalData.Service == null) yield break;
            foreach (var world in LeoEcsGlobalData.Service.Worlds)
            {
                yield return world.Key;
            }
        }

        public void SetAutoUpdate(bool enabled)
        {
            StopAutoRefresh();
            
            if (!enabled) return;
            
            EditorCoroutineUtility.StartCoroutine(AutoRefresh(), this);
        }

#if ODIN_INSPECTOR
        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopAutoRefresh();
        }
#endif
        
        private void StopAutoRefresh()
        {
            if (_coroutine == null) return;
            EditorCoroutineUtility.StopCoroutine(_coroutine);
            _coroutine = null;
        }
        
        private IEnumerator AutoRefresh()
        {
            var waitForOneSecond = new EditorWaitForSeconds(_updateDelay);

            while (_coroutine!=null)
            {
                yield return waitForOneSecond;

                UpdateView();
            }
        }

    }
}
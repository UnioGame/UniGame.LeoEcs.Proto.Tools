using UnityEditor;

namespace UniGame.LeoEcs.Debug.Editor
{
    using System.Collections.Generic;
    using Leopotam.EcsProto;
    using Leopotam.EcsProto.QoL;
    using Proto;
    using Runtime.ObjectPool;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class EntitiesBrowserWindow : OdinEditorWindow
    {
        #region statics data

        private static Color buttonColor = new Color(0.2f, 1, 0.6f);

        [MenuItem("ECS Proto/Entities Browser")]
        [MenuItem("Game/Editors/Entities Browser")]
        public static EntitiesBrowserWindow OpenWindow()
        {
            var window = Create();
            window.Show();
            return window;
        }
    
        public static EntitiesBrowserWindow OpenPopupWindow()
        {
            var window = Create();
            window.ShowPopup();
            return window;
        }

        public static EntitiesBrowserWindow Create()
        {
            var window = GetWindow<EntitiesBrowserWindow>();
            window.titleContent.text = "Entities Browser";
            return window;
        }

        #endregion

        #region inspector
        
        [InlineButton(nameof(UpdateFilter),
            nameof(search), Icon = SdfIconType.Search)]
        [HideLabel]
        [EnableIf(nameof(HasProtoWorld))]
        public string search;

        [Space(8)]
        [HorizontalGroup()]
        [LabelWidth(100)]
        [LabelText("world :")]
        [ValueDropdown(valuesGetter:nameof(GetWorlds),IsUniqueList = true,AppendNextDrawer = true)]
        public string worldId = string.Empty;
        
        [Space(8)]
        [HorizontalGroup()]
        [LabelWidth(60)]
        [ReadOnly]
        [LabelText("entities :")]
        public int totalEntities;

        [HideLabel]
        [BoxGroup("entities")]
        public EntityGridEditorView gridEditorView;

        [HideInInspector]
        [InlineProperty]
        [HideLabel]
        [EnableIf(nameof(HasProtoWorld))]
        public EntitiesEditorView view;

        #endregion
        
        private Slice<ProtoEntity> Entities = new();

        public bool HasProtoWorld => World != null;

        public ProtoWorld World
        {
            get
            {
                if(LeoEcsGlobalData.Service == null) return null;
                var service = LeoEcsGlobalData.Service;
                var isDefault = string.IsNullOrEmpty(worldId);
                if (isDefault) return service.DefaultWorld.CurrentValue?.World;
                if (!service.Worlds.TryGetValue(worldId, out var worldData))
                    return null;
                return worldData.World;
            }
        }
        
        [PropertyOrder(-1)]
        [ResponsiveButtonGroup()]
        [GUIColor(nameof(buttonColor))]
        [Button(ButtonSizes.Large,Icon = SdfIconType.ArrowClockwise)]
        public void Refresh()
        {
            view = new EntitiesEditorView();
            view.Initialize(World,worldId);
            
            Clear();
            
            if(HasProtoWorld && World.IsAlive()) UpdateFilter();
        }

        public void UpdateFilter()
        {
            if(!EntitiesEditorView.IsInitialized)
                view.Initialize(World,worldId);
            
            gridEditorView.items.Clear();
            
            view.UpdateFilter(search);

            gridEditorView.items.AddRange(view.entities);
            
            World.AliveEntities(Entities);

            totalEntities = Entities.Len();
        }

        public IEnumerable<string> GetWorlds()
        {
            var service = LeoEcsGlobalData.Service;
            if(service == null) yield break;
            foreach (var world in service.Worlds)
            {
                yield return world.Key;
            }
        }


        private void Clear()
        {
            gridEditorView.items.Clear();
        }

        protected override void Initialize()
        {
            base.Initialize();

            gridEditorView ??= new EntityGridEditorView();
            
            Refresh();
        }

        private string testName = "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest";
        public void Fill(int amount)
        {
            var items = gridEditorView.items;
            items.Clear();
            
            for (int j = 0; j < amount; j++)
            {
                var id = j;
                var item = ClassPool.Spawn<EntityIdEditorView>();
                item.id = id;
                item.world = World;
                item.name = testName.Substring(0,Random.Range(1,testName.Length));
                items.Add(item);
            }
        }
    }
}


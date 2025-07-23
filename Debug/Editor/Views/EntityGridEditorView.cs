namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.ObjectPool;
    using Leopotam.EcsProto;
    using Sirenix.OdinInspector;

    [Serializable]
    public class EntityGridEditorView
    {
        [HideLabel]
        public List<IEntityEditorView> items = new List<IEntityEditorView>();
    }

    [Serializable]
    [InlineProperty]
    [HideLabel]
    public class EntityIdEditorView : IPoolable, IEntityEditorView
    {
        public string name;
        public int id;
        public ProtoWorld world;

        public ProtoWorld World => world;

        public int Id => id;

        public string Name => name;

        public void Release()
        {
            name = string.Empty;
            id = -1;
        }

        public void Show()
        {
            
        }
    }
}
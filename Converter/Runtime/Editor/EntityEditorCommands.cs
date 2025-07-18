namespace UniGame.LeoEcs.Converter.Runtime.Editor
{
    using System;
    using Leopotam.EcsProto;

    public static class EntityEditorCommands
    {

        public static Action<EntityEditorData> OnEntityInfoRequested;

        public static void OpenEntityInfo(EntityEditorData entityId)
        {
            OnEntityInfoRequested?.Invoke(entityId);
        }
        
    }

    public struct EntityEditorData
    {
        public ProtoEntity entity;
        public string worldId;
        public ProtoWorld world;
    }
}
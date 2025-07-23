namespace UniGame.LeoEcs.Debug.Editor
{
    using Leopotam.EcsProto;

    public interface IEntityEditorView
    {
        public ProtoWorld World { get; }
        public int Id { get; }
        public string Name { get; }
        public void Show();
    }
}
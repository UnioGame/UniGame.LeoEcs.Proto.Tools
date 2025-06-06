namespace Game.Ecs.Core.Components
{
    using Leopotam.EcsProto;
    using UniGame.LeoEcs.Proto;
    using UnityEngine;

    /// <summary>
    /// Угол вращения при перемещении.
    /// </summary>
    public struct RotationComponent : IProtoAutoReset<RotationComponent>
    {
        public Quaternion Value;
        
        public void SetHandlers(IProtoPool<RotationComponent> pool) => pool.SetResetHandler(AutoReset);
        
        public static void AutoReset(ref RotationComponent c)
        {
            c.Value = Quaternion.identity;
        }
    }
}
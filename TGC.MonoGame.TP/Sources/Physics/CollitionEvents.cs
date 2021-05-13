using BepuPhysics;
using System.Collections.Generic;

namespace TGC.MonoGame.TP.Physics
{
    class CollitionEvents
    {
        private readonly Dictionary<StaticHandle, ICollitionHandler> collidersS = new Dictionary<StaticHandle, ICollitionHandler>();
        private readonly Dictionary<BodyHandle, ICollitionHandler> collidersB = new Dictionary<BodyHandle, ICollitionHandler>();

        internal void RegisterCollider(StaticHandle handle, ICollitionHandler handler) => collidersS.Add(handle, handler);
        internal void RegisterCollider(BodyHandle handle, ICollitionHandler handler) => collidersB.Add(handle, handler);

        internal ICollitionHandler GetHandler(StaticHandle handle) => collidersS.GetValueOrDefault(handle);
        internal ICollitionHandler GetHandler(BodyHandle handle) => collidersB.GetValueOrDefault(handle);
    }
}
using BepuPhysics;
using System.Collections.Generic;

namespace TGC.MonoGame.TP.Physics
{
    class CollitionEvents
    {
        private readonly Dictionary<BodyHandle, ICollitionHandler> colliders = new Dictionary<BodyHandle, ICollitionHandler>();

        internal ICollitionHandler GetHandler(BodyHandle handle) => colliders[handle];

        internal void RegisterCollider(BodyHandle handle, ICollitionHandler handler) => colliders.Add(handle, handler);
    }
}
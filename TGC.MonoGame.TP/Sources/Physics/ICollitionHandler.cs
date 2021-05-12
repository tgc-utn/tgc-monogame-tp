using BepuPhysics;

namespace TGC.MonoGame.TP.Physics
{
    internal interface ICollitionHandler
    {
        bool HandleCollition(ICollitionHandler other);
    }
}
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace WarSteel.Entities;

public struct OVector3
{

    public Vector3 Origin;
    public Vector3 Vector;

    public OVector3(Vector3 o, Vector3 a)
    {
        Origin = o;
        Vector = a;
    }

}








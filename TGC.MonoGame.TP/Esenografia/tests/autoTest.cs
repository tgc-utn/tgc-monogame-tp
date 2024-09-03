
using Microsoft.Xna.Framework;

namespace Escenografia.TESTS
{
    class Auto : Escenografia
    {
        public Auto(Vector3 posicion)
        {
            this.posicion = posicion;
        }
        public override Matrix getWorldMatrix()
        {
            return Matrix.CreateWorld(posicion, Vector3.Forward, Vector3.Up);
        }
    }
}
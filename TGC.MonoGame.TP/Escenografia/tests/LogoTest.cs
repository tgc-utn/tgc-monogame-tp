using Microsoft.Xna.Framework;
namespace Escenografia
{
    namespace TESTS
    {
        class LogoTest : Escenografia3D
        {
            public override Matrix getWorldMatrix()
            {
                return Matrix.Identity;
            }
        }

    }
}
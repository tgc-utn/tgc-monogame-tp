using Microsoft.Xna.Framework;
namespace Escenografia
{
    namespace TESTS
    {
        class LogoTest : Escenografia
        {
            public override Matrix getWorldMatrix()
            {
                return Matrix.Identity;
            }
        }

    }
}
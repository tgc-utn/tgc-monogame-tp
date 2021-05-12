using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    internal static class Input
    {
        private static KeyboardState KeyboardState => Keyboard.GetState();

        private static bool MovingRight() => KeyboardState.IsKeyDown(Keys.D) || KeyboardState.IsKeyDown(Keys.Right);
        private static bool MovingLeft() => KeyboardState.IsKeyDown(Keys.A) || KeyboardState.IsKeyDown(Keys.Left);

        private static bool MovingForward() => KeyboardState.IsKeyDown(Keys.W) || KeyboardState.IsKeyDown(Keys.Up);
        private static bool MovingBackward() => KeyboardState.IsKeyDown(Keys.S) || KeyboardState.IsKeyDown(Keys.Down);

        private static bool MovingUp() => KeyboardState.IsKeyDown(Keys.Space);
        private static bool MovingDown() => KeyboardState.IsKeyDown(Keys.LeftControl);

        private static int BoolToInt(bool b) => b ? 1 : 0;
        private static int BoolsToAxis(bool positive, bool negative) => BoolToInt(positive) - BoolToInt(negative);

        internal static int HorizontalAxis() => BoolsToAxis(MovingRight(), MovingLeft());
        internal static int VerticalAxis() => BoolsToAxis(MovingUp(), MovingDown());
        internal static int ForwardAxis() => BoolsToAxis(MovingForward(), MovingBackward());

        internal static bool Turbo() => KeyboardState.IsKeyDown(Keys.LeftShift);

        internal static bool Exit() => KeyboardState.IsKeyDown(Keys.Escape);
    }
}
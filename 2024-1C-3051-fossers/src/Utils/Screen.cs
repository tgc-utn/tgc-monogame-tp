using Microsoft.Xna.Framework;

namespace WarSteel.Utils;

public static class Screen
{
    // Function to get the center of the screen
    public static Vector2 GetScreenCenter(GraphicsDeviceManager graphics)
    {
        int screenWidth = graphics.PreferredBackBufferWidth;
        int screenHeight = graphics.PreferredBackBufferHeight;
        return new Vector2(screenWidth / 2f, screenHeight / 2f);
    }

    public static int GetScreenHeight(GraphicsDeviceManager graphics)
    {
        return graphics.GraphicsDevice.Viewport.Height;
    }

    public static int GetScreenWidth(GraphicsDeviceManager graphics)
    {
        return graphics.GraphicsDevice.Viewport.Width;
    }
}
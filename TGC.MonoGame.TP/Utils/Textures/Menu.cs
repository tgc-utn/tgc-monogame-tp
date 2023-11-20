using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Textures;

public static class Menu
{
    public static readonly TextureReference ButtonNormal = new TextureReference(
        $"{ContentFolder.Images}/button");
    public static readonly TextureReference ButtonHover = new TextureReference(
        $"{ContentFolder.Images}/hoverbutton");
    public static readonly TextureReference MenuImage = new TextureReference(
        $"{ContentFolder.Images}/main");
    public static readonly TextureReference GameOver = new TextureReference(
        $"{ContentFolder.Images}/gameover");
    public static readonly TextureReference Win = new TextureReference(
        $"{ContentFolder.Images}/win");
}
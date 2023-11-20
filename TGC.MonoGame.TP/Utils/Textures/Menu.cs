using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Textures;

public static class Menu
{
    public static readonly BasicTextureReference ButtonNormal = new (
        $"{ContentFolder.Images}/button");
    public static readonly BasicTextureReference ButtonHover = new (
        $"{ContentFolder.Images}/hoverbutton");
    public static readonly BasicTextureReference MenuImage = new (
        $"{ContentFolder.Images}/main");
}
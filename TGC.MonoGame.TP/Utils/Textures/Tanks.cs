using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Textures;

public class Tanks
{
    public static readonly ShadowTextureReference KF51 = new (
        $"{ContentFolder.Models}/tanks/kf51/textures/Panther_KF51_Body_Desert_BaseColor.tga"
    );
    
    public static readonly ShadowTextureReference T90 = new (
        $"{ContentFolder.Models}/tanks/T90/textures_mod/hullA"
    );
    
    public static readonly ShadowTextureReference T90V2 = new (
        $"{ContentFolder.Models}/tanks/T90/textures_mod/hullC"
    );
}
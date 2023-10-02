using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Textures;

public class Tanks
{
    public static readonly TextureReference KF51 = new TextureReference(
        $"{ContentFolder.Models}/tanks/kf51/textures/Panther_KF51_Body_Desert_BaseColor.tga"
    );
    
    public static readonly TextureReference T90 = new TextureReference(
        $"{ContentFolder.Models}/tanks/T90/textures_mod/hullA"
    );
    
    public static readonly TextureReference T90V2 = new TextureReference(
        $"{ContentFolder.Models}/tanks/T90/textures_mod/hullC"
    );
    
    /*
     public static readonly TextureReference Panzer = new TextureReference(
        $"{ContentFolder.Textures}/tanks/Panzer/Panzer"
    );
    */
}
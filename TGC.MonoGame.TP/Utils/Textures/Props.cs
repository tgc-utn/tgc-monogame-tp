using System.Collections.Generic;
using System.Net.Mime;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Textures;

public static class Props
{
    public static readonly TextureReference BuildingHouse = new TextureReference(
        $"{ContentFolder.Textures}/props/structures/building");

    public static readonly TextureReference Farm = new TextureReference(
        $"{ContentFolder.Textures}/props/farm/Farm16K");
    
    public static TextureReference Rock0 = new TextureReference(
        $"{ContentFolder.Textures}/props/stones/Rock_11_pbr_pbr_diffuse");
    
    public static TextureReference Rock1 = new TextureReference(
        $"{ContentFolder.Textures}/props/stones/Rock_14_pbr_pbr_diffuse");
    
    public static TextureReference Rock2 = new TextureReference(
        $"{ContentFolder.Textures}/props/stones/Rock_15_pbr_pbr_diffuse");
    
    public static TextureReference Wall1 = new TextureReference(
        $"{ContentFolder.Textures}/props/walls/Wall_Angled_02_Albedo");
    
    public static TextureReference Wall2 = new TextureReference(
        $"{ContentFolder.Textures}/props/walls/Wall_Angled_03_Albedo");

    public static TextureReference Bullet = new TextureReference(
            $"{ContentFolder.Textures}/props/bullet/bullet_BaseColor");
}
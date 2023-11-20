using System.Collections.Generic;
using System.Net.Mime;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Textures;

public static class Props
{
    public static readonly ShadowTextureReference BuildingHouse = new (
        $"{ContentFolder.Textures}/props/structures/building");

    public static readonly ShadowTextureReference Farm = new (
        $"{ContentFolder.Textures}/props/farm/Farm16K");
    
    public static readonly ShadowTextureReference Rock0 = new (
        $"{ContentFolder.Textures}/props/stones/Rock_11_pbr_pbr_diffuse");
    
    public static readonly ShadowTextureReference Rock1 = new (
        $"{ContentFolder.Textures}/props/stones/Rock_14_pbr_pbr_diffuse");
    
    public static readonly ShadowTextureReference Rock2 = new (
        $"{ContentFolder.Textures}/props/stones/Rock_15_pbr_pbr_diffuse");
    
    public static readonly ShadowTextureReference Wall1 = new (
        $"{ContentFolder.Textures}/props/walls/Wall_Angled_02_Albedo");
    
    public static readonly ShadowTextureReference Wall2 = new (
        $"{ContentFolder.Textures}/props/walls/Wall_Angled_03_Albedo");

    public static readonly BasicTextureReference Bullet = new (
        $"{ContentFolder.Textures}/props/bullet/bullet_BaseColor");
}
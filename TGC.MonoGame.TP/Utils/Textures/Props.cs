using System.Collections.Generic;
using System.Net.Mime;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Textures;

public static class Props
{
    public static readonly ShadowBlingPhongReference BuildingHouse = new (
        $"{ContentFolder.Textures}/props/structures/building");

    public static readonly ShadowBlingPhongReference Farm = new (
        $"{ContentFolder.Textures}/props/farm/Farm16K");
    
    public static readonly ShadowBlingPhongReference Rock0 = new (
        $"{ContentFolder.Textures}/props/stones/Rock_11_pbr_pbr_diffuse");
    
    public static readonly ShadowBlingPhongReference Rock1 = new (
        $"{ContentFolder.Textures}/props/stones/Rock_14_pbr_pbr_diffuse");
    
    public static readonly ShadowBlingPhongReference Rock2 = new (
        $"{ContentFolder.Textures}/props/stones/Rock_15_pbr_pbr_diffuse");
    
    public static readonly ShadowBlingPhongReference Wall1 = new (
        $"{ContentFolder.Textures}/props/walls/Wall_Angled_02_Albedo");
    
    public static readonly ShadowBlingPhongReference Wall2 = new (
        $"{ContentFolder.Textures}/props/walls/Wall_Angled_03_Albedo");

    public static readonly ShadowBlingPhongReference Bullet = new (
        $"{ContentFolder.Textures}/props/bullet/bullet_BaseColor");
}
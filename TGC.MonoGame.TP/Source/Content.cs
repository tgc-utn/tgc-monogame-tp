using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Geometries;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class Content
    {
         #region constantes
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        #endregion

        private readonly ContentManager ContentManager;
        internal Model 
                M_Alfil, M_Torre, M_Auto, M_AutoEnemigo, M_Inodoro, M_Misil, M_SillaOficina, M_CafeRojo, M_Silla, M_Mesa, M_Sillon, 
                M_Televisor , M_MuebleTV, M_Planta , M_Escritorio, M_Cocine, M_Plantis, M_Lego, M_Baniera,M_Sofa, M_Mesita, M_Aparador, 
                M_Bacha, M_Organizador, M_Cajonera, M_CamaMarinera , M_MesadaCentral, M_MesadaLateral, M_MesadaLateral2, M_Alacena, 
                M_Botella, M_Maceta, M_Maceta2, M_Maceta3, M_Maceta4, M_Olla, M_ParedCocina, M_Plato, M_PlatoGrande, M_PlatosApilados,
                M_Mesada, M_AutoPegni, M_Heladera, M_Dragon, M_Dragona
                ;

        internal readonly Effect E_BasicShader, E_TextureShader, E_SpiralShader, E_BlacksFilter, E_TextureMirror;
        internal readonly Texture2D T_Alfombra, T_PisoMadera, T_PisoCeramica, T_PisoAlfombrado, 
                            T_MeshFilter, T_MaderaNikari, T_SillaOficina, T_PisoMaderaClaro, T_Dragon,
                            T_RacingCar, T_CombatVehicle, T_Ladrillos, T_Marmol, T_MarmolNegro, T_Reboque, T_Concreto;
        internal readonly List<Effect> Efectos = new List<Effect>();
        internal readonly Song S_SynthWars;
        internal readonly QuadPrimitive G_Quad;
        
        internal Content(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            ContentManager = Content;
            ContentManager.RootDirectory = "Content";

            // Geometrias
            G_Quad = new QuadPrimitive(GraphicsDevice);

            // Efectos
            Efectos.Add(E_BasicShader   = LoadEffect("BasicShader")         );
            Efectos.Add(E_TextureShader = LoadEffect("TextureShader")       );
            Efectos.Add(E_TextureMirror = LoadEffect("TextureMirrorShader") );
            Efectos.Add(E_SpiralShader  = LoadEffect("SpiralShader")        );
            Efectos.Add(E_BlacksFilter  = LoadEffect("BlacksFilter")        );


            // Texturas
            T_Alfombra          = LoadTexture("Alfombra");
            T_Concreto          = LoadTexture("Concreto");
            T_Ladrillos         = LoadTexture("Bricks");
            T_Marmol            = LoadTexture("Marmol");
            T_MarmolNegro       = LoadTexture("MarmolNegro");
            T_Reboque           = LoadTexture("Reboque");
            T_MeshFilter        = LoadTexture("MeshFilter");
            T_MaderaNikari      = LoadTexture("MaderaNikari");
            T_PisoMadera        = LoadTexture("PisoMadera");
            T_PisoMaderaClaro   = LoadTexture("PisoMaderaClaro");
            T_PisoCeramica      = LoadTexture("PisoCeramica");
            T_PisoAlfombrado    = LoadTexture("PisoAlfombra");
            T_SillaOficina      = LoadTexture("Muebles/SillaOficina");
            T_Dragon            = LoadTexture("Muebles/Dragon");
            T_CombatVehicle     = LoadTexture("Autos/CombatVehicle");
            T_RacingCar         = LoadTexture("Autos/RacingCarMetalic");

            // Sonidos
            S_SynthWars         = LoadSong("SynthWars");
            
            #region Modelos ( Shader , CarpetaUbicacion, Etiqueta )
            M_Auto              = LoadModel("Autos/", "RacingCar"     );
            M_AutoPegni         = LoadModel("Autos/", "PegniZonda"    );
            M_AutoEnemigo       = LoadModel("Autos/", "CombatVehicle" );
            
            M_Misil             = LoadModel("Muebles/", "Misil"       );
            
            //Living
            M_Silla             = LoadModel("Muebles/", "Chair"       );
            M_Mesa              = LoadModel("Muebles/", "Mesa"        );
            M_Sillon            = LoadModel("Muebles/", "Sillon"      );
            M_Televisor         = LoadModel("Muebles/", "Televisor"   );
            M_MuebleTV          = LoadModel("Muebles/", "MuebleTV"    );
            M_Mesita            = LoadModel("Muebles/", "Mesita"      );
            M_Sofa              = LoadModel("Muebles/", "Sofa"        );
            M_Aparador          = LoadModel("Muebles/", "Aparador"    );

            //Oficina
            M_SillaOficina      = LoadModel("Muebles/", "SillaOficina");
            M_CafeRojo          = LoadModel("Muebles/", "Cafe-Rojo"   );
            M_Escritorio        = LoadModel("Muebles/", "Escritorio"  );
            M_Planta            = LoadModel("Muebles/", "Planta"      );
            M_Plantis           = LoadModel("Muebles/", "Plantis"     );

            //Dormitorios
            M_CamaMarinera      = LoadModel("Muebles/", "CamaMarinera");
            M_Cajonera          = LoadModel("Muebles/", "Cajonera"    );
            M_Organizador       = LoadModel("Muebles/", "Organizador" );
            M_Lego              = LoadModel("Muebles/", "Lego"        );
            M_Alfil             = LoadModel("Muebles/", "Alfil"       );
            M_Torre             = LoadModel("Muebles/", "Torre"       );
            M_Dragon            = LoadModel("Muebles/", "Dragon"      );
            M_Dragona           = LoadModel("Muebles/", "Dragona"     );
            
            //Baño
            M_Baniera           = LoadModel("Muebles/", "Baniera"     );
            M_Bacha             = LoadModel("Muebles/", "Bacha"       );
            M_Inodoro           = LoadModel("Muebles/", "Inodoro"     );
        
            //Cocina
            M_Cocine            = LoadModel("Muebles/", "Cocine"      );
            M_MesadaCentral     = LoadModel("Muebles/SetCocina/", "MesadaCentral");
            M_MesadaLateral     = LoadModel("Muebles/SetCocina/", "MesadaLateral");
            M_MesadaLateral2    = LoadModel("Muebles/SetCocina/", "MesadaLateral2");
            M_Mesada            = LoadModel("Muebles/SetCocina/", "Mesada");
            M_Alacena           = LoadModel("Muebles/SetCocina/", "Alacena");
            M_Botella           = LoadModel("Muebles/SetCocina/", "Botella");
            M_Maceta            = LoadModel("Muebles/SetCocina/", "Maceta");
            M_Maceta2           = LoadModel("Muebles/SetCocina/", "Maceta2");
            M_Maceta3           = LoadModel("Muebles/SetCocina/", "Maceta3");
            M_Maceta4           = LoadModel("Muebles/SetCocina/", "Maceta4");
            M_Olla              = LoadModel("Muebles/SetCocina/", "Olla");
            M_ParedCocina       = LoadModel("Muebles/SetCocina/", "ParedCocina");
            M_Plato             = LoadModel("Muebles/SetCocina/", "Plato");
            M_PlatoGrande       = LoadModel("Muebles/SetCocina/", "PlatoGrande");
            M_PlatosApilados    = LoadModel("Muebles/SetCocina/", "PlatosApilados");
            M_Heladera          = LoadModel("Muebles/SetCocina/", "Heladera");
            #endregion
        }

        public Model LoadModel(string dir) => ContentManager.Load<Model>(ContentFolder3D + dir);
        public Effect LoadEffect(string dir) => ContentManager.Load<Effect>(ContentFolderEffects + dir);
        public Texture2D LoadTexture(string dir) => ContentManager.Load<Texture2D>(ContentFolderTextures + dir);
        public Song LoadSong(string dir) => ContentManager.Load<Song>(ContentFolderMusic + dir);
        public Model LoadModel(string carpeta, string tag){
            var model = LoadModel(carpeta+tag+'/'+tag);
            model.Tag = tag;
            return model;
        }
    }
}
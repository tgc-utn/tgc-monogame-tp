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
        internal readonly Model 
                M_Alfil, M_Torre, M_Auto, M_AutoEnemigo, M_Inodoro, M_Misil, M_SillaOficina, M_Cafe, M_Silla, M_Mesa, M_Sillon, 
                M_Televisor1 , M_MuebleTV, M_Planta , M_Escritorio, M_Cocine, M_Plantis, M_Lego, M_Baniera,M_Sofa, M_Mesita, M_Aparador, 
                M_Bacha, M_Organizador, M_Cajonera, M_CamaMarinera , M_MesadaCentral, M_MesadaLateral, M_MesadaLateral2, M_Alacena, 
                M_Botella, M_Maceta, M_Maceta2, M_Maceta3, M_Maceta4, M_Olla, M_ParedCocina, M_Plato, M_PlatoGrande, M_PlatosApilados,
                M_Mesada, M_AutoPegni, M_Heladera, M_Dragon, M_Dragona
                ;

        internal readonly Effect E_BasicShader, E_TextureShader, E_SpiralShader, E_BlacksFilter, E_TextureMirror;
        internal readonly Texture2D T_Alfombra, T_PisoMadera, T_PisoCeramica, T_PisoAlfombrado, T_MeshFilter;
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
            Efectos.Add(E_BasicShader       = LoadEffect("BasicShader")         );
            Efectos.Add(E_TextureShader     = LoadEffect("TextureShader")       );
            Efectos.Add(E_TextureMirror     = LoadEffect("TextureMirrorShader") );
            Efectos.Add(E_SpiralShader      = LoadEffect("SpiralShader")        );
            Efectos.Add(E_BlacksFilter      = LoadEffect("BlacksFilter")        );


            // Texturas
            T_Alfombra          = LoadTexture("Alfombra");
            T_PisoMadera        = LoadTexture("PisoMadera");
            T_PisoCeramica      = LoadTexture("PisoCeramica");
            T_PisoAlfombrado    = LoadTexture("PisoAlfombra");
            T_MeshFilter        = LoadTexture("MeshFilter");

            // Sonidos
            S_SynthWars         = LoadSong("SynthWars");
            
            // Modelos
            M_Auto              = LoadModel(E_BasicShader, "RacingCar/RacingCar");
            M_AutoPegni         = LoadModel(E_BasicShader, "Autos/PegniZonda/PegniZonda");
            M_AutoEnemigo       = LoadModel(E_BasicShader, "CombatVehicle/Vehicle");
            
            M_Inodoro           = LoadModel(E_BasicShader, "Muebles/inodoro/inodoro");
            M_Misil             = LoadModel(E_BasicShader, "Muebles/Misil/Misil");
            M_SillaOficina      = LoadModel(E_BasicShader, "Muebles/Silla-Oficina/Silla-Oficina");
            M_Cafe              = LoadModel(E_BasicShader, "Muebles/Cafe-Rojo/Cafe-Rojo");
            M_Silla             = LoadModel(E_BasicShader, "Muebles/chair/chair");
            M_Mesa              = LoadModel(E_BasicShader, "Muebles/mesa/mesa");
            M_Sillon            = LoadModel(E_BasicShader, "Muebles/Sillon/Sillon");
            M_Televisor1        = LoadModel(E_BasicShader, "Muebles/Televisor1/Televisor");
            M_MuebleTV          = LoadModel(E_BasicShader, "Muebles/MuebleTV/MuebleTV"); 
            M_Planta            = LoadModel(E_BasicShader, "Muebles/Planta/Planta");
            M_Escritorio        = LoadModel(E_BasicShader, "Muebles/Escritorio/Escritorio");
            M_Alfil             = LoadModel(E_BasicShader, "Muebles/Alfil/Alfil");
            M_Torre             = LoadModel(E_BasicShader, "Muebles/Torre/Torre");
            M_Plantis           = LoadModel(E_BasicShader, "Muebles/Plantis/Plantis");
            M_Cocine            = LoadModel(E_BasicShader, "Muebles/Cocine/Cocine");
            M_Lego              = LoadModel(E_BasicShader, "Muebles/Lego/Lego");
            M_Baniera           = LoadModel(E_BasicShader, "Muebles/Baniera/Baniera");
            M_Mesita            = LoadModel(E_BasicShader, "Muebles/Mesita/Mesita");
            M_Sofa              = LoadModel(E_BasicShader, "Muebles/Sofa/Sofa");
            M_Aparador          = LoadModel(E_BasicShader, "Muebles/Aparador/Aparador");
            M_Bacha             = LoadModel(E_BasicShader, "Muebles/Bacha/Bacha");
            M_Organizador       = LoadModel(E_BasicShader, "Muebles/Organizador/Organizador");
            M_Cajonera          = LoadModel(E_BasicShader, "Muebles/Cajonera/Cajonera");
            M_CamaMarinera      = LoadModel(E_BasicShader, "Muebles/CamaMarinera/CamaMarinera");
            M_Dragon            = LoadModel(E_BasicShader, "Muebles/Dragon/Dragon");
            M_Dragona           = LoadModel(E_BasicShader, "Muebles/Dragona/Dragona");
        
            #region SetCocina
            M_MesadaCentral     = LoadModel(E_BasicShader, "Muebles/SetCocina/MesadaCentral");
            M_MesadaLateral     = LoadModel(E_BasicShader, "Muebles/SetCocina/MesadaLateral");
            M_MesadaLateral2    = LoadModel(E_BasicShader, "Muebles/SetCocina/MesadaLateral2");
            M_Mesada            = LoadModel(E_BasicShader, "Muebles/SetCocina/Mesada");
            M_Alacena           = LoadModel(E_BasicShader, "Muebles/SetCocina/Alacena");
            M_Botella           = LoadModel(E_BasicShader, "Muebles/SetCocina/Botella");
            M_Maceta            = LoadModel(E_BasicShader, "Muebles/SetCocina/Maceta");
            M_Maceta2           = LoadModel(E_BasicShader, "Muebles/SetCocina/Maceta2");
            M_Maceta3           = LoadModel(E_BasicShader, "Muebles/SetCocina/Maceta3");
            M_Maceta4           = LoadModel(E_BasicShader, "Muebles/SetCocina/Maceta4");
            M_Olla              = LoadModel(E_BasicShader, "Muebles/SetCocina/Olla");
            M_ParedCocina       = LoadModel(E_BasicShader, "Muebles/SetCocina/ParedCocina");
            M_Plato             = LoadModel(E_BasicShader, "Muebles/SetCocina/Plato");
            M_PlatoGrande       = LoadModel(E_BasicShader, "Muebles/SetCocina/PlatoGrande");
            M_PlatosApilados    = LoadModel(E_BasicShader, "Muebles/SetCocina/PlatosApilados");
            M_Heladera          = LoadModel(E_BasicShader, "Muebles/SetCocina/Heladera");
            #endregion
        }

        public Model LoadModel(string dir) => ContentManager.Load<Model>(ContentFolder3D + dir);
        public Effect LoadEffect(string dir) {
            Effect ef = ContentManager.Load<Effect>(ContentFolderEffects + dir);
            return ef;
        }
        public Texture2D LoadTexture(string dir) => ContentManager.Load<Texture2D>(ContentFolderTextures + dir);
        public Song LoadSong(string dir) => ContentManager.Load<Song>(ContentFolderMusic + dir);
           

        // PARA CARGAR UN MODELO CON SU EFECTO
        // Si se carga de otra forma el efecto que se quiera, va a romper
        public Model LoadModel(Effect shader, string dir){
            var model = LoadModel(dir);

            shader.Parameters["DiffuseColor"]?.SetValue(Color.Red.ToVector3());

            foreach(var mesh in model.Meshes)
            foreach(var meshPart in mesh.MeshParts)
                meshPart.Effect= shader;
            return model;
        }
    }
}
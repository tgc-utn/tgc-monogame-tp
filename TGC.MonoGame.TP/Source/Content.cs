using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Geometries;

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

        internal readonly Effect E_BasicShader, E_TextureShader, E_SpiralShader, E_BlacksFilter;
        internal readonly Texture2D T_Alfombra, T_PisoMadera;
        internal readonly QuadPrimitive G_Quad;
        
        internal Content(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            ContentManager = Content;
            ContentManager.RootDirectory = "Content";
            
            //Geometrias
            G_Quad = new QuadPrimitive(GraphicsDevice);

            //Efectos
            E_BasicShader       = LoadEffect("BasicShader");
            E_TextureShader     = LoadEffect("TextureShader");
            E_SpiralShader      = LoadEffect("SpiralShader");
            E_BlacksFilter      = LoadEffect("BlacksFilter");

            //texturas
            T_Alfombra          = LoadTexture("Alfombra");
            T_PisoMadera        = LoadTexture("PisoMadera");

            //Modelos
            M_Auto              = LoadModel("RacingCar/RacingCar");
            M_AutoPegni         = LoadModel("Autos/PegniZonda/PegniZonda");
            M_AutoEnemigo       = LoadModel("CombatVehicle/Vehicle");

            M_Inodoro           = LoadModel("Muebles/inodoro/inodoro");
            M_Misil             = LoadModel("Muebles/Misil/Misil");
            M_SillaOficina      = LoadModel("Muebles/Silla-Oficina/Silla-Oficina");
            M_Cafe              = LoadModel("Muebles/Cafe-Rojo/Cafe-Rojo");
            M_Silla             = LoadModel("Muebles/chair/chair");
            M_Mesa              = LoadModel("Muebles/mesa/mesa");
            M_Sillon            = LoadModel("Muebles/Sillon/Sillon");
            M_Televisor1        = LoadModel("Muebles/Televisor1/Televisor");
            M_MuebleTV          = LoadModel("Muebles/MuebleTV/MuebleTV"); 
            M_Planta            = LoadModel("Muebles/Planta/Planta");
            M_Escritorio        = LoadModel("Muebles/Escritorio/Escritorio");
            M_Alfil             = LoadModel("Muebles/Alfil/Alfil");
            M_Torre             = LoadModel("Muebles/Torre/Torre");
            M_Plantis           = LoadModel("Muebles/Plantis/Plantis");
            M_Cocine            = LoadModel("Muebles/Cocine/Cocine");
            M_Lego              = LoadModel("Muebles/Lego/Lego", E_BasicShader);
            M_Baniera           = LoadModel("Muebles/Baniera/Baniera");
            M_Mesita            = LoadModel("Muebles/Mesita/Mesita");
            M_Sofa              = LoadModel("Muebles/Sofa/Sofa");
            M_Aparador          = LoadModel("Muebles/Aparador/Aparador");
            M_Bacha             = LoadModel("Muebles/Bacha/Bacha");
            M_Organizador       = LoadModel("Muebles/Organizador/Organizador");
            M_Cajonera          = LoadModel("Muebles/Cajonera/Cajonera");
            M_CamaMarinera      = LoadModel("Muebles/CamaMarinera/CamaMarinera");
            M_Dragon            = LoadModel("Muebles/Dragon/Dragon");
            M_Dragona           = LoadModel("Muebles/Dragona/Dragona");
            


            #region SetCocina
            M_MesadaCentral     = LoadModel("Muebles/SetCocina/MesadaCentral");
            M_MesadaLateral     = LoadModel("Muebles/SetCocina/MesadaLateral");
            M_MesadaLateral2    = LoadModel("Muebles/SetCocina/MesadaLateral2");
            M_Mesada            = LoadModel("Muebles/SetCocina/Mesada");
            M_Alacena           = LoadModel("Muebles/SetCocina/Alacena");
            M_Botella           = LoadModel("Muebles/SetCocina/Botella");
            M_Maceta            = LoadModel("Muebles/SetCocina/Maceta");
            M_Maceta2           = LoadModel("Muebles/SetCocina/Maceta2");
            M_Maceta3           = LoadModel("Muebles/SetCocina/Maceta3");
            M_Maceta4           = LoadModel("Muebles/SetCocina/Maceta4");
            M_Olla              = LoadModel("Muebles/SetCocina/Olla");
            M_ParedCocina       = LoadModel("Muebles/SetCocina/ParedCocina");
            M_Plato             = LoadModel("Muebles/SetCocina/Plato");
            M_PlatoGrande       = LoadModel("Muebles/SetCocina/PlatoGrande");
            M_PlatosApilados    = LoadModel("Muebles/SetCocina/PlatosApilados");
            M_Heladera          = LoadModel("Muebles/SetCocina/Heladera");
            #endregion
        }

        public Model LoadModel(string dir) => ContentManager.Load<Model>(ContentFolder3D + dir);
        public Effect LoadEffect(string dir) => ContentManager.Load<Effect>(ContentFolderEffects + dir);
        public Texture2D LoadTexture(string dir) => ContentManager.Load<Texture2D>(ContentFolderTextures + dir);
           

        // PARA CARGAR UN MODELO CON SU EFECTO
        // Si se carga de otra forma el efecto que se quiera, va a romper
        public Model LoadModel(string dir, Effect shader){
            var model = LoadModel(dir);
            foreach(var mesh in model.Meshes)
            foreach(var meshPart in mesh.MeshParts)
                meshPart.Effect= shader;
            return model;
        }
    }
}
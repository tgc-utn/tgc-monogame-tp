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
        internal readonly Model M_Alfil, M_Torre, M_Auto, M_AutoEnemigo, M_Inodoro, M_Misil, M_SillaOficina, M_Cafe, M_Silla, M_Mesa, M_Sillon, M_Televisor1, M_MuebleTV, M_Planta,
                M_Escritorio, M_Cocine, M_Plantis, M_Lego, M_Baniera,M_Sofa, M_Mesita, M_Aparador, M_Bacha;


        internal readonly Effect E_BasicShader, E_TextureShader, E_SpirtalShader;
        internal readonly Texture2D T_Alfombra;
        internal readonly QuadPrimitive G_Quad;
        
        internal Content(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            ContentManager = Content;
            ContentManager.RootDirectory = "Content";
            //PRUEBA
            M_Alfil = LoadModel("Muebles/Alfil/Alfil");
            M_Torre = LoadModel("Muebles/Torre/Torre");
            
            //Geometrias
            G_Quad = new QuadPrimitive(GraphicsDevice);

            //Efectos
            E_BasicShader = LoadEffect("BasicShader");
            E_TextureShader = LoadEffect("TextureShader");
            E_SpirtalShader = LoadEffect("SpiralShader");

            //texturas
            T_Alfombra = LoadTexture("Alfombra");

            //Modelos
            M_Auto = LoadModel("RacingCar/RacingCar");
            M_AutoEnemigo = LoadModel("CombatVehicle/Vehicle");

            M_Inodoro = LoadModel("Muebles/inodoro/inodoro");
            M_Misil = LoadModel("Muebles/Misil/Misil");
            M_SillaOficina = LoadModel("Muebles/Silla-Oficina/Silla-Oficina");
            M_Cafe = LoadModel("Muebles/Cafe-Rojo/Cafe-Rojo");
            M_Silla = LoadModel("Muebles/chair/chair");
            M_Mesa = LoadModel("Muebles/mesa/mesa");
            M_Sillon = LoadModel("Muebles/Sillon/Sillon");
            M_Televisor1 = LoadModel("Muebles/Televisor1/Televisor");
            M_MuebleTV = LoadModel("Muebles/MuebleTV/MuebleTV"); 
            M_Planta = LoadModel("Muebles/Planta/Planta");
            M_Escritorio = LoadModel("Muebles/Escritorio/Escritorio");
            M_Plantis = LoadModel("Muebles/Plantis/Plantis");
            M_Cocine = LoadModel("Muebles/Cocine/Cocine");
            M_Lego = LoadModel("Muebles/Lego/Lego");
            M_Baniera = LoadModel("Muebles/Baniera/Baniera");
            M_Mesita = LoadModel("Muebles/Mesita/Mesita");
            M_Sofa = LoadModel("Muebles/Sofa/Sofa");
            M_Aparador = LoadModel("Muebles/Aparador/Aparador");
            M_Bacha = LoadModel("Muebles/Bacha/Bacha");
            
        }

        public Model LoadModel(string dir) => ContentManager.Load<Model>(ContentFolder3D + dir);
        public Effect LoadEffect(string dir) => ContentManager.Load<Effect>(ContentFolderEffects + dir);
        public Texture2D LoadTexture(string dir) => ContentManager.Load<Texture2D>(ContentFolderTextures + dir);
           
    }
}
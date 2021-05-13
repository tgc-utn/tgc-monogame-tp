using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace TGC.MonoGame.TP
{
    internal class Content
    {
        private readonly ContentManager contentManager;

        private const string EffectsFolder = "Effects/";
        private const string ModelsFolder = "Models/";
        private const string TexturesFolder = "Textures/";
        private const string SoundsFolder = "Sounds/";
        //private const string MusicFolder = "Music/";
        //private const string SpriteFontsFolder = "SpriteFonts/";

        internal readonly Effect E_BasicShader;
        internal readonly Model M_XWing, M_TIE, M_Trench, M_Trench2;
        internal readonly Texture2D[] T_XWing, T_TIE, T_Trench, T_Trench2;
        internal readonly SoundEffect S_Explotion;

        internal Content(ContentManager contentManager)
        {
            this.contentManager = contentManager;

            // Efects
            E_BasicShader = LoadEffect("BasicShader");

            // Models
            M_XWing = LoadModel("XWing/XWing", E_BasicShader);
            M_TIE = LoadModel("TIE/TIE", E_BasicShader);
            M_Trench = LoadModel("Trench/Trench", E_BasicShader);
            M_Trench2 = LoadModel("Trench2/Trench2", E_BasicShader);

            // Textures
            T_TIE = new Texture2D[] { LoadTexture("TIE/TIE_IN_DIFF") };
            T_XWing = new Texture2D[] {
                LoadTexture("XWing/lambert6_Base_Color"),
                LoadTexture("XWing/lambert5_Base_Color")
            };
            T_Trench = new Texture2D[] { LoadTexture("TIE/TIE_IN_Normal") };
            T_Trench2 = Enumerable.Repeat(LoadTexture("TIE/TIE_IN_Normal"), 27).ToArray();

            // Sound
            S_Explotion = LoadSound("Explotion");
        }

        private Effect LoadEffect(string name) => contentManager.Load<Effect>(EffectsFolder + name);
        private Model LoadModel(string modelName, Effect effect)
        {
            Model model = contentManager.Load<Model>(ModelsFolder + modelName);
            foreach (var mesh in model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            return model;
        }
        private Texture2D LoadTexture(string name) => contentManager.Load<Texture2D>(TexturesFolder + name);
        private SoundEffect LoadSound(string name) => contentManager.Load<SoundEffect>(SoundsFolder + name);
    }
}
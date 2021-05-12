using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace TGC.MonoGame.TP.ResourceManagers
{
    internal class TextureManager
    {
        private const string TexturesFolder = "Textures/";

        internal Texture2D[] TIE { get; private set; }
        internal Texture2D[] XWing { get; private set; }
        internal Texture2D[] Trench { get; private set; }
        internal Texture2D[] Trench2 { get; private set; }

        internal void LoadTextures(ContentManager contentManager)
        {
            TIE = new Texture2D[] { LoadTexture("TIE/TIE_IN_DIFF", contentManager) };
            XWing = new Texture2D[] {
                LoadTexture("XWing/lambert6_Base_Color", contentManager),
                LoadTexture("XWing/lambert5_Base_Color", contentManager)
            };
            Trench = new Texture2D[] { LoadTexture("TIE/TIE_IN_Normal", contentManager) };
            Trench2 = Enumerable.Repeat(LoadTexture("TIE/TIE_IN_Normal", contentManager), 27).ToArray();
        }

        private Texture2D LoadTexture(string name, ContentManager contentManager) => contentManager.Load<Texture2D>(TexturesFolder + name);
    }
}
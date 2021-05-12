using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.ResourceManagers
{
    internal class EffectManager
    {
        private const string EffectosFolder = "Effects/";

        internal Effect BasicShader { get; private set; }

        internal void LoadEffects(ContentManager contentManager)
        {
            BasicShader = LoadEffect("BasicShader", contentManager); ;
        }

        private Effect LoadEffect(string name, ContentManager contentManager) => contentManager.Load<Effect>(EffectosFolder + name);
    }
}
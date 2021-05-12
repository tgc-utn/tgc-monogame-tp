using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.ResourceManagers
{
    internal class SoundManager
    {
        private const string Folder = "Sounds/";

        internal SoundEffect Explotion { get; private set; }

        internal void LoadSounds(ContentManager contentManager)
        {
            Explotion = LoadSound("Explotion", contentManager); ;
        }

        private SoundEffect LoadSound(string name, ContentManager contentManager) => contentManager.Load<SoundEffect>(Folder + name);
    }
}
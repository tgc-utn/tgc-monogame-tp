using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.InsaneGames
{
    public class ContentManager
    {
        private const string ContentFolder3D = "Models/";
        private const string ContentFolderEffect = "Effects/";
        private const string ContentFolderMusic = "Music/";
        private const string ContentFolderSounds = "Sounds/";
        private const string ContentFolderSpriteFonts = "Fonts/";
        private const string ContentFolderTextures = "Textures/";

        private Microsoft.Xna.Framework.Content.ContentManager Content;

        static public ContentManager Instance { get; protected set; }
        
        private ContentManager(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Content = content;
        }

        static public void MakeInstance(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            if(ContentManager.Instance is Object) 
              throw new Exception("An instance of ContentManager already exist.");
            ContentManager.Instance = new ContentManager(content);
        }

        public Model LoadModel(string modelName)
        {
            return Content.Load<Model>($"{ContentFolder3D}{modelName}");
        }

        public Texture2D LoadTexture2D(string TextureName)
        {
            return Content.Load<Texture2D>($"{ContentFolderTextures}{TextureName}");
        }
        public SpriteFont LoadSpriteFont(string fontName)
        {
            return Content.Load<SpriteFont>($"{ContentFolderSpriteFonts}{fontName}");
        }
    }
}
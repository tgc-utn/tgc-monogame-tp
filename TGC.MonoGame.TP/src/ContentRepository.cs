using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP {

    class ContentRepository
     {

        private const string ContentFolder3D = "Models/";
        private const string ContentFolderEffects = "Effects/";
        private const string ContentFolderMusic = "Music/";
        private const string ContentFolderSounds = "Sounds/";
        private const string ContentFolderSpriteFonts = "SpriteFonts/";
        private const string ContentFolderTextures = "Textures/";

        private ContentManager _manager;


        private static ContentRepository
         _INSTANCE = null;

        private ContentRepository(){}

        public static void SetUpInstance(ContentManager manager){
            _INSTANCE = new ContentRepository
            {
                _manager = manager
            };
        }

        public static ContentRepository
         GetInstance(){
            return _INSTANCE;
        }

        public Effect GetEffect(string effect){
            return _manager.Load<Effect>(ContentFolderEffects + effect);
        }

        public Model GetModel(string model){
            return _manager.Load<Model>(ContentFolder3D + model);
        }

        public Texture2D GetTexture(string texture){
            return _manager.Load<Texture2D>(ContentFolder3D + texture);
        }

    }

}
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace TGC.MonoGame.TP
{
    static class ModelManager
    {
        private const string ModelsFolder = "Models/";

        public static Model XWing { get; private set; }
        public static Model TIE { get; private set; }
        public static Model Trench { get; private set; }
        public static Model Trench2 { get; private set; }

        public static void LoadModels(ContentManager contentManager, Effect effect)
        {
            XWing = LoadModel("XWing/XWing", contentManager, effect);
            TIE = LoadModel("TIE/TIE", contentManager, effect);
            Trench = LoadModel("Trench/Trench", contentManager, effect);
            Trench2 = LoadModel("Trench2/Trench2", contentManager, effect);
        }

        private static Model LoadModel(String modelName, ContentManager contentManager, Effect effect)
        {
            Model model = contentManager.Load<Model>(ModelsFolder + modelName);
            foreach (var mesh in model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            return model;
        }
    }
}
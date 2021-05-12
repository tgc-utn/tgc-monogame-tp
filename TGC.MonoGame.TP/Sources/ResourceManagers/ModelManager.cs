using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace TGC.MonoGame.TP.ResourceManagers
{
    internal class ModelManager
    {
        private const string ModelsFolder = "Models/";

        internal Model XWing { get; private set; }
        internal Model TIE { get; private set; }
        internal Model Trench { get; private set; }
        internal Model Trench2 { get; private set; }

        internal void LoadModels(ContentManager contentManager, Effect effect)
        {
            XWing = LoadModel("XWing/XWing", contentManager, effect);
            TIE = LoadModel("TIE/TIE", contentManager, effect);
            Trench = LoadModel("Trench/Trench", contentManager, effect);
            Trench2 = LoadModel("Trench2/Trench2", contentManager, effect);
        }

        private Model LoadModel(string modelName, ContentManager contentManager, Effect effect)
        {
            Model model = contentManager.Load<Model>(ModelsFolder + modelName);
            foreach (var mesh in model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
            return model;
        }
    }
}
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.ResourceManagers
{
    internal class ModelManager
    {
        private const string ModelsFolder = "Models/";

        internal Model XWing { get; private set; }
        internal Model TIE { get; private set; }
        internal Model Trench { get; private set; }
        internal Model Trench2 { get; private set; }

        internal void LoadModels(ContentManager contentManager)
        {
            XWing = LoadModel("XWing/XWing", contentManager, TGCGame.effectManager.BasicShader);
            TIE = LoadModel("TIE/TIE", contentManager, TGCGame.effectManager.BasicShader);
            Trench = LoadModel("Trench/Trench", contentManager, TGCGame.effectManager.BasicShader);
            Trench2 = LoadModel("Trench2/Trench2", contentManager, TGCGame.effectManager.BasicShader);
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
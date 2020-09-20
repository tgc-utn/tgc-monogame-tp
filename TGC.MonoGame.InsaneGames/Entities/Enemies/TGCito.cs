using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.InsaneGames.Entities
{
    class TGCito : Enemy
    {
        private const string ModelName = "tgcito/tgcito-classic";
        static private Model Model;
        private Matrix Misalignment { get; }
        public TGCito(Matrix? spawnPoint = null, Matrix? scaling = null)
        {
            Misalignment = Matrix.CreateTranslation(0, 44.5f, 0) * scaling.GetValueOrDefault(Matrix.CreateScale(0.2f));
            if(spawnPoint.HasValue)
                position = spawnPoint.Value;
            floorEnemy = true;
        }
        public override void Load()
        {
            if(Model is null)
                Model = ContentManager.Instance.LoadModel(ModelName);
        }
        public override void Draw(GameTime gameTime)
        {
            if(!position.HasValue)
                throw new System.Exception("The position of the TGCito was not set");
            var world = Misalignment * position.Value; 
            Model.Draw(world, Game.Camera.View, Game.Camera.Projection);
        }

    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.InsaneGames.Collectibles
{
    class Armor : Collectible
    {
        private const string ModelName = "armor/ItalianHelmet";
        static private Model Model;
        static private Matrix Misalignment;
        private Matrix SpawnPoint;
        public Armor(Matrix spawnPoint, Matrix? scaling = null)
        {
            if (Model is null)
            {
                Misalignment = Matrix.CreateTranslation(0, 0, 0);
            }
            SpawnPoint = Misalignment * Matrix.CreateRotationX(MathHelper.ToRadians(270f)) *
                        scaling.GetValueOrDefault(Matrix.CreateScale(2.0f)) *
                        spawnPoint;
        }
        public override void Load()
        {
            if (Model is null)
                Model = ContentManager.Instance.LoadModel(ModelName);
        }
        public override void Draw(GameTime gameTime)
        {
            Model.Draw(SpawnPoint, Game.Camera.View, Game.Camera.Projection);
        }
    }
}

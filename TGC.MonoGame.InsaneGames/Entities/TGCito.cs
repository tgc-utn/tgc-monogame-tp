using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.InsaneGames.Entities
{
    public class TGCito : DrawableGameComponent, IEnemy
    {
        private const string ModelName = "tgcito/tgcito-classic";
        private Model Model;
        private Matrix SpawnPoint;
        private new TGCGame Game;

        public TGCito(TGCGame game) : base(game)
        {
            Game = game;
        }
        public void Initialize(Matrix spawnPoint)
        {
            SpawnPoint = spawnPoint;
        }
        public void Load()
        {
            Model = Game.Content.Load<Model>(TGCGame.ContentFolder3D + ModelName);
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(GameTime gameTime)
        {
            Model.Draw(SpawnPoint, Game.Camera.View, Game.Camera.Projection);
        }
    }
}
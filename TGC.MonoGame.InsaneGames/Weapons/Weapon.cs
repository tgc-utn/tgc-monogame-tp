using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames.Weapons
{
    abstract class Weapon : IDrawable
    { 
        protected Model Model;
        protected string ModelName;
        protected Matrix World;

        static protected Matrix RotationMatrix = Matrix.CreateTranslation(0, -0.5f, 0) *
                                                Matrix.CreateRotationX(MathHelper.ToRadians(15f)) * 
                                                Matrix.CreateRotationY(MathHelper.ToRadians(-5f));

        public Weapon(string modelName)
        {
            ModelName = modelName;
        }

        public override void Load()
        {
            Model = ContentManager.Instance.LoadModel(ModelName);
        }

        public override void Update(GameTime gameTime)
        {
            var camera = Game.Camera;
            var x = Game.GraphicsDevice.DisplayMode.Width * 0.3f;
            var y = Game.GraphicsDevice.DisplayMode.Height * 0.1f;
            World =  RotationMatrix * 
                    Matrix.CreateTranslation(camera.Position.X + 0.4f, camera.Position.Y - 0.5f, camera.Position.Z - 0.1f);
        }

        public override void Draw(GameTime gameTime)
        {
            Model.Draw(World, Game.Camera.View, Game.Camera.Projection);
        }
    }
}
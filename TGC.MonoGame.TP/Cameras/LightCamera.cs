using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class LightCamera : Camera
    {
  
        TGCGame Game;
        public LightCamera(float aspectRatio, Vector3 position) : base(aspectRatio)
        {
            Game = TGCGame.Instance;
            Position = position;

            FrontDirection = Vector3.Normalize(Game.Xwing.Position - Position);
            RightDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, Vector3.Up));
            UpDirection = Vector3.Normalize(Vector3.Cross(RightDirection, FrontDirection));
        }

        private void CalculateView()
        {
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            Position = Game.Xwing.Position - Vector3.Left * 100 + Vector3.Up * 100;
            
            CalculateView();
        }       
    }
}
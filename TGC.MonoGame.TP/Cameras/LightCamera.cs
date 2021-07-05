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

        public Vector3 FrustumCenter;
        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            Position = FrustumCenter - Vector3.Left * 60 + Vector3.Up * 60;
            
            CalculateView();
        }       
    }
}
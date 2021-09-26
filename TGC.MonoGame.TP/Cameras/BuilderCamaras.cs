using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.Samples.Cameras
{
    /// <summary>
    ///     The minimum behavior that a camera should have.
    /// </summary>
    internal class BuilderCamaras : Camera
    {
        public const float DefaultFieldOfViewDegrees = MathHelper.PiOver4;
        public const float DefaultNearPlaneDistance = 0.1f;
        public const float DefaultFarPlaneDistance = 200000000;
        private static readonly Vector3 Position = new Vector3(-350, 50, 400);
        private static float Speed = 5;
        private static readonly Vector3 FromDirection = new Vector3(-350, 1000, 500);
        private static readonly Vector3 UpDirection = new Vector3(-350, 50, 400);
        private List<Camera> Cameras { get; set; }
        private Camera CurrentCamera { get; set; }
        
        
        public BuilderCamaras(float aspectRatio, Point screenCenter,float width, float height) : this(aspectRatio,screenCenter, width,height, DefaultNearPlaneDistance, DefaultFarPlaneDistance)
        {
        }

        public BuilderCamaras(float aspectRatio, Point screenCenter,float width, float height, float nearPlaneDistance, float farPlaneDistance) : this(aspectRatio,screenCenter, width,height,
            nearPlaneDistance, farPlaneDistance, DefaultFieldOfViewDegrees)
        {
        }

        public BuilderCamaras(float aspectRatio, Point screenCenter,float width, float height, float nearPlaneDistance, float farPlaneDistance, float fieldOfViewDegrees) : base(aspectRatio)
        {
            Cameras = new List<Camera>()
            {
                new FreeCamera(aspectRatio, Position, screenCenter),
                new SimpleCamera(aspectRatio,Position,Speed),
                new StaticCamera(aspectRatio, FromDirection, Position,Vector3.Up), //Revisar para que quede para abajo mostrando todo el mapa
                new TargetCamera(aspectRatio, FromDirection, Position, screenCenter, height, width)
            };
            CurrentCamera = Cameras[0];
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.NumPad1))
            {
                CurrentCamera = Cameras[0];
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.NumPad2))
                {
                    CurrentCamera = Cameras[1];
                }
                else
                {
                    if (keyboardState.IsKeyDown(Keys.NumPad3))
                    {
                        CurrentCamera = Cameras[2];
                    }
                    else
                    {
                        if (keyboardState.IsKeyDown(Keys.NumPad4))
                        {
                            CurrentCamera = Cameras[3];
                        }
                    }
                }
            }

            CurrentCamera.Update(gameTime);
            View = CurrentCamera.View;
            Projection = CurrentCamera.Projection;
        }
    }
}
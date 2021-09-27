using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Objects;

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
        //private static readonly Vector3 Position = new Vector3(-350, 50, 400);
        private Vector3 Position { get; set; }
        private Vector3 FrontPosition = new Vector3(0, 40, -200);
        private Vector3 CenterPosition = new Vector3(0, 70, 0);
        private static float Speed = 5;
        private static readonly Vector3 FromDirectionTarget = new Vector3(-350, 1000, 500);
        private static readonly Vector3 FromDirectionStatic = new Vector3(-200f, 10000, 0);
        private List<Camera> Cameras { get; set; }
        private Camera CurrentCamera { get; set; }
        public MainShip MainShip { get; set; }
        
        public BuilderCamaras(float aspectRatio, Point screenCenter,float width, float height, MainShip BarcoPositionCenter) : this(aspectRatio,screenCenter, width,height,BarcoPositionCenter, DefaultNearPlaneDistance, DefaultFarPlaneDistance)
        {
        }

        public BuilderCamaras(float aspectRatio, Point screenCenter,float width, float height,MainShip BarcoPositionCenter, float nearPlaneDistance, float farPlaneDistance) : this(aspectRatio,screenCenter, width,height, BarcoPositionCenter,
            nearPlaneDistance, farPlaneDistance, DefaultFieldOfViewDegrees)
        {
        }

        public BuilderCamaras(float aspectRatio, Point screenCenter,float width, float height, MainShip BarcoPositionCenter, float nearPlaneDistance, float farPlaneDistance, float fieldOfViewDegrees) : base(aspectRatio)
        {
            
            Position = BarcoPositionCenter.Position;
            MainShip = BarcoPositionCenter;
            Cameras = new List<Camera>()
            {
                new FreeCamera(aspectRatio, Position+CenterPosition, screenCenter),
                new SimpleCamera(aspectRatio,Position+FrontPosition,Speed),
                new StaticCamera(aspectRatio, FromDirectionStatic, new Vector3(0,-950,0),new Vector3(1,1,0)), //Revisar para que quede para abajo mostrando todo el mapa
                new TargetCamera(aspectRatio, FromDirectionTarget, Position, screenCenter, height, width)
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
            Cameras[0].Position = MainShip.Position + CenterPosition;
            Cameras[1].Position = MainShip.Position + FrontPosition;
            Cameras[2].FrontDirection = -(FromDirectionStatic - MainShip.Position);
            Cameras[3].TargetPosition = MainShip.Position;
            View = CurrentCamera.View;
            Projection = CurrentCamera.Projection;
        }
    }
}
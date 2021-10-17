using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace TGC.MonoGame.TP.Objects
{
    public class CannonBall
    {
        private Model cannonBall { get; set; }
        private TGCGame Game { get; set; }
        private Vector3 Position0 { get; set; }
        private float Rotate0;
        private int Scale0;
        private Vector3 Position1 { get; set; }
        private Vector3 PositionActual { get; set; }
        private Vector3 PositionAnterior { get; set; }
        private Vector3 Velocidad { get; set; }
        private float cont = 0f;
        private double DurationTotal = 0 ;
        private float Angulo = 0f;
        
        public CannonBall(Vector3 initialPosition, TGCGame game, Model model)
        {
            cannonBall = model;
            Game = game;
            //Position = initialPosition;
            Position0 = new Vector3(-200f, 32, 80);
            Rotate0 = -(float) Math.PI / 2;
            Scale0 = 7;
            Position1 = new Vector3(-200f, 10, 1000);
            PositionActual = Position0;
            PositionAnterior = PositionActual;
            var distancia = Position1 - Position0;
            var duracion = distancia.Length()*0.001;
            Velocidad = (distancia + new Vector3(0, (float) 9.8 + 1000, 0) * (float) duracion * (float) duracion) /
                          (float) duracion;
        }

        public void Draw()
        {
            cannonBall.Draw(Matrix.CreateRotationX((float) Math.PI - Angulo) * Matrix.CreateScale((float)0.1)  * Matrix.CreateTranslation(PositionActual),
                Game.Camera.View, Game.Camera.Projection);
        }

        public void Update(GameTime gameTime)
        {
            DurationTotal += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            if (PositionActual != Position1)
            {
                PositionActual = Position0 + Velocidad * (float) DurationTotal +
                                 (float) DurationTotal * (float) DurationTotal * new Vector3(0, (float) -9.8-1000, 0);
            }
            var y = (PositionActual - new Vector3(PositionActual.X, PositionAnterior.Y, PositionActual.Z)).Length();
            var x = (PositionAnterior - new Vector3(PositionActual.X, PositionAnterior.Y, PositionActual.Z)).Length();
            Angulo =(float) Math.Atan(y / x);
            
            
            PositionAnterior = PositionActual;
        }

    }
}
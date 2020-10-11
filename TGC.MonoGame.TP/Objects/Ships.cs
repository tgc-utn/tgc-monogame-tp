using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Objects 
{
    public class Ship 
    {
        public Vector3 Position { get; set; }
        public float velocidad { get; set; }
        private float maxspeed { get; set; }
        private float maxacceleration { get; set; }
        public Model modelo { get; set; }
        public Vector3 orientacion { get; set; }
        public float anguloDeGiro { get; set; }
        public float giroBase { get; set; }

        public Ship (Vector3 initialPosition, Model baseModel, Vector3 currentOrientation, float MaxSpeed) 
        {
            velocidad = 0;
            Position = initialPosition;
            modelo = baseModel;
            orientacion = currentOrientation;
            maxspeed = MaxSpeed;
            maxacceleration = 0.005f;
            anguloDeGiro = 0f;
            giroBase = 0.005f;
        }


        public void Update(float gameTime, float timeMultiplier) {
            ProcessKeyboard(gameTime);
            Move(gameTime, timeMultiplier);
        }
        public void Move(float gameTime, float timeMultiplier)
        {
            var newOrientacion = new Vector3((float)Math.Sin(anguloDeGiro), 0, (float)Math.Cos(anguloDeGiro));
            orientacion = newOrientacion;
            var newPosition = new Vector3(Position.X + velocidad*orientacion.X,Position.Y,Position.Z + velocidad*orientacion.Z );
            Position = newPosition;
        }

        public Matrix UpdateShipRegardingWaves (float time) {
            
            float waveFrequency = 0.01f;
            float waveAmplitude = 20;
           // float waveAmplitude2 = 50;
           time *= 2;
           
            var worldVector = Position;
            
            var newY = (MathF.Sin(worldVector.X*waveFrequency + time) + MathF.Sin(worldVector.Z*waveFrequency + time))*waveAmplitude;

            var tangent1 = Vector3.Normalize(new Vector3(1, 
                (MathF.Cos(worldVector.X*waveFrequency+time)*waveFrequency*waveAmplitude) * 0.5f
                ,0));
            var tangent2 = Vector3.Normalize(new Vector3(0, 
                (MathF.Cos(worldVector.Z*waveFrequency+time)*waveFrequency*waveAmplitude) * 0.5f
                ,1));
            
            worldVector = new Vector3(worldVector.X, newY, worldVector.Z);
            
            Position = worldVector;
            
            var waterNormal = Vector3.Normalize(Vector3.Cross(tangent2, tangent1));

            return Matrix.CreateLookAt(Vector3.Zero,tangent2, waterNormal);
        }
        
        
        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();


            if (keyboardState.IsKeyDown(Keys.A))
            {
                if(velocidad == 0){}
                else {
                    if(anguloDeGiro+giroBase >= 360){
                        anguloDeGiro = anguloDeGiro + giroBase - 360f;
                    }
                    else {
                        anguloDeGiro += giroBase;
                    } 
                }
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                if(velocidad == 0){}
                else {
                    if(anguloDeGiro+giroBase < 0){
                        anguloDeGiro = anguloDeGiro - giroBase + 360f;
                    }
                    else {
                        anguloDeGiro -= giroBase;
                    } 
                }
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                if(velocidad == maxspeed){}
                else if(velocidad+maxacceleration >= maxspeed){
                    velocidad = maxspeed;
                }
                else {
                    velocidad += maxacceleration;
                }
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                if(velocidad == -maxspeed){}
                else if((velocidad-maxacceleration) <= -maxspeed){
                    velocidad = -maxspeed;
                }
                else {
                    velocidad -= maxacceleration;
                }
            }

            if(keyboardState.IsKeyDown(Keys.Space))
            {
                if(velocidad > 0){
                    if(velocidad - maxacceleration*6 <= 0){
                        velocidad = 0;
                    }
                    else {
                        velocidad -= maxacceleration*6;
                    }
                }
                else if (velocidad < 0){
                    if(velocidad + maxacceleration*6 >= 0){
                        velocidad = 0;
                    }
                    else {
                        velocidad += maxacceleration*6;
                    }
                }
                else {
                    velocidad = 0;
                }
            }
        }
    }
}
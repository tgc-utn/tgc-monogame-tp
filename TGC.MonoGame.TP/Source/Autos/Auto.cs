using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class Auto : IElementoDinamico
    { 
        const float WHEEL_TURNING_AMOUNT = 0.5f;
        // [BUG]
        // Creo que hay que hacerlo depender de S_Metro porque si S_METRO cambia de valor, este valor tiene que cambiar para quedar "normal"
        const float ERROR_TRASLACION_RUEDAS = 0.75f;
        private const float AUTO_SCALE = 0.15f * TGCGame.S_METRO;
        private Vector3 Position;
        private Vector3 Velocity;
        private float AccelerationMagnitude = 2000f;
        private float AceleracionRelativa = 0f;
        private float Rotation;
        private float JumpPower = 50000f;
        private float Turning = 0f;
        private float WheelTurning = 0f;

        public Auto(Vector3 posicionInicial, float escala = AUTO_SCALE) 
        : base(TGCGame.GameContent.M_Auto, Vector3.Zero, Vector3.Zero, escala)
        {
            Position = posicionInicial;
            Escala = escala;
            this.SetEffect(TGCGame.GameContent.E_SpiralShader);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float accelerationSense = 0f;
            Vector3 acceleration = Vector3.Zero;

            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            // GRAVEDAD
            float floor = 0f;
            Vector3 Gravity = -Vector3.Up * 15f;
            Matrix MatrixRotation = Matrix.CreateRotationY(Rotation);
  
            // GIRO
            var pressedKeys = keyboardState.GetPressedKeys();
            foreach( var key in pressedKeys){
                switch(key){
                    case Keys.A:
                        Turning += 1f;
                        WheelTurning = WHEEL_TURNING_AMOUNT; // Gira a la izquierda
                    break;
                    case Keys.D:
                        Turning -= 1f;
                        WheelTurning = (-1)*WHEEL_TURNING_AMOUNT; // Gira a la derecha
                    break;
                }
            }
            if(pressedKeys.Length == 0) WheelTurning = 0f;

            Rotation = Turning * dTime;

            if(Position.Y<floor){
                Position = new Vector3(Position.X, floor, Position.Z);
                Velocity = new Vector3(Velocity.X, 0, Velocity.Z);
            
                // ACELERACION
                if (keyboardState.IsKeyDown(Keys.W))
                    accelerationSense = 1f;
                else if (keyboardState.IsKeyDown(Keys.S))
                    accelerationSense = -0.5f; //Reversa mas lenta

                Vector3 accelerationDirection = -MatrixRotation.Forward;
                acceleration = accelerationDirection * accelerationSense * AccelerationMagnitude;

                // ROZAMIENTO
                float u = -1.35f; //Coeficiente de Rozamiento
                if (keyboardState.IsKeyDown(Keys.LeftShift)) // LShift para Frenar
                    u*=2;
                Vector3 Friction = new Vector3(Velocity.X, 0, Velocity.Z) * u * dTime;
                Velocity += Friction;
            }
            else {
                Velocity += Gravity;
            }
            
            // SALTO
            if (keyboardState.IsKeyDown(Keys.Space) && Position.Y==floor)
                Velocity += Vector3.Up * JumpPower * dTime;

            Velocity += acceleration * dTime;
            Position += Velocity * dTime;

            // MATRIZ DE MUNDO
            World = 
                Matrix.CreateScale(Escala) * 
                MatrixRotation *
                Matrix.CreateTranslation(Position);
        }   
        public override void Draw(){
            // acá se están dibujando las ruedas una vez. sacarlas del dibujado.
            foreach(var bone in Model.Bones){
                switch(bone.Name){
                    case "Car":
                        foreach(var mesh in bone.Meshes){  
                            foreach(var meshPart in mesh.MeshParts){
                                meshPart.Effect.Parameters["World"]?.SetValue(World);
                            }
                            mesh.Draw();
                        }
                    break;
                    case "WheelA":
                    case "WheelB":
                        foreach(var mesh in bone.Meshes){
                            foreach(var meshPart in mesh.MeshParts){
                                // Escalo -> Rotación extra -> Llevo a su lugar -> Rotación auto -> Traslación auto
                                World = 
                                        Matrix.CreateRotationY(WheelTurning) *
                                        Matrix.CreateScale(Escala) * 
                                        Matrix.CreateTranslation(bone.Transform.Translation*ERROR_TRASLACION_RUEDAS) *
                                        Matrix.CreateRotationY(Rotation) *
                                        Matrix.CreateTranslation(Position);
                                meshPart.Effect.Parameters["World"]?.SetValue(World);
                            }
                            mesh.Draw();
                        }
                    break;
                    case "WheelC":
                    case "WheelD":
                        foreach(var mesh in bone.Meshes){
                            foreach(var meshPart in mesh.MeshParts){
                                // Escalo -> Rotación extra -> Llevo a su lugar -> Rotación auto -> Traslación auto
                                World = 
                                        Matrix.CreateScale(Escala) * 
                                        Matrix.CreateTranslation(bone.Transform.Translation*ERROR_TRASLACION_RUEDAS) *
                                        Matrix.CreateRotationY(Rotation) *
                                        Matrix.CreateTranslation(Position);
                                meshPart.Effect.Parameters["World"]?.SetValue(World);
                            }
                            mesh.Draw();
                        }
                    break;
                    default: 
                    break;
                }






















                // if(bone.Name == "WheelB" || bone.Name == "WheelA" || bone.Name == "WheelC" || bone.Name == "WheelD"){
                //     if(bone.Name == "WheelB" || bone.Name == "WheelA"){                        
                //         Matrix MatrixRotation = Matrix.CreateRotationY(Rotation+WheelTurning);
                //         World = 
                //             Matrix.CreateScale(Escala) * 
                //             MatrixRotation *
                //             Matrix.CreateTranslation(Position);
                //     }
                //     foreach(var mesh in bone.Meshes){
                //         World *= Matrix.CreateTranslation(bone.Transform.Translation);
                //         foreach( var meshPart in mesh.MeshParts){
                //             meshPart.Effect = TGCGame.GameContent.E_BasicShader;
                //             if(bone.Name == "WheelB" || bone.Name == "WheelA"){
                //                 meshPart.Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                //             }   
                //             if(bone.Name == "WheelC" || bone.Name == "WheelD"){
                //                 meshPart.Effect.Parameters["DiffuseColor"].SetValue(Color.Green.ToVector3());
                //             }   
                //             meshPart.Effect.Parameters["World"]?.SetValue(World);
                //         }
                //         mesh.Draw(); 
                //     }
                // }
            }
        }
    }

}
    
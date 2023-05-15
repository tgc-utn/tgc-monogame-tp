using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class Auto : IElementoDinamico
    { 
        private const float WHEEL_TURNING_LIMIT = 0.5f;
        private const float MAX_SPEED = 2100f; // . . . Aproximada (la medí)
        private const float ACCELERATION_MAGNITUDE = 4000f;
        private const float JUMP_POWER = 50000f;
        private const float AUTO_SCALE = 0.08f * TGCGame.S_METRO;
        private const float ERROR_TRASLACION_RUEDAS = AUTO_SCALE*0.01f;
        private Vector3 Position;
        private Vector3 Velocity;
        private float VelocidadTablero = 0f; // . . . . Velocidad absoluta
        private float CoeficienteVelocidad = 0f; // . . Velocidad Tabero / Velocidad Max
        private float Rotation;
        private float Turning = 0f;
        private float WheelTurning = 0f;
        private float WheelRotation = 0f;
        private float WheelRotationF = 0f;
        private float TurboRestante = 1000f;

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

            //float  velocidadDeRotacion = (float)Math.Atan(Velocity.Length()/100) / MathHelper.PiOver2;
            //Console.WriteLine("Vel. tablero : . . . . . . . . . . . {0:F}", velocidadDeRotacion);
            //Console.WriteLine("Velocity.length() : . . . . . . . . . . . {0:F}", Velocity.Length());


            
            // GRAVEDAD
            float floor = 0f;
            Vector3 gravity = -Vector3.Up * 15f;
            Matrix matrixRotation = Matrix.CreateRotationY(Rotation);
  
            // DETECCIÓN DE GIRO Y DIRECCIÓN
            var pressedKeys = keyboardState.GetPressedKeys();
            foreach( var key in pressedKeys){
                switch(key){
                    case Keys.A:
                        
                        WheelTurning = (WheelTurning<WHEEL_TURNING_LIMIT)? // Qué no gire de más
                                          WheelTurning+WHEEL_TURNING_LIMIT*dTime*4f
                                        : WHEEL_TURNING_LIMIT; 
                    break;
                    case Keys.D:
                        WheelTurning = (WheelTurning>(-1)*WHEEL_TURNING_LIMIT)? // Qué no gire de más
                                          WheelTurning-WHEEL_TURNING_LIMIT*dTime*4f
                                        : (-1)*WHEEL_TURNING_LIMIT; 
                        //Turning -= 1f * Math.Min(CoeficienteVelocidad*3, 1) * (WheelTurning * -2);
                    break;
                    case Keys.W:
                        accelerationSense = 1f;
                        
                    break;
                    case Keys.S:
                        accelerationSense = -0.5f; //Reversa mas lenta
                    break;
                }

            if(TurboRestante > 0 && keyboardState.IsKeyDown(Keys.F)){ //Turbo
                TurboRestante--;
                accelerationSense*=2;
            }

            }
            Turning += 1f * Math.Min(CoeficienteVelocidad*3, 1) * (WheelTurning * 3); //El giro depende del giro de la rueda

            Rotation = Turning * dTime;

            if(Position.Y<floor){
                Position.Y = floor;
                Velocity.Y = 0;

                // ACELERACIÓN

                Vector3 accelerationDirection = - matrixRotation.Forward;
                acceleration = accelerationDirection * accelerationSense * ACCELERATION_MAGNITUDE;

                // ROZAMIENTO
                float u = -2.35f; //Coeficiente de Rozamiento
                if (keyboardState.IsKeyDown(Keys.LeftShift)) // LShift para Frenar
                    u*=2;
                Vector3 friction = 
                    (new Vector3(Velocity.X * (1 - Math.Abs(World.Forward.X / 40)), 0, Velocity.Z * (1 - Math.Abs(World.Forward.Z / 40))) +
                     new Vector3(Velocity.X, 0, Velocity.Z))* 
                    u * 
                    dTime;
                Velocity += friction;
            }
            else {
                Velocity += gravity;
            }
            
            // SALTO
            if (keyboardState.IsKeyDown(Keys.Space) && Position.Y==floor)
                Velocity += Vector3.Up * JUMP_POWER * dTime;

            Velocity += acceleration * dTime;
            Position += Velocity * dTime;
            
            // VELOCIDAD TABLERO
            var velocidadAux = Velocity;
            velocidadAux.X = (velocidadAux.X<0)? velocidadAux.X*(-1) : velocidadAux.X; 
            velocidadAux.Z = (velocidadAux.Z<0)? velocidadAux.Z*(-1) : velocidadAux.Z;
            VelocidadTablero =  velocidadAux.X + velocidadAux.Z;
            CoeficienteVelocidad = VelocidadTablero / MAX_SPEED;

            
            // ROTACIÓN RUEDAS (distinto en reversa y para adelante)
            WheelRotation = (accelerationSense>=0)? WheelRotation+(CoeficienteVelocidad) * MathHelper.PiOver4 
                                                 : WheelRotation-(CoeficienteVelocidad) * MathHelper.PiOver4;
            // WheelRotationF = (accelerationSense>=0)? WheelRotationF+(CoeficienteVelocidad) * MathHelper.PiOver4 
            //                                      : WheelRotationF-(CoeficienteVelocidad) * MathHelper.PiOver4;                                     
            // WheelRotationF = (WheelRotationF * 0.8f)%MathHelper.TwoPi;                                    
            WheelRotation = WheelRotation%MathHelper.TwoPi; // para que no acumule al infinito
            WheelTurning = (WheelTurning>0)?  WheelTurning - WHEEL_TURNING_LIMIT*dTime*2*CoeficienteVelocidad 
                                            : WheelTurning + WHEEL_TURNING_LIMIT*dTime*2*CoeficienteVelocidad;


            // MATRIZ DE MUNDO AUTO
            World = 
                Matrix.CreateScale(Escala) * 
                matrixRotation *
                Matrix.CreateTranslation(Position);

            // DEBUG TABLERO AUTO 
            // Console.WriteLine("> > > > > > > > > > > > > > > > > > > > > > > > > > > > > > > ", acceleration.X, acceleration.Y, acceleration.Z);
            Console.WriteLine("Velocity : . . . . . . . . . . . . . x: {0:F}, y: {1:F}", World.Forward.X, World.Forward.Z);
            // Console.WriteLine("Vel. tablero : . . . . . . . . . . . {0:F}", VelocidadTablero);
             Console.WriteLine("Velocidad alcanzada :    . . . . . . {0:F}%", (CoeficienteVelocidad * 100f));
            // Console.WriteLine("Giro rueda (radianes) :  . . . . . . {0:F}", (CoeficienteVelocidad) * MathHelper.TwoPi);
            Console.WriteLine("Ruedas :    . . . . . . {0:F}", (WheelTurning)); 
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
                    case "WheelA": // Adelante derecha
                    case "WheelB": // Adelante izquierda
                        foreach(var mesh in bone.Meshes){
                            foreach(var meshPart in mesh.MeshParts){
                                // Escalo -> Rotación extra -> Llevo a su lugar -> Rotación auto -> Traslación auto
                                World = 
                                        // Matrix.CreateRotationX(VelocidadTablero*WHEEL_SPEED_AMOUNT) *
                                        Matrix.CreateRotationX(WheelRotation) *
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
                    case "WheelC": // Atras izquierda
                    case "WheelD": // Atras derecha
                        foreach(var mesh in bone.Meshes){
                            foreach(var meshPart in mesh.MeshParts){
                                // Escalo -> Llevo a su lugar -> Rotación auto -> Traslación auto
                                World = 
                                        Matrix.CreateRotationX(WheelRotation) *
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
            }
        }
    }

}
    
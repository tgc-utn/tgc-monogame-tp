using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Design;
using TGC.MonoGame.TP.Collisions;
using BepuVector3    = System.Numerics.Vector3;
using BepuQuaternion = System.Numerics.Quaternion;
using TGC.MonoGame.Samples.Viewer.Gizmos;
using BepuPhysics;
using BepuPhysics.Collidables;

namespace TGC.MonoGame.TP
{
    public class Auto : IElementoDinamico
    { 
        private const float WHEEL_TURNING_LIMIT = 0.5f;
        private const float MAX_SPEED = 2100f; // . . . Aproximada (la medí)
        private const float ACCELERATION_MAGNITUDE = 4f * TGCGame.S_METRO;
        private const float JUMP_POWER = 50000f;
        private const float AUTO_SCALE = 0.08f * TGCGame.S_METRO;
        private const float ERROR_TRASLACION_RUEDAS = AUTO_SCALE*0.01f;
        private BodyHandle Handle;
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
            Position = posicionInicial + new Vector3(0,0.05f,0);
            Escala = escala;
            this.SetEffect(TGCGame.GameContent.E_SpiralShader);
                
            var boxSize = (Utils.ModelSize(Model))*0.015f*AUTO_SCALE;
            var boxShape = new Box(boxSize.X,boxSize.Y,boxSize.Z); // a chequear
            Console.WriteLine("A ver si seba tiene razon");
            Console.WriteLine("Tamaño de la caja :: {0:F},{1:F},{2:F}",boxSize.X,boxSize.Y,boxSize.Z);
            var boxInertia = boxShape.ComputeInertia(1);
            var boxIndex = TGCGame.Simulation.Shapes.Add(boxShape);

            Handle = TGCGame.Simulation.Bodies.Add(BodyDescription.CreateDynamic(
                            new BepuVector3(Position.X,Position.Y,Position.Z),
                            boxInertia,
                            new CollidableDescription(boxIndex, 0.1f),
                            new BodyActivityDescription(0.01f)));
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {   
            var simuWorld = TGCGame.Simulation.Bodies.GetBodyReference(Handle);
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
        
            // DETECCIÓN DE GIRO Y DIRECCIÓN
            float accelerationSense = 0f;
            Vector3 acceleration = Vector3.Zero;

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

            var right = keyboardState.IsKeyDown(Keys.D) ? 1 : 0;
            var left = keyboardState.IsKeyDown(Keys.A) ? 1 : 0;
            var axis = left - right;
            Turning += WheelTurning * 0.3f; //El giro depende del giro de la rueda

            //ROTACION
            Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, axis * dTime * 0.5f);

            simuWorld.Pose.Orientation = simuWorld.Pose.Orientation * rotation.ToBepu();

            Vector3 accelerationDirection = Utils.FowardFromQuaternion(simuWorld.Pose.Orientation);
            acceleration = accelerationDirection * accelerationSense * ACCELERATION_MAGNITUDE;
            
            // SALTO
            if (keyboardState.IsKeyDown(Keys.Space))// TODO; Checkear que toque el piso
                Velocity += Vector3.Up * JUMP_POWER * dTime;
            Velocity += acceleration * dTime;

            // ROTACIÓN RUEDAS (distinto en reversa y para adelante)
            WheelRotation = (accelerationSense>=0)? WheelRotation+(CoeficienteVelocidad) * MathHelper.PiOver4 
                                                 : WheelRotation-(CoeficienteVelocidad) * MathHelper.PiOver4;
            WheelTurning = (WheelTurning>0)?  WheelTurning - WHEEL_TURNING_LIMIT*dTime*2*CoeficienteVelocidad 
                                            : WheelTurning + WHEEL_TURNING_LIMIT*dTime*2*CoeficienteVelocidad;

            //ACELERACION
            simuWorld.Velocity.Linear += new BepuVector3(Velocity.X,Velocity.Y,Velocity.Z);
            
            if(simuWorld.Velocity.Linear.Length()>MAX_SPEED)
                simuWorld.Velocity.Linear = (Vector3.Normalize(simuWorld.Velocity.Linear.ToVector3()) * MAX_SPEED).ToBepu(); 


            //WORLD MATRIX
            Position = simuWorld.Pose.Position;
            var quaternion = simuWorld.Pose.Orientation;
            World =
                Matrix.CreateScale(Escala) *
                Matrix.CreateFromQuaternion(new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W)) * 
                Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z));


            // DEBUG TABLERO AUTO 
            // Console.WriteLine("> > > > > > > > > > > > > > > > > > > > > > > > > > > > > > > ", acceleration.X, acceleration.Y, acceleration.Z);
            Console.WriteLine("Velocity : . . . . . . . . . . . . . x: {0:F}, z: {1:F}", Velocity.X, Velocity.Z);
            Console.WriteLine("BepuVelocity :   . . . . . . . . . . . x: {0:F}, y:{1:F}", simuWorld.Velocity.Linear.X,simuWorld.Velocity.Linear.Z);
            Console.WriteLine("Vel. tablero : . . . . . . . . . . . {0:F}", VelocidadTablero);
            Console.WriteLine("Velocidad alcanzada :    . . . . . . {0:F}%", (CoeficienteVelocidad * 100f));
            // Console.WriteLine("Giro rueda (radianes) :  . . . . . . {0:F}", (CoeficienteVelocidad) * MathHelper.TwoPi);
            // Console.WriteLine("Ruedas :    . . . . . . {0:F}", (WheelTurning)); 
        }   

        public override void Draw(){
            var body = TGCGame.Simulation.Bodies.GetBodyReference(Handle);
            var aabb = body.BoundingBox;
            
            TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Black);
            
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
    
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
            var boxSize = Utils.ModelSize(Model);
            var angularImpulse = Vector3.Zero;
            var linearImpulse = Vector3.Zero;


            // DETECCIÓN DE GIRO Y DIRECCIÓN
            var pressedKeys = keyboardState.GetPressedKeys();
            foreach( var key in pressedKeys){
                switch(key){
                    case Keys.A:
                        angularImpulse = new Vector3(0,1.2f,0);
                    break;
                    case Keys.D:
                        angularImpulse = new Vector3(0,-1.2f,0);
                    break;
                    case Keys.W:
                        linearImpulse = Utils.FowardFromQuaternion(simuWorld.Pose.Orientation)*ACCELERATION_MAGNITUDE;
                    break;
                    case Keys.S:
                        linearImpulse = -Utils.FowardFromQuaternion(simuWorld.Pose.Orientation)*ACCELERATION_MAGNITUDE;
                    break;
                }
            }

            linearImpulse += Vector3.UnitY * -15f; 

            simuWorld.ApplyLinearImpulse(linearImpulse.ToBepu());
            simuWorld.ApplyAngularImpulse(angularImpulse.ToBepu());

            Position = simuWorld.Pose.Position;
            var quaternion = simuWorld.Pose.Orientation;
            World =
                Matrix.CreateScale(Escala) *
                Matrix.CreateFromQuaternion(new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W)) * 
                Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z));


            // DEBUG TABLERO AUTO 
            // Console.WriteLine("> > > > > > > > > > > > > > > > > > > > > > > > > > > > > > > ", acceleration.X, acceleration.Y, acceleration.Z);
            Console.WriteLine("Velocity : . . . . . . . . . . . . . x: {0:F}, y: {1:F}", World.Forward.X, World.Forward.Z);
            // Console.WriteLine("Vel. tablero : . . . . . . . . . . . {0:F}", VelocidadTablero);
             Console.WriteLine("Velocidad alcanzada :    . . . . . . {0:F}%", (CoeficienteVelocidad * 100f));
            // Console.WriteLine("Giro rueda (radianes) :  . . . . . . {0:F}", (CoeficienteVelocidad) * MathHelper.TwoPi);
            Console.WriteLine("Ruedas :    . . . . . . {0:F}", (WheelTurning)); 
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
    
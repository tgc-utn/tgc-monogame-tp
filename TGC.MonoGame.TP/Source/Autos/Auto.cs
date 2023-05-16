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
        private const float ANGULAR_SPEED = 600f;
        private const float LINEAR_SPEED = 80f;
        private const float AUTO_SCALE = 0.08f * TGCGame.S_METRO;
        private const float SIMU_BOX_SCALE = 0.010f*AUTO_SCALE;
        private const float WHEEL_ROTATION_FACTOR = 0.000005f; // Factor de ajuste para la rotación
        private const float JUMP_POWER = 10f; // Factor de ajuste para la rotación
        private bool isJumping = false;
        private BodyHandle Handle;
        private Vector3 Position;
        private float WheelRotation; // Tal vez pueda usarse también como rotación del auto
        private float WheelTurning = 0f;

        public Auto(Vector3 posicionInicial, float escala = AUTO_SCALE) 
        : base(TGCGame.GameContent.M_Auto, Vector3.Zero, Vector3.Zero, escala)
        {
            Position = posicionInicial + new Vector3(0,TGCGame.S_METRO,0);
            Escala = escala;
            this.SetEffect(TGCGame.GameContent.E_SpiralShader);
                
            var boxSize = (Utils.ModelSize(Model))*SIMU_BOX_SCALE;
            var boxShape = new Box(boxSize.X,boxSize.Y,boxSize.Z); // a chequear
            var boxInertia = boxShape.ComputeInertia(3);
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
            var linearImpulse = Vector3.Zero;
            var angularImpulse = Vector3.Zero;

            var pressedKeys = keyboardState.GetPressedKeys();
            if(pressedKeys.Length>0) simuWorld.Awake = true;

            foreach(var key in pressedKeys){
                switch(key){
                    case Keys.A:
                        angularImpulse = new Vector3(0,ANGULAR_SPEED,0);
                        WheelTurning = (WheelTurning<WHEEL_TURNING_LIMIT)? // Qué no gire de más
                                                                    WheelTurning+WHEEL_TURNING_LIMIT*dTime*4f
                                                                  : WHEEL_TURNING_LIMIT; 

                    break;
                    case Keys.D:
                        angularImpulse = new Vector3(0,-ANGULAR_SPEED,0);
                        WheelTurning = (WheelTurning>(-1)*WHEEL_TURNING_LIMIT)? // Qué no gire de más
                                                                   WheelTurning-WHEEL_TURNING_LIMIT*dTime*4f
                                                                 : (-1)*WHEEL_TURNING_LIMIT;
                    break;
                    case Keys.S:
                        linearImpulse = Utils.FowardFromQuaternion(simuWorld.Pose.Orientation)*(-LINEAR_SPEED);
                    break;
                    case Keys.W:
                        linearImpulse = Utils.FowardFromQuaternion(simuWorld.Pose.Orientation)*(LINEAR_SPEED);
                    break;
                    case Keys.Space:
                        // if(!isJumping){
                        //     linearImpulse += new Vector3(0,JUMP_POWER,0);
                        //     isJumping = false;
                        // }
                    break;
                }
            }

            // if(simuWorld.Pose.Position.Y > 0){
            //     simuWorld.ApplyLinearImpulse((Vector3.UnitY*-10f).ToBepu());
            // }


            simuWorld.ApplyAngularImpulse(angularImpulse.ToBepu());
            // simuWorld.ApplyLinearImpulse(linearImpulse.ToBepu());
            simuWorld.ApplyImpulse(linearImpulse.ToBepu(), Utils.FowardFromQuaternion((simuWorld.Pose.Orientation.ToQuaternion())*2).ToBepu());

            WheelRotation += simuWorld.Velocity.Angular.Y * WHEEL_ROTATION_FACTOR;

            //WORLD MATRIX
            Position = simuWorld.Pose.Position;
            var quaternion = simuWorld.Pose.Orientation;
            World =
                Matrix.CreateScale(Escala) *
                Matrix.CreateFromQuaternion(new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W)) * 
                Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z));
        }   

        public override void Draw(){
            var simuWorld = TGCGame.Simulation.Bodies.GetBodyReference(Handle);
            
            var aabb = simuWorld.BoundingBox;
            
            // TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Black);
            
            var quaternion = simuWorld.Pose.Orientation;
            var worldAux = Matrix.Identity;

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
                        worldAux = 
                                    Matrix.CreateRotationY(WheelTurning)                        // giro del volante
                                    * Matrix.CreateTranslation(bone.ModelTransform.Translation) // error inicial de traslación de ruedas
                                    * Matrix.CreateRotationY(WheelRotation)                     // giro con el auto
                                    * World
                                    ;
                        foreach(var mesh in bone.Meshes){
                            foreach(var meshPart in mesh.MeshParts){
                                // Escalo -> Rotación extra -> Llevo a su lugar -> Rotación auto -> Traslación auto
                                meshPart.Effect.Parameters["World"]?.SetValue(worldAux);
                            }
                            mesh.Draw();
                        }
                    break;
                    case "WheelC": // Atras izquierda
                    case "WheelD": // Atras derecha
                        worldAux = 
                                    Matrix.CreateTranslation(bone.ModelTransform.Translation) // error inicial de traslación de ruedas
                                    * Matrix.CreateRotationY(WheelRotation)                   // giro con el auto
                                    * World 
                                    ;
                        foreach(var mesh in bone.Meshes){
                            foreach(var meshPart in mesh.MeshParts){
                                meshPart.Effect.Parameters["World"]?.SetValue(worldAux);
                            }
                            mesh.Draw();
                        }
                    break;
                }
            }
        }
    }

}
    
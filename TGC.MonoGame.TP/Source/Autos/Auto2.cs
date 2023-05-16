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
    public class Auto2 : IElementoDinamico
    { 
        private const float ANGULAR_SPEED = 90f;
        private const float LINEAR_SPEED = 30f;
        private const float AUTO_SCALE = 0.08f * TGCGame.S_METRO;
        private const float ERROR_TRASLACION_RUEDAS = AUTO_SCALE*0.01f;
        private BodyHandle Handle;
        private Vector3 Position;
        private float Rotation;
        private float WheelTurning = 0f;
        private float WheelRotation = 0f;
        
        public Auto2(Vector3 posicionInicial, float escala = AUTO_SCALE) 
        : base(TGCGame.GameContent.M_Auto, Vector3.Zero, Vector3.Zero, escala)
        {
            Position = posicionInicial + new Vector3(0,0.05f,0);
            Escala = escala;
            this.SetEffect(TGCGame.GameContent.E_SpiralShader);
                
            var boxSize = (Utils.ModelSize(Model))*0.015f*AUTO_SCALE;
            var boxShape = new Box(boxSize.X,boxSize.Y,boxSize.Z); // a chequear
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
            var linearImpulse = Vector3.Zero;
            var angularImpulse = Vector3.Zero;

            var pressedKeys = keyboardState.GetPressedKeys();
            if(pressedKeys.Length>0) simuWorld.Awake = true;

            foreach(var key in pressedKeys){
                switch(key){
                    case Keys.Left:
                        angularImpulse = new Vector3(0,ANGULAR_SPEED,0);
                    break;
                    case Keys.Right:
                        angularImpulse = new Vector3(0,-ANGULAR_SPEED,0);
                    break;
                    case Keys.Down:
                        linearImpulse = Utils.FowardFromQuaternion(simuWorld.Pose.Orientation)*(-LINEAR_SPEED);
                    break;
                    case Keys.Up:
                        linearImpulse = Utils.FowardFromQuaternion(simuWorld.Pose.Orientation)*(LINEAR_SPEED);
                    break;
                    case Keys.Space:
                    break;
                }
            }

            simuWorld.ApplyAngularImpulse(angularImpulse.ToBepu());
            simuWorld.ApplyLinearImpulse(linearImpulse.ToBepu());

            //WORLD MATRIX
            Position = simuWorld.Pose.Position;
            var quaternion = simuWorld.Pose.Orientation;
            World =
                Matrix.CreateScale(Escala) *
                Matrix.CreateFromQuaternion(new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W)) * 
                Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z));
        }   

        public override void Draw(){
            var body = TGCGame.Simulation.Bodies.GetBodyReference(Handle);
            
            var aabb = body.BoundingBox;
            
            TGCGame.Gizmos.DrawCube((aabb.Max + aabb.Min) / 2f, aabb.Max - aabb.Min, Color.Red);
            
            // acá se están dibujando las ruedas una vez. sacarlas del dibujado.
            foreach(var bone in Model.Bones){
                switch(bone.Name){
                    default:
                        foreach(var mesh in bone.Meshes){  
                            foreach(var meshPart in mesh.MeshParts){
                                meshPart.Effect.Parameters["World"]?.SetValue(World);
                            }
                            mesh.Draw();
                        }
                    break;
                }
            }
        }
    }

}
    
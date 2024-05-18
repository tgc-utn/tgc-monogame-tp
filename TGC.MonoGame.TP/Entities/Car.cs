using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Collisions;
using NumericVector3 = System.Numerics.Vector3;

namespace TGC.MonoGame.TP;

public class Car 
{
    private Vector3 Position;
    private Matrix World { get; set; }
    private Model Model;
    private ModelMesh MainBody;
    private ModelMesh FrontLeftWheel;
    private ModelMesh FrontRightWheel;
    private ModelMesh BackLeftWheel;
    private ModelMesh BackRightWheel;
    private BodyHandle Handle;
    private float Scale = 1f;
    private Effect Effect;
    private float CarVelocity { get; set; }
    private float CarRotation { get; set; }
    private float acceleration = 3f;
    private float frictionCoefficient = 0.5f;
    private float maxVelocity = 0.7f;
    private float minVelocity = -0.7f;
    private float stopCar = 0f;
    private float carRotatingVelocity = 2.3f;
    private float jumpSpeed = 100f;
    private float gravity = 10f;
    private float carMass = 3.8f;
    private float carInFloor = 0f;
    private float wheelRotation = 0f;
    private List<List<Texture2D>> MeshPartTextures = new List<List<Texture2D>>();


    public Car(Vector3 pos)
    {
        Position = pos;
    }

    public Matrix getWorld() { return World; }

    public void LoadPhysics(Simulation Simulation) {
        var box = new Box(5f, 2.5f, 5f);
        var boxIndex = Simulation.Shapes.Add(box);
        var bh = Simulation.Bodies.Add(BodyDescription.CreateDynamic(
            new NumericVector3(0, 0, 0),
            box.ComputeInertia(carMass),
            new CollidableDescription(boxIndex, 0.1f),
            new BodyActivityDescription(0.01f)));
        Handle = bh;
    }

    public void Load(Model model, Effect effect) {

        Effect = effect;
        Model = model;
        World = Matrix.Identity;

        MainBody = Model.Meshes[0];
        FrontRightWheel = Model.Meshes[1];
        FrontLeftWheel = Model.Meshes[2];
        BackLeftWheel = Model.Meshes[3];
        BackRightWheel = Model.Meshes[4];

        for (int mi = 0; mi < Model.Meshes.Count; mi++)
        {
            var mesh = model.Meshes[mi];
            MeshPartTextures.Add(new List<Texture2D>());
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            for (int mpi = 0; mpi < mesh.MeshParts.Count; mpi++)
            {
                var meshPart = mesh.MeshParts[mpi];
                var texture = ((BasicEffect) meshPart.Effect).Texture;
                MeshPartTextures[mi].Add(texture);
                meshPart.Effect = Effect;
            }
        }

    }

    private void MoveCar(KeyboardState keyboardState, GameTime gameTime, BodyReference bodyReference)
    {
         if (keyboardState.IsKeyDown(Keys.W))
        {
           bodyReference.ApplyLinearImpulse(new System.Numerics.Vector3(0, 0, -100f) * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            bodyReference.ApplyLinearImpulse(new System.Numerics.Vector3(0, 0, 100f) * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            bodyReference.ApplyAngularImpulse(new System.Numerics.Vector3(0, -100f, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            bodyReference.ApplyAngularImpulse(new System.Numerics.Vector3(0, 100f, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }

    public void Update(KeyboardState keyboardState, GameTime gameTime, Simulation simulation) {
    
        var bodyHandle = Handle;
        var bodyReference = simulation.Bodies.GetBodyReference(bodyHandle);
        MoveCar(keyboardState, gameTime, bodyReference);
        var position = bodyReference.Pose.Position;
        var quaternion = bodyReference.Pose.Orientation;
        var world =
            Matrix.CreateFromQuaternion(new Quaternion(quaternion.X, quaternion.Y, quaternion.Z,
                quaternion.W)) * Matrix.CreateTranslation(new Vector3(position.X, position.Y, position.Z));
        World = world;

    }



    public void Draw() {
        DrawCarBody();
        DrawFrontWheels();
        DrawBackWheels();
    }

    private void DrawCarBody() {
        // World = MainBody.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * Matrix.CreateRotationY(CarRotation) * Matrix.CreateTranslation(Position);
        World = MainBody.ParentBone.ModelTransform * World;
        for (int mpi = 0; mpi < MainBody.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[0][mpi];
            meshPart.Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        MainBody.Draw();
    }

    private void DrawFrontWheels() {
        var frontLeftWorld = FrontLeftWheel.ParentBone.ModelTransform * World;
        for (int mpi = 0; mpi < FrontLeftWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[2][mpi];
            meshPart.Effect.Parameters["World"].SetValue(Matrix.CreateRotationY(wheelRotation) * frontLeftWorld);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        FrontLeftWheel.Draw();

        var frontRightWorld = FrontRightWheel.ParentBone.ModelTransform * World;
        for (int mpi = 0; mpi < FrontRightWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[1][mpi];
            meshPart.Effect.Parameters["World"].SetValue(Matrix.CreateRotationY(wheelRotation) * frontRightWorld);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        FrontRightWheel.Draw();
    }

    private void DrawBackWheels() {
        var backLeftWorld = BackLeftWheel.ParentBone.ModelTransform * World;
        for (int mpi = 0; mpi < BackLeftWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[3][mpi];
            meshPart.Effect.Parameters["World"].SetValue(backLeftWorld);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        BackLeftWheel.Draw();

        var backRightWorld = BackRightWheel.ParentBone.ModelTransform * World;
        for (int mpi = 0; mpi < BackRightWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[4][mpi];
            meshPart.Effect.Parameters["World"].SetValue(backRightWorld);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        BackRightWheel.Draw();
    }
}


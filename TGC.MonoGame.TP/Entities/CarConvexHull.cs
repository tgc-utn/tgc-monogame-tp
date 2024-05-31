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

public class CarConvexHull   
{
    public bool CanShoot { get; set; }
    public Vector3 Position { get; set; }
    public RigidPose Pose { get; private set; }
    public Matrix World { get; set; }

    private Model Model;
    private ModelMesh MainBody;
    private ModelMesh FrontLeftWheel;
    private ModelMesh FrontRightWheel;
    private ModelMesh BackLeftWheel;
    private ModelMesh BackRightWheel;

    public BodyHandle CarHandle { get; private set; }

    private ConvexHull CarConvex;

    public Quaternion quaternion = new Quaternion();

    public Quaternion rotationQuaternion = new Quaternion();

    public float maxSpeed = 20f;

    public float maxTurn = 3f;

    private Effect Effect;

    private float wheelRotation = 0f;

    private static NumericVector3[] carColliderVertices = new NumericVector3[]
    {
        // Bottom vertices
        new NumericVector3(-1.0f, 0.0f, -2.0f),
        new NumericVector3(1.0f, 0.0f, -2.0f),
        new NumericVector3(1.0f, 0.0f, 2.0f),
        new NumericVector3(-1.0f, 0.0f, 2.0f),

        // middle vertices
        new NumericVector3(-1.0f, 1.5f, -2.7f),
        new NumericVector3(1.0f, 1.5f, -2.7f),
        new NumericVector3(1.0f, 1.5f, 3.2f),
        new NumericVector3(-1.0f, 1.5f, 3.5f),

        // Top vertices
        new NumericVector3(-0.8f, 3f, -1.5f),
        new NumericVector3(0.8f, 3f, -1.5f),
        new NumericVector3(0.8f, 3f, 1.5f),
        new NumericVector3(-0.8f, 3f, 1.5f)
    };

    private List<List<Texture2D>> MeshPartTextures = new List<List<Texture2D>>();

    public CarConvexHull(Vector3 InitialPosition, float Gravity, Simulation Simulation)
    {
        NumericVector3 center;
        Position = InitialPosition;
        CarHandle = new BodyHandle();
        World = new Matrix();
        CarConvex = new ConvexHull(carColliderVertices, Simulation.BufferPool, out center);
        var carBodyDescription = BodyDescription.CreateConvexDynamic(
           new NumericVector3(0, 0, 0),
           new BodyVelocity(new NumericVector3(0, 0, 0)),
           1,
           Simulation.Shapes, CarConvex
       );
        CarHandle = Simulation.Bodies.Add(carBodyDescription);

    }

    public void Load(Model model, Effect effect)
    {

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
                var texture = ((BasicEffect)meshPart.Effect).Texture;
                MeshPartTextures[mi].Add(texture);
                meshPart.Effect = Effect;
            }
        }

    }

    private void MoveCar(KeyboardState keyboardState, GameTime gameTime, BodyReference bodyReference, Simulation simulation)
    {
        float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Define constants for car physics
        float acceleration = 50f;
        float braking = 30f;

        float turnSpeed = 20f;
        float friction = 0.98f;

        // Calculate forward and backward impulses
        var forwardImpulse = new System.Numerics.Vector3(0, 0, -acceleration) * elapsedTime;
        var backwardImpulse = new System.Numerics.Vector3(0, 0, braking) * elapsedTime;
        var linearVelocity = bodyReference.Velocity.Linear;
        var angularImpulse = new System.Numerics.Vector3(0f, turnSpeed * wheelRotation * linearVelocity.Length() / maxSpeed, 0f) * elapsedTime;
        var awake = bodyReference.Awake;

        // Apply forward/backward impulses relative to the car's orientation
        if (keyboardState.IsKeyDown(Keys.W))
        {
            if (!awake) bodyReference.SetLocalInertia(bodyReference.LocalInertia);
            var transformedForwardImpulse = System.Numerics.Vector3.Transform(forwardImpulse, bodyReference.Pose.Orientation);
            bodyReference.ApplyLinearImpulse(new System.Numerics.Vector3(transformedForwardImpulse.X, forwardImpulse.Y, transformedForwardImpulse.Z));
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            if (!awake) bodyReference.SetLocalInertia(bodyReference.LocalInertia);
            var transformedBackwardImpulse = System.Numerics.Vector3.Transform(backwardImpulse, bodyReference.Pose.Orientation);
            bodyReference.ApplyLinearImpulse(new System.Numerics.Vector3(transformedBackwardImpulse.X, backwardImpulse.Y, transformedBackwardImpulse.Z));
        }
        if (keyboardState.IsKeyDown(Keys.Space) && bodyReference.Pose.Position.Y <= 2)
        {
            // Aplica un impulso hacia arriba cuando se presiona la tecla de espacio y el objeto est� en el suelo
            bodyReference.ApplyLinearImpulse(System.Numerics.Vector3.UnitY * 100f * (float)gameTime.ElapsedGameTime.TotalSeconds);

        }

        // Apply friction to simulate resistance
        bodyReference.Velocity.Linear *= friction;
        // bodyReference.Velocity.Angular *= friction;

        // Apply angular impulses for turning
        if (keyboardState.IsKeyDown(Keys.A))
        {
            if (!awake) bodyReference.SetLocalInertia(bodyReference.LocalInertia);
            bodyReference.ApplyAngularImpulse(new System.Numerics.Vector3(0, turnSpeed, 0) * elapsedTime);
            // Rotar hacia la izquierda
            // CarRotation += (carRotatingVelocity) * elapsedTime;
            // CarRotation += (carRotatingVelocity) * deltaTime;
            wheelRotation += 5f * elapsedTime;
            wheelRotation = Math.Clamp(wheelRotation, Convert.ToSingle(Math.PI / -4), Convert.ToSingle(Math.PI / +4));
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            if (!awake) bodyReference.SetLocalInertia(bodyReference.LocalInertia);
            bodyReference.ApplyAngularImpulse(new System.Numerics.Vector3(0, -turnSpeed, 0) * elapsedTime);
            // Rotar hacia la Derecha
            // CarRotation += (-carRotatingVelocity) * elapsedTime;
            // CarRotation += (-carRotatingVelocity) * deltaTime;
            wheelRotation -= 5f * elapsedTime;
            wheelRotation = Math.Clamp(wheelRotation, Convert.ToSingle(Math.PI / -4), Convert.ToSingle(Math.PI / 4));
        }
        else
        {
            wheelRotation -= wheelRotation * 2f * elapsedTime;
            bodyReference.Velocity.Angular -= bodyReference.Velocity.Angular * 0.12f;
        }

        var transformedAngularImpulse = System.Numerics.Vector3.Transform(angularImpulse, bodyReference.Pose.Orientation);
        bodyReference.ApplyAngularImpulse(transformedAngularImpulse);

        // Limit the maximum speed
        if (linearVelocity.Length() > maxSpeed)
        {
            linearVelocity = System.Numerics.Vector3.Normalize(linearVelocity) * maxSpeed;
            bodyReference.Velocity.Linear = linearVelocity;
        }
        var angularVelocity = bodyReference.Velocity.Angular;
        if (angularVelocity.Length() > maxTurn)
        {
            angularVelocity = System.Numerics.Vector3.Normalize(angularVelocity) * maxTurn;
            bodyReference.Velocity.Angular = angularVelocity;
        }
    }

    public void Update(KeyboardState keyboardState, GameTime gameTime, Simulation simulation)
    {

        var bodyReference = simulation.Bodies.GetBodyReference(CarHandle);
        bodyReference.Awake = true;

        MoveCar(keyboardState, gameTime, bodyReference, simulation);

        var position = bodyReference.Pose.Position;

        Position = position;

        Pose = bodyReference.Pose;

        quaternion = bodyReference.Pose.Orientation;

        rotationQuaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(180));

        World = Matrix.CreateFromQuaternion(rotationQuaternion * quaternion) * Matrix.CreateTranslation(new Vector3(position.X, position.Y, position.Z));
    }

    public void Draw()
    {
        DrawCarBody();
        DrawFrontWheels();
        DrawBackWheels();
    }

    private void DrawCarBody()
    {
        for (int mpi = 0; mpi < MainBody.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[0][mpi];
            meshPart.Effect.Parameters["World"]?.SetValue(World);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        MainBody.Draw();
    }

    private void DrawFrontWheels()
    {
        var frontLeftWorld = FrontLeftWheel.ParentBone.ModelTransform * World;
        for (int mpi = 0; mpi < FrontLeftWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[2][mpi];
            meshPart.Effect.Parameters["World"]?.SetValue(Matrix.CreateRotationY(wheelRotation) * frontLeftWorld);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        FrontLeftWheel.Draw();

        var frontRightWorld = FrontRightWheel.ParentBone.ModelTransform * World;
        for (int mpi = 0; mpi < FrontRightWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[1][mpi];
            meshPart.Effect.Parameters["World"]?.SetValue(Matrix.CreateRotationY(wheelRotation) * frontRightWorld);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        FrontRightWheel.Draw();
    }

    private void DrawBackWheels()
    {
        var backLeftWorld = BackLeftWheel.ParentBone.ModelTransform * World;
        for (int mpi = 0; mpi < BackLeftWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[3][mpi];
            meshPart.Effect.Parameters["World"]?.SetValue(backLeftWorld);
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


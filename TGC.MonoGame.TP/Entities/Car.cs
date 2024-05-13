using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Collisions;

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
    public OrientedBoundingBox BBox;
    private List<List<Texture2D>> MeshPartTextures = new List<List<Texture2D>>();


    public Car(Vector3 pos)
    {
        Position = pos;
    }

    public Matrix getWorld() { return World; }

    public void Load(Model model, Effect effect) {

        Effect = effect;
        Model = model;
        World = Matrix.Identity;

        MainBody = Model.Meshes[0];
        FrontRightWheel = Model.Meshes[1];
        FrontLeftWheel = Model.Meshes[2];
        BackLeftWheel = Model.Meshes[3];
        BackRightWheel = Model.Meshes[4];

        BBox = OrientedBoundingBox.FromAABB(BoundingVolumesExtensions.CreateAABBFrom(Model));
        BBox = new OrientedBoundingBox(new Vector3(0, 0, 0), new Vector3(5f, 2.5f, 5f));

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

    private void MovePrincipalCarFollowCamara(KeyboardState keyboardState, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        //Fuerza de gravedad 
        Position.Y -= carMass * gravity * deltaTime;

        if (Position.Y <= carInFloor)
        {
            // Reiniciar la posición vertical del coche
            Position.Y = carInFloor;
        }

        if (keyboardState.IsKeyDown(Keys.W))
        {
            // Acelerar hacia adelante en la dirección del coche
            CarVelocity += (acceleration) * deltaTime;
        }

        if (keyboardState.IsKeyDown(Keys.S))
        {
            // Acelerar hacia atrás en la dirección opuesta del coche
            CarVelocity += (-acceleration) * deltaTime;
        }

        if (keyboardState.IsKeyDown(Keys.A))
        {
            // Rotar hacia la izquierda
            // CarRotation += (carRotatingVelocity) * deltaTime;
            wheelRotation += 5f * deltaTime;
            wheelRotation = Math.Clamp(wheelRotation, Convert.ToSingle(Math.PI / -4), Convert.ToSingle(Math.PI / +4));
        } 
        else 
        {
            wheelRotation -= wheelRotation * 2f * deltaTime;
        }

        if (keyboardState.IsKeyDown(Keys.D))
        {
            // Rotar hacia la Derecha
            // CarRotation += (-carRotatingVelocity) * deltaTime;
            wheelRotation -= 5f * deltaTime;
            wheelRotation = Math.Clamp(wheelRotation, Convert.ToSingle(Math.PI / -4), Convert.ToSingle(Math.PI / 4));

        }
        else 
        {
            wheelRotation -= wheelRotation * 2f * deltaTime;
        }

        // Frenado gradual por fricción
        if (CarVelocity != stopCar)
        {
            // Calcular la dirección opuesta a la velocidad actual
            float direction = Math.Sign(CarVelocity);

            // Calcular la cantidad de frenado basada en la velocidad actual y el coeficiente de fricción
            float friction = frictionCoefficient * direction * deltaTime;

            // Aplicar el frenado por fricción
            CarVelocity -= friction;

            // Asegurar que la velocidad no se vuelva negativa
            if (Math.Sign(CarVelocity) != direction)
            {
                CarVelocity = stopCar;
            }
        }


        // Actualizar la posición del coche en función de su velocidad, rotacion y ultima posicion 
        // Position = Position + Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(CarRotation)) * CarVelocity;
        CarRotation += CarVelocity * wheelRotation * 4f * deltaTime;
        Position.X += Convert.ToSingle(Math.Sin(CarRotation)) * CarVelocity;
        Position.Z += Convert.ToSingle(Math.Cos(CarRotation)) * CarVelocity;

        // Limitar la velocidad máxima y mínima
        CarVelocity = MathHelper.Clamp(CarVelocity, minVelocity, maxVelocity);

        // Actualizar la matriz de transformación del coche
        World = Matrix.CreateRotationY(CarRotation) * Matrix.CreateTranslation(Position);
    }

    public void Chocar() {
        this.CarVelocity = this.CarVelocity * -0.5f;
    }

    public void Update(KeyboardState keyboardState, GameTime gameTime) {
        BBox.Orientation = Matrix.CreateRotationY(CarRotation);
        MovePrincipalCarFollowCamara(keyboardState, gameTime);
    }



    public void Draw() {
        DrawCarBody();
        DrawFrontWheels();
        DrawBackWheels();
    }

    private void DrawCarBody() {
        World = MainBody.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * Matrix.CreateRotationY(CarRotation) * Matrix.CreateTranslation(Position);
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
        World = FrontLeftWheel.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * Matrix.CreateRotationY(CarRotation) * Matrix.CreateTranslation(Position);
        for (int mpi = 0; mpi < FrontLeftWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[2][mpi];
            meshPart.Effect.Parameters["World"].SetValue(Matrix.CreateRotationY(wheelRotation) * World);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        FrontLeftWheel.Draw();

        World = FrontRightWheel.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * Matrix.CreateRotationY(CarRotation) * Matrix.CreateTranslation(Position);
        for (int mpi = 0; mpi < FrontRightWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[1][mpi];
            meshPart.Effect.Parameters["World"].SetValue(Matrix.CreateRotationY(wheelRotation) * World);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        FrontRightWheel.Draw();
    }

    private void DrawBackWheels() {
        World = BackLeftWheel.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * Matrix.CreateRotationY(CarRotation) * Matrix.CreateTranslation(Position);
        for (int mpi = 0; mpi < BackLeftWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[3][mpi];
            meshPart.Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        BackLeftWheel.Draw();

        World = BackRightWheel.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * Matrix.CreateRotationY(CarRotation) * Matrix.CreateTranslation(Position);
        for (int mpi = 0; mpi < BackRightWheel.MeshParts.Count; mpi++)
        {
            var meshPart = MainBody.MeshParts[mpi];
            var texture = MeshPartTextures[4][mpi];
            meshPart.Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["ModelTexture"].SetValue(texture);
        }
        BackRightWheel.Draw();
    }
}


using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP;

public class Car 
{
    private Vector3 Position;
    private Matrix World { get; set; }
    private Model Model;
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


    public Car(Vector3 pos)
    {
        Position = pos;
    }

    public Matrix getWorld() { return World; }

    public void Load(Model model, Effect effect) {

        Effect = effect;
        Model = model;
        World = Matrix.Identity;

        foreach (var mesh in model.Meshes)
        {
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            foreach (var meshPart in mesh.MeshParts)
            {
                meshPart.Effect = Effect;
            }
        }

    }




    private void MovePrincipalCarFollowCamara(KeyboardState keyboardState, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (keyboardState.IsKeyDown(Keys.Space))
        {
            //Mantener apretado para saltar 
            Position.Y += jumpSpeed * deltaTime;
        }
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
            CarRotation += (carRotatingVelocity) * deltaTime;
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            // Rotar hacia la Derecha
            CarRotation += (-carRotatingVelocity) * deltaTime;

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
        Position = Position + Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(CarRotation)) * CarVelocity;

        // Limitar la velocidad máxima y mínima
        CarVelocity = MathHelper.Clamp(CarVelocity, minVelocity, maxVelocity);

        // Actualizar la matriz de transformación del coche
        World = Matrix.CreateRotationY(CarRotation) * Matrix.CreateTranslation(Position);
    }

    public void Update(KeyboardState keyboardState, GameTime gameTime) {
        MovePrincipalCarFollowCamara(keyboardState, gameTime);
    }



    public void Draw() {
        foreach (var mesh in Model.Meshes)
        {
            var scale = 1f;
            World =  mesh.ParentBone.ModelTransform * Matrix.CreateScale(scale) * Matrix.CreateRotationY(CarRotation) * Matrix.CreateTranslation(Position);
            foreach (var meshPart in mesh.MeshParts) {
                meshPart.Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                meshPart.Effect.Parameters["World"].SetValue(World);
            }

            mesh.Draw();
        }
    }
}


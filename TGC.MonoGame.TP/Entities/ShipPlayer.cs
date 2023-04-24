using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using TGC.MonoGame.TP.Camera;

namespace TGC.MonoGame.TP.Entities;

public class ShipPlayer
{
    public const string ContentFolder3D = "Models/";
    private Model Model { get; set; }
    private Effect Effect { get; set; }
    private Matrix World { get; set; }
    private float Rotation { get; set; }
    private Vector3 Position { get; set; }
    private float RotationVelocity { get; set; } = 3f;
    private float Velocity { get; set; } = 10f;

    // Uso el constructor como el Initialize
    public ShipPlayer()
    {
        World = Matrix.Identity;
        Position = Vector3.Zero;
        Rotation = 0f;
    }
    
    public void LoadContent(ContentManager content, Effect effect)
    {
        Effect = effect;
        Model = content.Load<Model>(ContentFolder3D + "ShipA/Ship");

        foreach (var mesh in Model.Meshes)
        {
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            foreach (var meshPart in mesh.MeshParts)
            {
                meshPart.Effect = Effect;
            }
        }
    }
    
    public void Update(GameTime gameTime, FollowCamera followCamera)
    {
        var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
        var keyboardState = Keyboard.GetState();
        
        // Capturar Input teclado
        ResolveShipRotation(deltaTime, keyboardState);
        ResolveShipMovement(deltaTime, keyboardState);
        World = Matrix.CreateScale(0.00025f) * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(Position);
        Debug.WriteLine("[Ship Position] " + Position);
        followCamera.Update(gameTime, World);
    }

    private void ResolveShipMovement(float deltaTime, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                Position += Matrix.CreateRotationY(Rotation).Right * deltaTime * Velocity;
            } else if (keyboardState.IsKeyDown(Keys.S))
            {
                Position += Matrix.CreateRotationY(Rotation).Left * deltaTime * Velocity;
            }
        }
        
    private void ResolveShipRotation(float deltaTime, KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.A))
        {
            Rotation += deltaTime * RotationVelocity;
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            Rotation -= deltaTime * RotationVelocity;
        }
    }
    public void Draw(FollowCamera followCamera)
    {
        Effect.Parameters["View"].SetValue(followCamera.View);
        Effect.Parameters["Projection"].SetValue(followCamera.Projection);
        Effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
        var modelMeshesBaseTransforms = new Matrix[Model.Bones.Count];
        Model.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransforms);
        foreach (var mesh in Model.Meshes)
        {
            var relativeTransform = modelMeshesBaseTransforms[mesh.ParentBone.Index];
            Effect.Parameters["World"].SetValue(relativeTransform * World);
            mesh.Draw();
        }
    }
}
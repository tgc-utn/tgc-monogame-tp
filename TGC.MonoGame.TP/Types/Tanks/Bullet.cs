using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types.Tanks;

public class Bullet : ICollidable
{
    public Model BulletModel { get; set; }
    public readonly Effect BulletEffect;
    public readonly ModelReference BulletReference;
    public Matrix World { get; set; }
    public Matrix Rotation { get; set; }
    public Matrix TankFixRotation { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Direction { get; set; }
    public float Speed { get; set; }
    public float LifeTime { get; set; }
    public bool IsAlive { get; set; } = true;
    
    public BoundingSphere Box;

    public Bullet(Model model, Effect bulletEffect, ModelReference bulletReference, Matrix rotation,
        Matrix tankFixRotation, Vector3 position, Vector3 direction, float speed, float lifeTime)
    {
        BulletModel = model;
        BulletReference = bulletReference;
        BulletEffect = bulletEffect;
        Rotation = rotation;
        TankFixRotation = tankFixRotation;
        Position = position;
        Direction = direction;
        Speed = speed;
        LifeTime = lifeTime;
        
        Box = BoundingVolumesExtension.CreateSphereFrom(model);
        Box.Center = Position;
        Box.Radius *= bulletReference.Scale;
    }

    public void Update(GameTime gameTime)
    {
        if (IsAlive)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
            Position += Direction * Speed * elapsedTime;
            World = Matrix.CreateTranslation(Position);
            
            // Box
            Box.Center = Position;
            LifeTime -= elapsedTime;
            if (LifeTime <= 0)
            {
                IsAlive = false;
            }
        }
    }

    public void Draw(Matrix view, Matrix projection, Vector3 lightPosition, Vector3 lightViewProjection)
    {
        if (IsAlive)
        {
            BulletModel.Root.Transform = Matrix.CreateScale(BulletReference.Scale) * Rotation;
            BulletEffect.Parameters["View"]?.SetValue(view);
            BulletEffect.Parameters["Projection"]?.SetValue(projection);

            // Draw the model.
            foreach (var mesh in BulletModel.Meshes)
            {
                EffectsRepository.SetEffectParameters(BulletEffect, BulletReference.DrawReference, mesh.Name);
                var worldMatrix = mesh.ParentBone.Transform  * TankFixRotation * Matrix.CreateRotationY((float)Math.PI) * World;
                BulletEffect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
                BulletEffect.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(worldMatrix)));
                BulletEffect.Parameters["WorldViewProjection"]?.SetValue(worldMatrix * view * projection);
                BulletEffect.Parameters["lightPosition"]?.SetValue(lightPosition);
                BulletEffect.Parameters["eyePosition"]?.SetValue(lightViewProjection);
                mesh.Draw();
            }
        }
    }

    public void CollidedWithSmallProp()
    {
        IsAlive = false;
    }

    public void CollidedWithLargeProp()
    {
        IsAlive = false;
    }

    public bool VerifyCollision(BoundingBox box)
    {
        return Box.Intersects(box);
    }
}
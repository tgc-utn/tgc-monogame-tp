using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Types.Tanks;

public class Bullet : Resource, ICollidable
{
    // SETTINGS
    public float Speed = 0.2f;
    public float LifeTime = 10000f;
    public float Gravity = 0.001f;      
    
    // Status
    public bool IsAlive { get; set; } = true;
    
    // COORDS
    public Matrix Rotation { get; set; }
    public Matrix TankFixRotation { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Direction { get; set; }
    
    
    // Box Parameters
    public BoundingSphere Box;
    
    public Bullet(Model model, Effect bulletEffect, ModelReference bulletReference, Matrix rotation,
        Matrix tankFixRotation, Vector3 position, Vector3 direction)
    {
        Model = model;
        Reference = bulletReference;
        Effect = bulletEffect;
        Rotation = rotation;
        TankFixRotation = tankFixRotation;
        Position = position;
        Direction = direction;
        // BOX
        Box = BoundingVolumesExtension.CreateSphereFrom(model);
        Box.Center = Position;
        Box.Radius *= bulletReference.Scale;
    }
    
    // Update
    
    public void Update(GameTime gameTime)
    {
        var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
        Direction -= Vector3.Up * Gravity;
        Position += Direction * Speed * elapsedTime;
        World = Matrix.CreateTranslation(Position);

        if (Position.Y < 0)
            IsAlive = false;
            
        // Box
        Box.Center = Position;
        LifeTime -= elapsedTime;
        if (LifeTime <= 0)
            IsAlive = false;
    }
    
    // Draw
    
    public override void DrawOnShadowMap(Camera camera, SkyDome skyDome, RenderTarget2D ShadowMapRenderTarget,
        GraphicsDevice GraphicsDevice, Camera TargetLightCamera, bool modifyRootTransform = true)
    {
        base.DrawOnShadowMap(camera, skyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera, false);
    }

    public override void Draw(Camera camera, SkyDome skyDome, RenderTarget2D ShadowMapRenderTarget, GraphicsDevice GraphicsDevice,
        Camera TargetLightCamera, List<Vector3> ImpactPositions = null, List<Vector3> ImpactDirections = null, bool modifyRootTransform = true)
    {
        Model.Root.Transform = Matrix.CreateScale(Reference.Scale) * Rotation;
        base.Draw(camera, skyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera,
            modifyRootTransform: false);
    }
    
    // ICollidable
    
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
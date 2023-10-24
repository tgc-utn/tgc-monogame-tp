using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.HUD;
using TGC.MonoGame.TP.Types.Props;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TGC.MonoGame.TP.Types.Tanks;

public class Tank : Resource, ICollidable
{
    // Configs
    private const float Acceleration = 0.0055f;
    private const float MaxSpeed = 0.01f;
    private const float RotationSpeed = 0.01f;
    private const float Friction = 0.004f;
    
    public TankReference TankRef;
    
    public bool isPlayer = false;
    
    public float Velocidad;
    
    // Torret
    private ModelBone turretBone;
    private ModelBone cannonBone;

    private Matrix[] boneTransforms;
    private Matrix turretTransform;
    private Matrix cannonTransform;

    private Matrix cannonTest;

    public Matrix TurretRotation { get; set; }
    public Matrix CannonRotation { get; set; }

    private Vector2 pastMousePosition;
    public float MouseSensitivity { get; } = 0.008f;

    private float pitch;
    private float yaw = -90f;
    
    // Box Parameters
    public Vector3 Position;
    private Vector3 LastPosition;
    
    public Matrix OBBWorld { get; set; }
    public float Angle { get; set; } = 0f;
    public Matrix Translation { get; set; }
    public OrientedBoundingBox Box { get; set; }

    // Bullet
    public Model BulletModel { get; set; }
    public Effect BulletEffect;
    public ModelReference BulletReference;
    public List<Bullet> Bullets { get; set; } = new();
    
    //HUD
    public CarHUD TankHud { get; set; }
    public float health { get; set; } = 1f;
    public bool curandose { get; set; } = true;
    public float shootTime { get; set; } = 0f;
    private bool hasShot = false;
    
    public Tank(TankReference model, Vector3 position, GraphicsDeviceManager graphicsDeviceManager)
    {
        Reference = model.Tank;
        TankRef = model;
        // _velocidad = 0;
        // Rotation = Matrix.Identity;
        Position = position;
        TurretRotation = Matrix.Identity;
        CannonRotation = Matrix.Identity;
        
        TankHud = new CarHUD(graphicsDeviceManager);
    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        Translation = Matrix.CreateTranslation(Position);
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateRotationY(Angle) * Translation;
        
        var temporaryCubeAABB = BoundingVolumesExtension.CreateAABBFrom(Model);
        temporaryCubeAABB = BoundingVolumesExtension.Scale(temporaryCubeAABB, 0.015f);
        Box = OrientedBoundingBox.FromAABB(temporaryCubeAABB);
        Box.Center = Position;
        Box.Orientation = Matrix.CreateRotationY(Angle);
        OBBWorld = Matrix.CreateScale(Box.Extents) * Box.Orientation * Translation;
        
        // Torret
        turretBone = Model.Bones[TankRef.TurretBoneName];
        cannonBone = Model.Bones[TankRef.CannonBoneName];
        turretTransform = turretBone.Transform;
        cannonTransform = cannonBone.Transform;
        boneTransforms = new Matrix[Model.Bones.Count];
        
        // Bullet
        BulletReference = Utils.Models.Props.Bullet;
        BulletModel = content.Load<Model>(BulletReference.Path);
        BulletEffect = EffectsRepository.GetEffect(BulletReference.DrawReference, content);
        TexturesRepository.InitializeTextures(BulletReference.DrawReference, content);
        foreach (var modelMeshPart in BulletModel.Meshes.SelectMany(tankModelMesh => tankModelMesh.MeshParts))
        {
            modelMeshPart.Effect = BulletEffect;
        }
        
        //HUD
        TankHud.Load(content);
    }

    public void Update(GameTime gameTime)
    {
        var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
        if (isPlayer)
        {
            KeySense();
            ProcessMouse(elapsedTime);
        }

        LastPosition = Position;
        var rotation = Matrix.CreateRotationY(Angle);
        Position += Vector3.Transform(Vector3.Forward, rotation) * Velocidad * elapsedTime;
        Translation = Matrix.CreateTranslation(Position);
        Velocidad = Math.Max(0, Velocidad - Friction);
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * rotation * Translation;
        
        // Box
        Box.Orientation = rotation;
        Box.Center = Position;
        OBBWorld = Matrix.CreateScale(Box.Extents) * Box.Orientation * Translation;
        
        
        // Bullet
        foreach (var bullet in Bullets)
        {
            bullet.Update(gameTime);
        }
        
        // Valor de incremento o decremento de la salud
        var incremento = curandose ? -0.01f : 0.01f;

        health = Math.Clamp(health + incremento, 0.0f, 1.0f);

        if (health <= 0.0f || health >= 1.0f)
        {
            curandose = !curandose;
        }
        
        if (hasShot)
        {
            shootTime -= elapsedTime * 0.0005f;
            if (shootTime <= 0)
            {
                hasShot = false;
            }
        }
        
        TankHud.Update(World, health, shootTime);
    }

    public override void Draw(Matrix view, Matrix projection, Vector3 lightPosition, Vector3 lightViewProjection)
    {
        turretBone.Transform = TurretRotation * turretTransform;
        cannonBone.Transform =
            turretTransform * Matrix.CreateRotationZ((float)Math.PI) * cannonTransform * CannonRotation;
        Model.CopyAbsoluteBoneTransformsTo(boneTransforms);
        
        foreach (var bullet in Bullets)
        {
            bullet.Draw(view, projection, lightPosition, lightViewProjection);
        }
        
        base.Draw(view, projection, lightPosition, lightViewProjection);
        TankHud.Draw(projection);
    }
    
    public void KeySense()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            // Avanzo
            Velocidad += Acceleration;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            // Retrocedo
            Velocidad -= Acceleration;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            // Giro izq
            Angle += RotationSpeed;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            // Giro der
            Angle -= RotationSpeed;
        }
    }

    public void ProcessMouse(float elapsedTime)
    {
        var currentMouseState = Mouse.GetState();

        var mouseDelta = currentMouseState.Position.ToVector2() - pastMousePosition;
        mouseDelta *= MouseSensitivity * elapsedTime;

        // canion
        pitch = Math.Clamp(pitch - mouseDelta.Y, -8f, 10f);

        // Torreta
        yaw = Math.Clamp(yaw - mouseDelta.X, -90.0f, 90.0f);

        UpdateRotations();

        pastMousePosition = Mouse.GetState().Position.ToVector2();
        
        // Disparo
        if (Mouse.GetState().LeftButton == ButtonState.Pressed && !hasShot)
        {
            var bulletPosition = Position;
            var bulletDirection = Vector3.Forward;
            var bullet = new Bullet(BulletModel, BulletEffect, BulletReference,
                TurretRotation, Reference.Rotation,
                bulletPosition, bulletDirection, 0.1f, 10000f);
            Bullets.Add(bullet);
            hasShot = true;
            shootTime = 1.0f;
        }
    }
    
    public void UpdateRotations()
    {
        var yawRadians = MathHelper.ToRadians(yaw);
        var pitchRadians = MathHelper.ToRadians(pitch);

        Matrix turretRotation = Matrix.CreateRotationZ(yawRadians);
        Matrix cannonRotation = Matrix.CreateRotationX(pitchRadians) * Matrix.CreateRotationZ(yawRadians);

        TurretRotation = turretRotation;
        CannonRotation = cannonRotation;
    }
    
    public void CollidedWithSmallProp()
    {
        Console.WriteLine("Chocaste con prop chico" + $"{DateTime.Now}");
        Velocidad *= 0.5f;
    }

    public void CollidedWithLargeProp()
    {
        Console.WriteLine("Chocaste con prop grande" + $"{DateTime.Now}");
        Velocidad = 0;
        Position = LastPosition;
    }

    public bool VerifyCollision(BoundingBox box)
    {
        return Box.Intersects(box);
    }
}

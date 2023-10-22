using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.HUD;
using TGC.MonoGame.TP.Types.Props;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

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
    public CarHUD CarHud { get; set; }
    public float health { get; set; } = 1f;
    public bool curandose { get; set; } = true;
    
    public Tank(TankReference model, Vector3 position, GraphicsDeviceManager graphicsDeviceManager)
    {
        Reference = model.Tank;
        TankRef = model;
        // _velocidad = 0;
        // Rotation = Matrix.Identity;
        Position = position;
        TurretRotation = Matrix.Identity;
        CannonRotation = Matrix.Identity;
        
        CarHud = new CarHUD(graphicsDeviceManager);
    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        Translation = Matrix.CreateTranslation(Position);
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateRotationY(Angle) * Translation;
        
        var temporaryCubeAABB = BoundingVolumesExtension.CreateAABBFrom(Model);
        temporaryCubeAABB = BoundingVolumesExtension.Scale(temporaryCubeAABB, 0.025f);
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
        CarHud.Load(content);
    }

    public void Update(GameTime gameTime)
    {
        var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
        if (isPlayer)
        {
            KeySense();
            ProcessMouse(elapsedTime);
        }

        var rotation = Matrix.CreateRotationY(Angle);
        Position += Vector3.Transform(Vector3.Forward, rotation) * Velocidad * elapsedTime;
        Translation = Matrix.CreateTranslation(Position);
        Velocidad = Math.Max(0, Velocidad - Friction);
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * rotation * Translation;
        
        // Box
        Box.Orientation = rotation;
        OBBWorld = Matrix.CreateScale(Box.Extents) * Box.Orientation * Translation;
        
        
        // Bullet
        foreach (var bullet in Bullets)
        {
            bullet.Update(gameTime);
        }

        if (curandose)
        {
            health -= 0.01f;
            if (health <= 0f)
            {
                health = 0f;
                curandose = false;
            }
        }
        else
        {
            health += 0.01f;
            if (health >= 1f)
            {
                health = 1f;
                curandose = true;
            }
        }
        CarHud.Update(World, health);
    }

    public override void Draw(Matrix view, Matrix projection)
    {
        turretBone.Transform = TurretRotation * turretTransform;
        cannonBone.Transform =
            turretTransform * Matrix.CreateRotationZ((float)Math.PI) * cannonTransform * CannonRotation;
        Model.CopyAbsoluteBoneTransformsTo(boneTransforms);
        
        foreach (var bullet in Bullets)
        {
            bullet.Draw(view, projection);
        }
        
        base.Draw(view, projection);
        CarHud.Draw(projection);
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
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            var bulletPosition = Position;
            var bulletDirection = Vector3.Forward;
            var bullet = new Bullet(BulletModel, BulletEffect, BulletReference,
                TurretRotation, Reference.Rotation,
                bulletPosition, bulletDirection, 0.1f, 10000f);
            Bullets.Add(bullet);
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
        Velocidad = 0.5f;
    }

    public void CollidedWithLargeProp()
    {
        Console.WriteLine("Chocaste con prop grande" + $"{DateTime.Now}");
    }

    public bool VerifyCollision(BoundingBox box)
    {
        return Box.Intersects(box);
    }
}

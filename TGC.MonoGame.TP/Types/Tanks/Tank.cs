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
using TGC.MonoGame.TP.Utils.Effects;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TGC.MonoGame.TP.Types.Tanks;

public abstract class Tank : Resource, ICollidable
{
    // Configs
    public const float Acceleration = 0.0045f;
    public const float MaxSpeed = 0.01f;
    public const float RotationSpeed = 0.01f;
    public const float Friction = 0.004f;
    
    public TankReference TankRef;
    
    public float Velocidad;

    public List<Vector3> ImpactPositions { get; set; } = new();
    public List<Vector3> ImpactDirections { get; set; } = new();
    
    // Torret
    public ModelBone turretBone;
    public ModelBone cannonBone;

    public Matrix[] boneTransforms;
    public Matrix turretTransform;
    public Matrix cannonTransform;

    public Matrix cannonTest;

    public Matrix TurretRotation { get; set; }
    public Matrix CannonRotation { get; set; }

    public Vector2 pastMousePosition;
    public float MouseSensitivity { get; } = 0.008f;

    public float pitch;
    public float yaw = -90f;
    
    // Box Parameters
    public Vector3 Position;
    public Vector3 LastPosition;
    public Vector3 RespawnPosition;
    
    public Matrix OBBWorld { get; set; }
    public float Angle { get; set; } = 0f;
    public Matrix Translation { get; set; }
    public OrientedBoundingBox Box { get; set; }

    // Bullet
    public Model BulletModel { get; set; }
    public Effect BulletEffect;
    public Effect Effect;
    public ModelReference BulletReference;
    public List<Bullet> Bullets { get; set; } = new();
    
    public Point _center;
    public float _sensitivityX = 0.0018f;
    public float _sensitivityY = 0.002f;
    
    //HUD
    public CarHUD TankHud { get; set; }
    public int health { get; set; } = 5;
    public bool curandose { get; set; } = true;
    public float shootTime { get; set; } = 2.5f;
    public bool hasShot = true;
    
    public Tank(TankReference model, Vector3 position, GraphicsDeviceManager graphicsDeviceManager)
    {
        Reference = model.Tank;
        TankRef = model;
        // _velocidad = 0;
        // Rotation = Matrix.Identity;
        Position = position;
        RespawnPosition = position;
        TurretRotation = Matrix.Identity;
        CannonRotation = Matrix.Identity;
        var w = graphicsDeviceManager.GraphicsDevice.Viewport.Width / 2;
        var h = graphicsDeviceManager.GraphicsDevice.Viewport.Height / 2;
        _center = new Point(w, h);
        
        TankHud = new CarHUD(graphicsDeviceManager);
    }

    public override void Load(ContentManager content)
    {
        // Deformation effect
        Effect = content.Load<Effect>(ContentFolder.Effects + "/DeformationShader");
        
        Model = content.Load<Model>(Reference.Path);
        TexturesRepository.InitializeTextures(Reference.DrawReference, content);
        foreach (var modelMeshPart in Model.Meshes.SelectMany(tankModelMesh => tankModelMesh.MeshParts))
        {
            modelMeshPart.Effect = Effect;
        }
        
        Translation = Matrix.CreateTranslation(Position);
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateRotationY(Angle) * Translation;
        
        var temporaryCubeAABB = BoundingVolumesExtension.CreateAABBFrom(Model);
        temporaryCubeAABB = BoundingVolumesExtension.Scale(temporaryCubeAABB, 0.005f);
        Box = OrientedBoundingBox.FromAABB(temporaryCubeAABB);
        Box.Center = Position;
        Box.Orientation = Matrix.CreateRotationY(Angle);
        OBBWorld = Matrix.CreateScale(Box.Extents * 2) * Box.Orientation * Translation;
        
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

    public virtual void Update(GameTime gameTime)
    {
        var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
        
        LastPosition = Position;
        var rotation = Matrix.CreateRotationY(Angle);
        Position += Vector3.Transform(Vector3.Forward, rotation) * Velocidad * elapsedTime;
        Translation = Matrix.CreateTranslation(Position);
        Velocidad = Math.Max(0, Velocidad - Friction);
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * rotation * Translation;
        
        // Box
        Box.Orientation = rotation;
        Box.Center = Position;
        OBBWorld = Matrix.CreateScale(Box.Extents * 2) * Box.Orientation * Translation;
        
        
        // Bullets
        Bullets = Bullets.Where(bullet => bullet.IsAlive).ToList();
        foreach (var bullet in Bullets)
        {
            bullet.Update(gameTime);
        }
        
        if (health <= 0)
        {
            Respawn();
        }
        
        if (hasShot)
        {
            shootTime -= elapsedTime * 0.0005f;
            if (shootTime <= 0)
            {
                hasShot = false;
            }
        }
    }

    private void Respawn()
    {
        Position = RespawnPosition;
        health = 5;
        Angle = 0f;
        Velocidad = 0f;
        shootTime = 2.5f;
        hasShot = true;
        
        ImpactDirections.Clear();
        ImpactPositions.Clear();
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
        
        Model.Root.Transform = World;

        // Draw the model.
        foreach (var mesh in Model.Meshes)
        {
            EffectsRepository.SetEffectParameters(Effect, Reference.DrawReference, mesh.Name);
            var worldMatrix = mesh.ParentBone.Transform * World;
            Effect.Parameters["World"].SetValue(worldMatrix);
            Effect.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(worldMatrix)));
            Effect.Parameters["View"]?.SetValue(view);
            Effect.Parameters["Projection"]?.SetValue(projection);
            
            Effect.Parameters["ImpactPositions"]?.SetValue(ImpactPositions.ToArray());
            Effect.Parameters["ImpactDirections"]?.SetValue(ImpactDirections.ToArray());
            Effect.Parameters["Impacts"]?.SetValue(ImpactPositions.Count);
            mesh.Draw();
        }
        
        TankHud.Draw(projection);
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

    public void CheckCollisionWithBullet(Bullet bullet)
    {
        if (Box.Intersects(bullet.Box))
        {
            if (Bullets.Contains(bullet))
                return;
            ImpactPositions.Add(bullet.Position);
            ImpactDirections.Add(bullet.Direction);
            bullet.IsAlive = false;
            health -= 1;
            Console.WriteLine("Me pego una bala - Cant impactos en lista = " + ImpactPositions.Count + " - Health: " + health);
        }
    }

    public bool VerifyCollision(BoundingBox box)
    {
        return Box.Intersects(box);
    }
}

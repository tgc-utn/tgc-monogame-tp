using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types.Tanks;

public class Tank : Resource, ICollidable
{
    #region Vars

    // SETTINGS
    public float Acceleration = 0.0045f;
    public float MaxSpeed = 0.01f;
    public float RotationSpeed = 0.01f;
    public float Friction = 0.004f;
    
    // TANK MODEL
    public ActionTank Action { get; set; }
    public TankReference TankRef;
    
    // COORDS
    public Vector3 Position;
    public Vector3 RespawnPosition;
    public float Angle { get; set; } = 0f;
    public Matrix Translation { get; set; }
    
    // Box Parameters
    public Matrix OBBWorld { get; set; }
    public OrientedBoundingBox Box { get; set; }
    
    // Movement
    public float Velocidad;
    public Vector3 LastPosition;
    
    // Torret
    public float pitch;
    public float yaw = -90f;
    public Matrix TurretRotation { get; set; }
    public Matrix CannonRotation { get; set; }
    
    // Torret Bones
    public ModelBone turretBone;
    public ModelBone cannonBone;

    public Matrix[] boneTransforms;
    public Matrix turretTransform;
    public Matrix cannonTransform;
    
    // Shot
    public bool hasShot = true;
    public ModelReference BulletReference;
    public Model BulletModel { get; set; }
    public Effect BulletEffect;
    public List<Bullet> Bullets { get; set; } = new();
    public float shootTime { get; set; } = 2.5f;
    
    // Health
    public int health { get; set; } = 5;
    
    #endregion
    
    public Tank(TankReference modelReference, Vector3 position, GraphicsDeviceManager graphicsDevice, int team, bool player = false)
    {
        if (player)
            Action = new PlayerActionTank(team, graphicsDevice);
        else
            Action = new AIActionTank(team);
        Reference = modelReference.Tank;
        TankRef = modelReference;
        Position = position;
        RespawnPosition = position;
    }
    
    public override void Load(ContentManager contentManager)
    {
        base.Load(contentManager);
        Translation = Matrix.CreateTranslation(Position);
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateRotationY(Angle) * Translation;
        
        // OOBB
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
        BulletModel = contentManager.Load<Model>(BulletReference.Path);
        BulletEffect = EffectsRepository.GetEffect(BulletReference.DrawReference, contentManager);
        TexturesRepository.InitializeTextures(BulletReference.DrawReference, contentManager);
        foreach (var modelMeshPart in BulletModel.Meshes.SelectMany(tankModelMesh => tankModelMesh.MeshParts))
            modelMeshPart.Effect = BulletEffect;
    }
    
    // UPDATE
    
    public void Update(GameTime gameTime)
    {
        // Position
        var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
        LastPosition = Position;
        var rotation = Matrix.CreateRotationY(Angle);
        Position += Vector3.Transform(Vector3.Forward, rotation) * Velocidad * elapsedTime;
        Translation = Matrix.CreateTranslation(Position);
        Velocidad = Math.Max(0, Velocidad - Friction);
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * rotation * Translation;

        // Update Box
        Box.Orientation = rotation;
        Box.Center = Position;
        OBBWorld = Matrix.CreateScale(Box.Extents * 2) * Box.Orientation * Translation;

        Action.Update(gameTime, this);
        
        // Bullets
        Bullets.Where(bullet => bullet.IsAlive).ToList().ForEach(bullet => bullet.Update(gameTime));
        
        if (health <= 0)
            Respawn();
        
        if (hasShot)
        {
            shootTime -= elapsedTime * 0.0005f;
            if (shootTime <= 0)
                hasShot = false;
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
    
    private void Respawn()
    {
        Position = RespawnPosition;
        health = 5;
        Angle = 0f;
        Velocidad = 0f;
        shootTime = 2.5f;
        hasShot = true;
        
        // ImpactDirections.Clear();
        // ImpactPositions.Clear();
    }
    
    // DRAW
    public override void DrawOnShadowMap(Camera camera, SkyDome skyDome, RenderTarget2D ShadowMapRenderTarget,
        GraphicsDevice GraphicsDevice, Camera TargetLightCamera, bool modifyRootTransform = true)
    {
        base.DrawOnShadowMap(camera, skyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        Bullets.Where(bullet => bullet.IsAlive).ToList().ForEach(bullet => bullet.DrawOnShadowMap(camera, skyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera));
    }

    public override void Draw(Camera camera, SkyDome skyDome, RenderTarget2D ShadowMapRenderTarget, GraphicsDevice GraphicsDevice,
        Camera TargetLightCamera, List<Vector3> ImpactPositions = null, List<Vector3> ImpactDirections = null, bool modifyRootTransform = true)
    {
        turretBone.Transform = TurretRotation * turretTransform;
        cannonBone.Transform =
            turretTransform * Matrix.CreateRotationZ((float)Math.PI) * cannonTransform * CannonRotation;
        Model.CopyAbsoluteBoneTransformsTo(boneTransforms);
        base.Draw(camera, skyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera, ImpactPositions, ImpactDirections);
        Bullets.Where(bullet => bullet.IsAlive).ToList().ForEach(bullet => bullet.Draw(camera, skyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera));
    }

    // ICollidable
    public void CollidedWithSmallProp()
    {
        // Console.WriteLine("Chocaste con prop chico" + $"{DateTime.Now}");
        Velocidad *= 0.5f;
    }
    
    public void CollidedWithLargeProp()
    {
        // Console.WriteLine("Chocaste con prop grande" + $"{DateTime.Now}");
        Velocidad = 0;
        Position = LastPosition;
    }

    public bool VerifyCollision(BoundingBox box)
    {
        return Box.Intersects(box);
    }
}

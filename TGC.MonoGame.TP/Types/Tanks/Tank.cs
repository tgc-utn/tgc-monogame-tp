using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.HUD;
using TGC.MonoGame.TP.Maps;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.Utils.Effects;

namespace TGC.MonoGame.TP.Types.Tanks;

public class Tank : Resource, ICollidable
{
    #region Vars

    // SETTINGS
    public float Acceleration = 0.0045f;
    public float MaxSpeed = 0.01f;
    public float RotationSpeed = 0.01f;
    public float Friction = 0.0035f;

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

    // Torret
    public float pitch;
    public float yaw = -90f;
    public Matrix TurretRotation { get; set; } = Matrix.Identity;
    public Matrix CannonRotation { get; set; } = Matrix.Identity;

    // Torret Bones
    public ModelBone turretBone;
    public ModelBone cannonBone;

    public Matrix[] boneTransforms;
    public Matrix turretTransform;
    public Matrix cannonTransform;

    // Shot
    public bool hasShot = true;
    public ModelReference BulletReference;
    public SoundEffect BulletSoundEffect;
    public Model BulletModel { get; set; }
    public Effect BulletEffect;
    public List<Bullet> Bullets { get; set; } = new();
    public float shootTime { get; set; } = 2.5f;
    public List<Vector3> ImpactPositions { get; set; } = new();
    public List<Vector3> ImpactDirections { get; set; } = new();

    // Health
    public int health { get; set; } = 5;
    public bool curandose { get; set; } = true;

    // HUD
    public TankHUD TankHud { get; set; }

    // Wheels
    public ModelBone leftTreadBone;
    public ModelBone rightTreadBone;
    public ModelBone[] leftWheelsBones;
    public ModelBone[] rightWheelsBones;

    public Matrix leftTreadTransform;
    public Matrix rightTreadTransform;
    public Matrix[] leftWheelsTransforms;
    public Matrix[] rightWheelsTransforms;

    public float LeftWheelRotation { get; set; } = 0;
    public float RightWheelRotation { get; set; } = 0;

    #endregion

    public Tank(TankReference modelReference, Vector3 position, GraphicsDeviceManager graphicsDevice, bool isAEnemy,
        int index, Map map, bool player = false)
    {
        if (player)
            Action = new PlayerActionTank(isAEnemy, graphicsDevice);
        else
            Action = new AIActionTank(isAEnemy, index, map);
        Reference = modelReference.Tank;
        TankRef = modelReference;
        Position = position;
        RespawnPosition = position;
        TankHud = new TankHUD(graphicsDevice);
    }

    public override void Load(ContentManager contentManager)
    {
        // base.Load(contentManager);

        Model = contentManager.Load<Model>(Reference.Path);
        Effect = contentManager.Load<Effect>(Effects.DeformationShader.Path);
        TexturesRepository.InitializeTextures(Reference.DrawReference, contentManager);
        foreach (var modelMeshPart in Model.Meshes.SelectMany(tankModelMesh => tankModelMesh.MeshParts))
        {
            modelMeshPart.Effect = Effect;
        }

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

        //HUD
        TankHud.Load(contentManager);

        // Music
        BulletSoundEffect = contentManager.Load<SoundEffect>($"{ContentFolder.Sounds}/fireshot");

        // Wheels
        leftTreadBone = Model.Bones[TankRef.LeftTreadBoneName];
        rightTreadBone = Model.Bones[TankRef.RightTreadBoneName];
        leftTreadTransform = leftTreadBone.Transform;
        rightTreadTransform = rightTreadBone.Transform;

        var wheelsCount = TankRef.LeftWheelsBoneNames.Count;
        leftWheelsBones = new ModelBone[wheelsCount];
        rightWheelsBones = new ModelBone[wheelsCount];
        leftWheelsTransforms = new Matrix[wheelsCount];
        rightWheelsTransforms = new Matrix[wheelsCount];

        for (var i = 0; i < wheelsCount; i++)
        {
            leftWheelsBones[i] = Model.Bones[TankRef.LeftWheelsBoneNames[i]];
            rightWheelsBones[i] = Model.Bones[TankRef.RightWheelsBoneNames[i]];
            leftWheelsTransforms[i] = leftWheelsBones[i].Transform * leftTreadTransform;
            rightWheelsTransforms[i] = rightWheelsBones[i].Transform * rightTreadTransform;
        }
    }

    // UPDATE
    
    public void Update(GameTime gameTime)
    {
        // Position
        var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
        var rotation = Matrix.CreateRotationY(Angle);
        Position += Vector3.Transform(Vector3.Forward, rotation) * Velocidad * elapsedTime;
        Translation = Matrix.CreateTranslation(Position);
        if(Velocidad > 0)
            Velocidad = Math.Max(0, Velocidad - Friction);
        else
            Velocidad = Math.Min(0, Velocidad + Friction);
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
    
    public void Respawn()
    {
        Action.Respawn(this);
        Position = RespawnPosition;
        health = 5;
        Angle = 0f;
        Velocidad = 0f;
        shootTime = 2.5f;
        hasShot = true;
        
        ImpactDirections.Clear();
        ImpactPositions.Clear();
    }
    
    // DRAW
    public override void DrawOnShadowMap(Camera camera, SkyDome skyDome, RenderTarget2D ShadowMapRenderTarget,
        GraphicsDevice GraphicsDevice, Camera TargetLightCamera, bool modifyRootTransform = true)
    {
        // base.DrawOnShadowMap(camera, skyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        Effect.CurrentTechnique = Effect.Techniques["DepthPass"];
        Model.Root.Transform = World;
        foreach (var modelMesh in Model.Meshes)
        {
            foreach (var part in modelMesh.MeshParts)
                part.Effect = Effect;
            var worldMatrix = modelMesh.ParentBone.Transform * World;
            Effect.Parameters["WorldViewProjection"]
                .SetValue(worldMatrix * TargetLightCamera.View * TargetLightCamera.Projection);
            modelMesh.Draw();
        }
        Bullets.Where(bullet => bullet.IsAlive).ToList().ForEach(bullet => bullet.DrawOnShadowMap(camera, skyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera));
    }

    public override void Draw(Camera camera, SkyDome skyDome, RenderTarget2D ShadowMapRenderTarget, GraphicsDevice GraphicsDevice,
        Camera TargetLightCamera, List<Vector3> ImpactPositions = null, List<Vector3> ImpactDirections = null, bool modifyRootTransform = true)
    {
        turretBone.Transform = TurretRotation * turretTransform;
        cannonBone.Transform =
            turretTransform * Matrix.CreateRotationZ((float)Math.PI) * cannonTransform * CannonRotation;
        Model.CopyAbsoluteBoneTransformsTo(boneTransforms);
        // Wheels
        var leftWheelRotation = Matrix.CreateRotationX(LeftWheelRotation);
        var rightWheelRotation = Matrix.CreateRotationX(RightWheelRotation);
        for (var i = 0; i < leftWheelsBones.Length; i++)
        {
            leftWheelsBones[i].Transform = leftWheelRotation * leftWheelsTransforms[i];
            rightWheelsBones[i].Transform = rightWheelRotation * rightWheelsTransforms[i];
        }
        
        Model.Root.Transform = World;
        
        Effect.CurrentTechnique = Effect.Techniques["DrawShadowedPCF"];
        Effect.Parameters["baseTexture"].SetValue((Reference.DrawReference as ShadowBlingPhongReference)?.Texture);
        Effect.Parameters["shadowMap"].SetValue(ShadowMapRenderTarget);
        Effect.Parameters["lightPosition"].SetValue(skyDome.LightPosition);
        Effect.Parameters["shadowMapSize"].SetValue(Vector2.One * TexturesRepository.ShadowmapSize);
        Effect.Parameters["LightViewProjection"].SetValue(TargetLightCamera.View * TargetLightCamera.Projection);
        foreach (var modelMesh in Model.Meshes)
        {
            EffectsRepository.SetEffectParameters(Effect, Reference.DrawReference, modelMesh.Name);
            foreach (var part in modelMesh.MeshParts)
                part.Effect = Effect;
            var worldMatrix = modelMesh.ParentBone.Transform * World;
            
            if (modelMesh.ParentBone.Name == leftTreadBone.Name)
            {
                // Configuración cadena izquierda
                Effect.Parameters["applyTextureScrolling"]?.SetValue(true);
                Effect.Parameters["ScrollSpeed"]?.SetValue(LeftWheelRotation*0.125f);
            }
            
            if (modelMesh.ParentBone.Name == rightTreadBone.Name)
            {
                // Configuración cadena derecha
                Effect.Parameters["applyTextureScrolling"]?.SetValue(true);
                Effect.Parameters["ScrollSpeed"]?.SetValue(RightWheelRotation*0.125f);
            }
            
            Effect.Parameters["WorldViewProjection"].SetValue(worldMatrix * camera.View * camera.Projection);
            Effect.Parameters["World"].SetValue(worldMatrix);
            Effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(worldMatrix)));
            Effect.Parameters["WorldViewProjection"].SetValue(worldMatrix * camera.View * camera.Projection);
            Effect.Parameters["lightPosition"].SetValue(skyDome.LightPosition);
            Effect.Parameters["eyePosition"].SetValue(skyDome.LightViewProjection);
            Effect.Parameters["View"]?.SetValue(camera.View);

            Effect.Parameters["Projection"].SetValue(camera.Projection);
            Effect.Parameters["ImpactPositions"]?.SetValue(this.ImpactPositions.ToArray());
            Effect.Parameters["ImpactDirections"]?.SetValue(this.ImpactDirections.ToArray());
            Effect.Parameters["Impacts"]?.SetValue(this.ImpactPositions.Count);
                
            // Once we set these matrices we draw
            modelMesh.Draw();
            Effect.Parameters["applyTextureScrolling"]?.SetValue(false);
        }
        
        Bullets.Where(bullet => bullet.IsAlive).ToList().ForEach(bullet => bullet.Draw(camera, skyDome, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera));
        TankHud.Draw(camera.Projection);
    }

    // ICollidable
    public void CollidedWithSmallProp()
    {
        Console.WriteLine($"Chocaste con prop chico {DateTime.Now}");
        Velocidad *= 0.5f;
    }
    
    public void CollidedWithLargeProp()
    {
        Console.WriteLine($"Chocaste con prop grande {DateTime.Now}");
        if (Velocidad > 0)
            Velocidad = -0.05f;
        else
            Velocidad = 0.05f;
    }

    public bool VerifyCollision(BoundingBox box)
    {
        return Box.Intersects(box);
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

    public void CollidedWithTank(float velocidad)
    {
        if (velocidad > 0)
        {
            if (Velocidad < 0)
                Velocidad = velocidad;
            else
                Velocidad += velocidad;
        }
        else if(velocidad < 0)
        {
            if (Velocidad > 0)
                Velocidad = velocidad;
            else
                Velocidad += velocidad;
        }
        else
        {
            Velocidad = -0.0001f;
        }
    }
    
    public void CheckCollisionWithTank(Tank tank)
    {
        if (Box.Intersects(tank.Box))
        {
            var v = new Vector2(tank.World.Forward.X, tank.World.Forward.Z);
            var vTank = new Vector2(World.Forward.X, World.Forward.Z);
            var dot = (tank.World.Forward.X * World.Forward.X + tank.World.Forward.Z * World.Forward.Z);
            var angleOfCollision = Math.Acos(dot / (v.Length() * vTank.Length()));
            Console.WriteLine(tank.World.Forward + " - " + World.Forward);
            Console.WriteLine(angleOfCollision);
            if (angleOfCollision < MathHelper.PiOver4)
            {
                var vectorDistance = tank.Position - Position;
                // el tanque que evalua esta atras del que llega por parametro
                Boolean estaAtras = Vector3.Dot(vectorDistance, World.Forward) > 0;
                if (estaAtras)
                {
                    CollidedWithTank(-(tank.Velocidad * 0.5f + 0.01f));
                    tank.CollidedWithTank(Velocidad * 0.5f + 0.01f);
                }
                else
                {
                    CollidedWithTank(tank.Velocidad * 0.5f + 0.01f);
                    tank.CollidedWithTank(-(Velocidad * 0.5f + 0.01f));
                }
            }
            else if(angleOfCollision > MathHelper.PiOver4 + MathHelper.PiOver2)
            {
                CollidedWithTank(-(tank.Velocidad * 0.5f + 0.01f));
                tank.CollidedWithTank(-(Velocidad * 0.5f + 0.01f));
            }
            else
            {
                CollidedWithTank(0);
                tank.CollidedWithTank(0);
            }
        }
    }
}

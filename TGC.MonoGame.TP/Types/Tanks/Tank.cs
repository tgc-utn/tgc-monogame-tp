using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types.Tanks;

public class Tank : Resource, ICollidable
{
    // Configs
    private float Acceleration = 0.0055f;
    private float MaxSpeed = 0.01f;
    private float RotationSpeed = 0.01f;
    private float Friction = 0.004f;

    public TankReference TankRef;

    public Vector3 Position;
    private Vector3 LastPosition;

    private float _velocidad;
    public Matrix Rotation;

    //private Turret Turret;

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
    
    public BoundingBox Box { get; set; }

    public Tank(TankReference model, Vector3 position)
    {
        Reference = model.Tank;
        TankRef = model;
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateTranslation(position);
        _velocidad = 0;
        Rotation = Matrix.Identity;
        Position = position;
        TurretRotation = Matrix.Identity;
        CannonRotation = Matrix.Identity;
    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        Model.Root.Transform = World;
        turretBone = Model.Bones[TankRef.TurretBoneName];
        cannonBone = Model.Bones[TankRef.CannonBoneName];
        turretTransform = turretBone.Transform;
        cannonTransform = cannonBone.Transform;
        boneTransforms = new Matrix[Model.Bones.Count];
       
        Box = BoundingVolumesExtension.CreateAABBFrom(Model);
        Box = new BoundingBox(Box.Min * Reference.Scale + Position, Box.Max * Reference.Scale + Position);
    }

    public override void Draw(Matrix view, Matrix projection)
    {
        Model.Root.Transform = World;
        turretBone.Transform = TurretRotation * turretTransform;
        cannonBone.Transform =
            turretTransform * Matrix.CreateRotationZ((float)Math.PI) * cannonTransform * CannonRotation;

        Model.CopyAbsoluteBoneTransformsTo(boneTransforms);

        Effect.Parameters["View"].SetValue(view);
        Effect.Parameters["Projection"].SetValue(projection);

        foreach (var mesh in Model.Meshes)
        {
            EffectsRepository.SetEffectParameters(Effect, Reference.DrawReference, mesh.Name);
            Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
            mesh.Draw();
        }
    }

    public void Update(GameTime gameTime)
    {
        var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;

        KeySense();
        ProcessMouse(elapsedTime);


        LastPosition = Position;
        Position += Vector3.Transform(Vector3.Forward, Rotation) * _velocidad * elapsedTime;
        Move(Position);
        _velocidad = Math.Max(0, _velocidad - Friction);
        var desplazamiento = (Position - LastPosition) * Reference.Scale;
        Box = new BoundingBox(Box.Min * Reference.Scale + desplazamiento, Box.Max * Reference.Scale + desplazamiento);
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

    public void KeySense()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            // Avanzo
            _velocidad += Acceleration;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            // Retrocedo
            _velocidad -= Acceleration;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            // Giro izq
            Rotation *= Matrix.CreateRotationY(RotationSpeed);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            // Giro der
            Rotation *= Matrix.CreateRotationY(-RotationSpeed);
        }
    }

    public void Move(Vector3 position)
    {
        World = Reference.Rotation * Rotation * Matrix.CreateTranslation(position) *
                Matrix.CreateScale(Reference.Scale);
    }

    public void CollidedWithSmallProp()
    {
        Console.WriteLine("Chocaste con prop chico");
        _velocidad = 0.5f;
    }

    public void CollidedWithLargeProp()
    {
        Console.WriteLine("Chocaste con prop grande");
        _velocidad = 0f;
        // Corrigiendo la posicion del tanque y de la box
        var desplazamiento = (LastPosition - Position) * Reference.Scale;
        Position = LastPosition;
        Box = new BoundingBox(Box.Min * Reference.Scale + desplazamiento, Box.Max * Reference.Scale + desplazamiento);
    }

    public BoundingBox GetBoundingBox() { return Box; }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types.Tanks;

public class Tank : Resource
{
    public TankReference TankRef;
    
    private Vector3 Position;
    private float _velocidad;
    private Matrix _rotacion;

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
    public float MouseSensitivity { get; set; } = 0.008f;
    
    private float pitch;
    private float yaw = -90f;
    
    public Tank(TankReference model, Vector3 position)
    {
        Reference = model.Tank;
        TankRef = model;
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateTranslation(position);
        _velocidad = 0;
        _rotacion = Matrix.Identity;
        Position = position;
        TurretRotation = Matrix.Identity;
        CannonRotation = Matrix.Identity;
    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        turretBone = Model.Bones[TankRef.TurretBoneName];
        cannonBone = Model.Bones[TankRef.CannonBoneName];
        turretTransform = turretBone.Transform;
        cannonTransform = cannonBone.Transform;
        boneTransforms = new Matrix[Model.Bones.Count];
    }

    public override void Draw(Matrix view, Matrix projection)
    {
        Model.Root.Transform = World;
        turretBone.Transform = TurretRotation * turretTransform;
        cannonBone.Transform = turretTransform * Matrix.CreateRotationZ((float)Math.PI) * cannonTransform * CannonRotation;
        
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
        var elapsedTime = (float) gameTime.ElapsedGameTime.Milliseconds;
        
        KeySense();
        ProcessMouse(elapsedTime);
        
        Position += Vector3.Transform(Vector3.Forward, _rotacion) * _velocidad * elapsedTime;
        Move(Position,_rotacion);
        _velocidad = Math.Max(0, _velocidad-0.008f);
        
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
            _velocidad += 0.01f;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            // Retrocedo
            _velocidad -= 0.01f;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            // Giro izq
            _rotacion *= Matrix.CreateRotationY(0.02f);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            // Giro der
            _rotacion *= Matrix.CreateRotationY(-0.02f);
        }
    }

    public void Move(Vector3 position, Matrix rotation)
    {
        World = Reference.Rotation * rotation * Matrix.CreateTranslation(position) * Matrix.CreateScale(Reference.Scale);
    }
    
    public void CollidedWithSmallProp()
    {
        Console.WriteLine("Chocaste con prop chico");
        // TODO frenar un poco el tanque
    }

    public void CollidedWithLargeProp()
    {
        Console.WriteLine("Chocaste con prop grande");
        // TODO frenar el tanque del todo
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types;

public class SkyDome : Resource
{
    private Matrix LightBoxWorld { get; set; } = Matrix.Identity;
    public Vector3 LightPosition  { get; set; } = Vector3.Zero;
    public Vector3 LightViewProjection { get; set; }
    public SpherePrimitive LightBox { get; set; }
    private float Timer { get; set; }
    
    private PropReference Prop;
    public Vector3 Position;
    public Matrix Translation { get; set; }
    public float Angle { get; set; } = 0f;
    
    public SkyDome(PropReference modelReference)
    {
        Reference = modelReference.Prop;
        Prop = modelReference;
        Position = Prop.Position;
    }
    
    public SkyDome(PropReference modelReference, Vector3 position)
    {
        Reference = modelReference.Prop;
        Prop = modelReference;
        Position = position;
    }
    public void Load(GraphicsDevice graphicsDevice, ContentManager content)
    {
        base.Load(content);
        LightBox = new SpherePrimitive(graphicsDevice, 50, 16, new Color(239f, 142f, 56f));
        Translation = Matrix.CreateTranslation(Position);
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateRotationY(Angle) * Translation;
        Model.Root.Transform = World;
    }

    public void Update(GameTime gameTime)
    {
        // Timer += (float) gameTime.ElapsedGameTime.TotalSeconds;
        // LightPosition = new Vector3((float) Math.Cos(Timer * 0.5f) * 650f, (float) Math.Sin(Timer * 0.5f) * 650f, 0);
        // LightBoxWorld = Matrix.CreateTranslation(LightPosition);
    }
    
    public void Draw(Camera camera, RenderTarget2D ShadowMapRenderTarget, GraphicsDevice GraphicsDevice, Camera TargetLightCamera)
    {
        base.Draw(camera, this, ShadowMapRenderTarget, GraphicsDevice, TargetLightCamera);
    } 
}
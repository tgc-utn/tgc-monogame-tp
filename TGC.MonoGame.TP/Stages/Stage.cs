
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Camera;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.MainCharacter;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.Samples.Samples.Shaders.SkyBox;
using TGC.MonoGame.TP;


namespace TGC.MonoGame.TP.Stages;
    public abstract class Stage
{
    public const string ContentFolder3D = "Models/";
    public const string ContentFolderEffects = "Effects/";
    public const string ContentFolderMusic = "Music/";
    public const string ContentFolderSounds = "Sounds/";
    public const string ContentFolderSpriteFonts = "SpriteFonts/";
    public const string ContentFolderTextures = "Textures/";

    protected GraphicsDevice GraphicsDevice;
    protected ContentManager Content;

    public List<GeometricPrimitive> Track; // circuito y obstáculos fijos 
    public List<GeometricPrimitive> Obstacles; // obstáculos móviles
    public List<GeometricPrimitive> Signs; //FIXME: eventualmente podrían ser algo distinto a GeometricPrimitive
    public List<GeometricPrimitive> Pickups; //FIXME: eventualmente podrían ser algo distinto a GeometricPrimitive
    public List<GeometricPrimitive> Checkpoints; // puntos de respawn

    public Vector3 CharacterInitialPosition;
    //private SpriteBatch SpriteBatch;
    
    public void LoadSpriteBatch(){
        //SpriteBatch=new SpriteBatch(GraphicsDevice);
        //SpriteBatch.Begin();
        
    }


        //COLISIONES
        public List<OrientedBoundingBox> Colliders;
        //COLISIONES


    private SpriteFont SpriteFont { get; set; }
    public Vector3 CamPosition{get;set;}
    
    public void Draw(Matrix view, Matrix projection)
    {
        foreach (GeometricPrimitive primitive in Track)
        {
            primitive.Draw(view, projection);
        }

        foreach (GeometricPrimitive primitive in Obstacles)
        {
            primitive.Draw(view, projection);
        }

        foreach (GeometricPrimitive sign in Signs)
        {
            sign.Draw(view, projection);
        }

        foreach (GeometricPrimitive pickup in Pickups)
        {
            pickup.Draw(view, projection);
        }
        SkyBox.Draw(view, projection, CamPosition);
    }
        public Stage(GraphicsDevice graphicsDevice, ContentManager content, Vector3 characterPosition)
        {
            GraphicsDevice = graphicsDevice;
            Content = content;

            CharacterInitialPosition = characterPosition;

            Colliders = new List<OrientedBoundingBox>();

            LoadTrack();
            LoadObstacles();
            LoadSigns();
            LoadPickups();
            LoadCheckpoints();
            LoadSpriteBatch();
            LoadColliders();
            LoadSkyBox();
        }

        public abstract void Update(GameTime gameTime);


        abstract protected void LoadTrack();

        abstract protected void LoadObstacles();

        abstract protected void LoadColliders();

        abstract protected void LoadPickups();

        abstract protected void LoadSigns();

        abstract protected void LoadCheckpoints();



    public Model SkyBoxModel;
    public TextureCube SkyBoxTexture;
    public Effect SkyBoxEffect;
    public SkyBox SkyBox;
    public void LoadSkyBox(){
        SkyBoxModel = Content.Load<Model>(ContentFolder3D + "skybox/cube");
        SkyBoxTexture =Content.Load<TextureCube>(ContentFolderTextures+"skyboxes/skybox/skybox");
        SkyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");
        SkyBox=new SkyBox(SkyBoxModel, SkyBoxTexture, SkyBoxEffect, 1000);
    }


}
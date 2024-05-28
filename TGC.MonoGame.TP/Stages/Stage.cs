
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Camera;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.MainCharacter;

abstract class Stage
{

    protected GraphicsDevice GraphicsDevice;
    protected ContentManager Content;

    protected List<GeometricPrimitive> Track; // circuito y obstáculos fijos 
    protected List<GeometricPrimitive> Obstacles; // obstáculos móviles
    protected List<GeometricPrimitive> Signs; //FIXME: eventualmente podrían ser algo distinto a GeometricPrimitive
    protected List<GeometricPrimitive> Pickups; //FIXME: eventualmente podrían ser algo distinto a GeometricPrimitive
    protected List<GeometricPrimitive> Checkpoints; // puntos de respawn

    public Vector3 CharacterInitialPosition;

    public Stage(GraphicsDevice graphicsDevice, ContentManager content, Vector3 characterPosition)
    {
        GraphicsDevice = graphicsDevice;
        Content = content;

        CharacterInitialPosition = characterPosition;

        LoadTrack();
        LoadObstacles();
        LoadSigns();
        LoadPickups();
        LoadCheckpoints();
    }

    public abstract void Update(GameTime gameTime);


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
    }

    abstract protected void LoadTrack();

    abstract protected void LoadObstacles();

    abstract protected void LoadPickups();

    abstract protected void LoadSigns();

    abstract protected void LoadCheckpoints();


}
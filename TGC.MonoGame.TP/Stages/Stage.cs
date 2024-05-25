
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

    protected List<GeometricPrimitive> Track;
    protected List<GeometricPrimitive> Signs; //FIXME: eventualmente podrían ser algo distinto a GeometricPrimitive
    protected List<GeometricPrimitive> Pickups; //FIXME: eventualmente podrían ser algo distinto a GeometricPrimitive

    public Vector3 CharacterInitialPosition;

    //TODO: por acá podría estar el tema de los puntos de respawn



    public Stage(GraphicsDevice graphicsDevice, ContentManager content, Vector3 characterPosition)
    {
        GraphicsDevice = graphicsDevice;
        Content = content;

        CharacterInitialPosition = characterPosition;

        LoadTrack();
        LoadSigns();
        LoadPickups();
    }

    public void Draw(Matrix view, Matrix projection)
    {
        foreach (GeometricPrimitive primitive in Track)
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

    abstract protected void LoadPickups();

    abstract protected void LoadSigns();

}
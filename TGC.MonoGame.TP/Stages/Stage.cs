
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Camera;
using TGC.MonoGame.TP.Geometries;

abstract class Stage
{

    GraphicsDevice GraphicsDevice;
    ContentManager Content;
    Vector3 CharacterPosition;

    protected List<GeometricPrimitive> Track;


    public Stage(GraphicsDevice graphicsDevice, ContentManager content, Vector3 characterPosition)
    {
        GraphicsDevice = graphicsDevice;
        Content = content;
        CharacterPosition = characterPosition;

        Track = new List<GeometricPrimitive>();
        
    }

    public void AddPrimitive(GeometricPrimitive primitive)
    {
        Track.Add(primitive);
    }

    public void AddPrimitives(List<GeometricPrimitive> primitives)
    {
        foreach (GeometricPrimitive primitive in primitives)
        {
            AddPrimitive(primitive);
        }
    }

    public void Draw(Matrix view, Matrix projection)
    {
        foreach (GeometricPrimitive primitive in Track)
        {
            primitive.Draw(view, projection);
        }
    }

    abstract protected List<GeometricCandidate> TrackParameters();

}

abstract class GeometricCandidate
{

    protected Color Color;
    protected Vector3 Coordinates;
    protected Vector3 Scale;
    protected Matrix Rotation;
    protected float Size;

    public GeometricCandidate(Color color, float size, Vector3? coordinates = null, Vector3? scale = null, Matrix? rotation = null)
    {
        Color = color;
        Size = size;
        Coordinates = coordinates ?? Vector3.Zero;
        Scale = scale ?? Vector3.One;
        Rotation = rotation ?? Matrix.Identity;
    }

    abstract public GeometricPrimitive Build(GraphicsDevice graphicsDevice, ContentManager content);
}

class CubeCandidate : GeometricCandidate
{

    public CubeCandidate(Color color, Vector3? coordinates = null, Vector3? scale = null, Matrix? rotation = null, float size = 25f) :
        base(color, size, coordinates, scale, rotation)
    { }
    override public GeometricPrimitive Build(GraphicsDevice graphicsDevice, ContentManager content)
    {
        return new CubePrimitive(graphicsDevice, content, Color, Size, Coordinates, Scale, Rotation);
    }
}

class RampCandidate : GeometricCandidate
{

    public RampCandidate(Color color, Vector3? coordinates = null, Vector3? scale = null, Matrix? rotation = null, float size = 25f) :
        base(color, size, coordinates, scale, rotation)
    { }
    override public GeometricPrimitive Build(GraphicsDevice graphicsDevice, ContentManager content)
    {
        return new RampPrimitive(graphicsDevice, content, Color, Size, Coordinates, Scale, Rotation);
    }
}

class Stage_01 : Stage
{

    public Stage_01(GraphicsDevice graphicsDevice, ContentManager content) : 
        base(graphicsDevice, content, new Vector3(25, 25, -800)) {}

    protected override List<GeometricCandidate> TrackParameters()
    {
        List<GeometricCandidate> primitiveParameters = new List<GeometricCandidate>();

        // PRIMERA PLATAFORMA
        primitiveParameters.Add(new CubeCandidate(Color.BlueViolet, coordinates: new Vector3(25, 0, 0), scale: new Vector3(3f, 1f, 1f)));
        primitiveParameters.Add(new CubeCandidate(Color.Aquamarine, coordinates: new Vector3(25, 0, 25), scale: new Vector3(3f, 1f, 1f)));
        primitiveParameters.Add(new CubeCandidate(Color.LightPink, coordinates: new Vector3(25, 0, 50), scale: new Vector3(3f, 1f, 1f)));
        primitiveParameters.Add(new CubeCandidate(Color.BlueViolet, coordinates: new Vector3(25, 0, 75), scale: new Vector3(3f, 1f, 1f)));
        primitiveParameters.Add(new CubeCandidate(Color.Aquamarine, coordinates: new Vector3(25, 0, 100), scale: new Vector3(3f, 1f, 1f)));
        primitiveParameters.Add(new CubeCandidate(Color.LightPink, coordinates: new Vector3(25, 0, 125), scale: new Vector3(3f, 1f, 1f)));

        // SEGUNDA PLATAFORMA
        primitiveParameters.Add(new CubeCandidate(Color.BlueViolet, coordinates: new Vector3(25, 0, 175), scale: new Vector3(3f, 1f, 1f)));
        primitiveParameters.Add(new CubeCandidate(Color.Aquamarine, coordinates: new Vector3(25, 0, 200), scale: new Vector3(3f, 1f, 1f)));
        primitiveParameters.Add(new CubeCandidate(Color.LightPink, coordinates: new Vector3(25, 0, 225), scale: new Vector3(3f, 1f, 1f)));

        // ESCALERA
        primitiveParameters.Add(new CubeCandidate(Color.BlueViolet, coordinates: new Vector3(75, 25, 200), scale: new Vector3(1f, 1f, 3f)));
        primitiveParameters.Add(new CubeCandidate(Color.Aquamarine, coordinates: new Vector3(100, 50, 200), scale: new Vector3(1f, 1f, 3f)));
        primitiveParameters.Add(new CubeCandidate(Color.LightPink, coordinates: new Vector3(125, 75, 200), scale: new Vector3(1f, 1f, 3f)));

        // TERCERA PLATAFORMA


        return primitiveParameters;
        
    }


}

// TERCERA PLATAFORMA
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, 225));
Track.Add(Cube);

Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, 200));
Track.Add(Cube);

Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, 175));
Track.Add(Cube);

//CUARTA PLATAFORMA
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
Cube.World = Matrix.CreateScale(1f, 1f, 6f) * Matrix.CreateTranslation(new Vector3(175, 100, 62));
Track.Add(Cube);


// QUINTA PLATAFORMA
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, -50));
Track.Add(Cube);

Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, -75));
Track.Add(Cube);

Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, -100));
Track.Add(Cube);

// RAMPA
Ramp = new RampPrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
Ramp.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 125, -125));
Track.Add(Ramp);

// CANALETA
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
Cube.World = Matrix.CreateScale(3f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(175, 125, -250));
Track.Add(Cube);
Ramp = new RampPrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
Ramp.World = Matrix.CreateScale(9f, 1f, 1f) * Matrix.CreateFromYawPitchRoll((float)Math.PI / 2, 0, 0) * Matrix.CreateTranslation(new Vector3(150, 150, -250)); // * ;
Track.Add(Ramp);
Ramp = new RampPrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
Ramp.World = Matrix.CreateScale(9f, 1f, 1f) * Matrix.CreateFromYawPitchRoll((float)Math.PI / (-2), 0, 0) * Matrix.CreateTranslation(new Vector3(200, 150, -250)); // * ;
Track.Add(Ramp);

// BOLITA
// Propuesta de punto de inicio del escenario
Bola = new SpherePrimitive(GraphicsDevice, Content, 25, 50, Color.Red, Matrix.CreateTranslation(new Vector3(25, 25, -800)));

// PLANOS INCLINADOS (ROLL)
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / (-6)) * Matrix.CreateTranslation(new Vector3(25, 0, -75));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateTranslation(new Vector3(25, 0, -175));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / 6) * Matrix.CreateTranslation(new Vector3(25, 0, -275));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateTranslation(new Vector3(25, 0, -375));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / (-6)) * Matrix.CreateTranslation(new Vector3(25, 0, -475));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateTranslation(new Vector3(25, 0, -575));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / 6) * Matrix.CreateTranslation(new Vector3(25, 0, -675));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateTranslation(new Vector3(25, 0, -775));
Track.Add(Cube);

// RAMPA GRANDE
Ramp = new RampPrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
Ramp.World = Matrix.CreateScale(7f, 7f, 7f) * Matrix.CreateFromYawPitchRoll((float)Math.PI / 2, 0, 0) * Matrix.CreateTranslation(new Vector3(175, 125, -450));
Track.Add(Ramp);

// CUADRADOS GRANDES 
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Red);
Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(275, 25, -425));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Orange);
Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(300, 0, -400));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Yellow);
Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(325, -25, -375));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Green);
Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(350, -50, -350));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Blue);
Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(375, -75, -325));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Indigo);
Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(400, -100, -300));
Track.Add(Cube);
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Purple);
Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(425, -125, -275));
Track.Add(Cube);

// PLATAFORMA FINAL
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
Cube.World = Matrix.CreateScale(7f, 1f, 6f) * Matrix.CreateTranslation(new Vector3(425, -125, -75));
Track.Add(Cube);

// aca iria la banderita del final
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.White);
Cube.World = Matrix.CreateTranslation(new Vector3(425, -100, -75));
Track.Add(Cube);

// aca irian cartelitos
//jump
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Black);
Cube.World = Matrix.CreateTranslation(new Vector3(-25, 50, 150));
Track.Add(Cube);

//up (flechita?)
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Black);
Cube.World = Matrix.CreateTranslation(new Vector3(50, 75, 250));
Track.Add(Cube);

//jump
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Black);
Cube.World = Matrix.CreateTranslation(new Vector3(200, 150, 150));
Track.Add(Cube);

//jump
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Black);
Cube.World = Matrix.CreateTranslation(new Vector3(200, 150, -25));
Track.Add(Cube);

//down (flechita?)
Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Black);
Cube.World = Matrix.CreateTranslation(new Vector3(200, 175, -475));
Track.Add(Cube);
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.MainCharacter;

class Stage_01 : Stage
{

    public Stage_01(GraphicsDevice graphicsDevice, ContentManager content) : 
        base(graphicsDevice, content, characterPosition: new Vector3(25, 25, -800)) {}

    protected override void LoadTrack()
    {
        Track = new List<GeometricPrimitive>
        {
            // PRIMERA PLATAFORMA
            new CubePrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(25, 0, 0), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(25, 0, 25), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.LightPink, coordinates: new Vector3(25, 0, 50), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(25, 0, 75), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(25, 0, 100), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.LightPink, coordinates: new Vector3(25, 0, 125), scale: new Vector3(3f, 1f, 1f)),

            // SEGUNDA PLATAFORMA
            new CubePrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(25, 0, 175), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(25, 0, 200), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.LightPink, coordinates: new Vector3(25, 0, 225), scale: new Vector3(3f, 1f, 1f)),

            // ESCALERA
            new CubePrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(75, 25, 200), scale: new Vector3(1f, 1f, 3f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(100, 50, 200), scale: new Vector3(1f, 1f, 3f)),
            new CubePrimitive(GraphicsDevice, Content, Color.LightPink, coordinates: new Vector3(125, 75, 200), scale: new Vector3(1f, 1f, 3f)),

            // TERCERA PLATAFORMA
            new CubePrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(175, 100, 225), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(175, 100, 200), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.LightPink, coordinates: new Vector3(175, 100, 175), scale: new Vector3(3f, 1f, 1f)),

            //CUARTA PLATAFORMA
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(175, 100, 62), scale: new Vector3(1f, 1f, 6f)),

            // QUINTA PLATAFORMA
            new CubePrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(175, 100, -50), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(175, 100, -75), scale: new Vector3(3f, 1f, 1f)),
            new CubePrimitive(GraphicsDevice, Content, Color.LightPink, coordinates: new Vector3(175, 100, -100), scale: new Vector3(3f, 1f, 1f)),

            // RAMPA
            new RampPrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(175, 125, -125), scale: new Vector3(3f, 1f, 1f)),

            // CANALETA
            new CubePrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(175, 125, -250), scale: new Vector3(3f, 1f, 9f)),
            new RampPrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(150, 150, -250), scale: new Vector3(9f, 1f, 1f), rotation: Matrix.CreateFromYawPitchRoll((float)Math.PI / 2, 0, 0)),
            new RampPrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(200, 150, -250), scale: new Vector3(9f, 1f, 1f), rotation: Matrix.CreateFromYawPitchRoll((float)Math.PI / (-2), 0, 0)),

            // PLANOS INCLINADOS (ROLL)
            new CubePrimitive(GraphicsDevice, Content, Color.LightPink, coordinates: new Vector3(25, 0, -75), scale: new Vector3(3f, 1f, 4f), rotation: Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / (-6))),
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(25, 0, -175), scale: new Vector3(3f, 1f, 4f)),
            new CubePrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(25, 0, -275), scale: new Vector3(3f, 1f, 4f), rotation: Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / 6)),
            new CubePrimitive(GraphicsDevice, Content, Color.LightPink, coordinates: new Vector3(25, 0, -375), scale: new Vector3(3f, 1f, 4f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(25, 0, -475), scale: new Vector3(3f, 1f, 4f), rotation: Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / (-6))),
            new CubePrimitive(GraphicsDevice, Content, Color.BlueViolet, coordinates: new Vector3(25, 0, -575), scale: new Vector3(3f, 1f, 4f)),
            new CubePrimitive(GraphicsDevice, Content, Color.LightPink, coordinates: new Vector3(25, 0, -675), scale: new Vector3(3f, 1f, 4f), rotation: Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / 6)),
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(25, 0, -775), scale: new Vector3(3f, 1f, 4f)),

            // RAMPA GRANDE
            new RampPrimitive(GraphicsDevice, Content, Color.LightPink, coordinates: new Vector3(175, 125, -450), scale: new Vector3(7f, 7f, 7f), rotation: Matrix.CreateFromYawPitchRoll((float)Math.PI / 2, 0, 0)),

            // CUADRADOS GRANDES 
            new CubePrimitive(GraphicsDevice, Content, Color.Red, coordinates: new Vector3(275, 25, -425), scale: new Vector3(9f, 1f, 9f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Orange, coordinates: new Vector3(300, 0, -400), scale: new Vector3(9f, 1f, 9f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Yellow, coordinates: new Vector3(325, -25, -375), scale: new Vector3(9f, 1f, 9f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Green, coordinates: new Vector3(350, -50, -350), scale: new Vector3(9f, 1f, 9f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Blue, coordinates: new Vector3(375, -75, -325), scale: new Vector3(9f, 1f, 9f)),   
            new CubePrimitive(GraphicsDevice, Content, Color.Indigo, coordinates: new Vector3(400, -100, -300), scale: new Vector3(9f, 1f, 9f)),
            new CubePrimitive(GraphicsDevice, Content, Color.Purple, coordinates: new Vector3(425, -125, -275), scale: new Vector3(9f, 1f, 9f)),

            // PLATAFORMA FINAL
            new CubePrimitive(GraphicsDevice, Content, Color.Aquamarine, coordinates: new Vector3(425, -125, -75), scale: new Vector3(7f, 1f, 6f))
        };

    }

    protected override void LoadObstacles()
    {
        Obstacles = new List<GeometricPrimitive>();
    }

    protected override void LoadSigns()
    {
        Signs = new List<GeometricPrimitive>
        {
            // aca iria la banderita del final
            new CubePrimitive(GraphicsDevice, Content, Color.White, coordinates: new Vector3(425, -100, -75)),

            // aca irian cartelitos
            //jump
            new CubePrimitive(GraphicsDevice, Content, Color.Black, coordinates: new Vector3(-25, 50, 150)),

            //up (flechita?)
            new CubePrimitive(GraphicsDevice, Content, Color.Black, coordinates: new Vector3(50, 75, 250)),

            //jump
            new CubePrimitive(GraphicsDevice, Content, Color.Black, coordinates: new Vector3(200, 150, 150)),

            //jump
            new CubePrimitive(GraphicsDevice, Content, Color.Black, coordinates: new Vector3(200, 150, -25)),

            //down (flechita?)
            new CubePrimitive(GraphicsDevice, Content, Color.Black, coordinates: new Vector3(200, 175, -475))
        };
    }

    protected override void LoadPickups()
    {
        Pickups = new List<GeometricPrimitive>();
    }

    protected override void LoadCheckpoints()
    {
        Checkpoints = new List<GeometricPrimitive>();
    }

    public override void Update(GameTime gameTime)
    {
        // TODO: actualizar el estado de todas las piezas móviles del nivel

    }

}








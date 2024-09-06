using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Geometries;

namespace TGC.MonoGame.TP {
public class TGCGame : Game {
  public const string ContentFolder3D = "Models/";
  public const string ContentFolderEffects = "Effects/";
  public const string ContentFolderMusic = "Music/";
  public const string ContentFolderSounds = "Sounds/";
  public const string ContentFolderSpriteFonts = "SpriteFonts/";
  public const string ContentFolderTextures = "Textures/";

  public TGCGame() {
    Graphics = new GraphicsDeviceManager(this);

    Graphics.PreferredBackBufferWidth =
        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
    Graphics.PreferredBackBufferHeight =
        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;

    Content.RootDirectory = "Content";
    IsMouseVisible = true;
  }

    private GraphicsDeviceManager Graphics;
    private SpriteBatch SpriteBatch;
    private Effect Effect;
    private Matrix View;
    private Matrix Projection;

    private SpherePrimitive PlayerSphere;
    private Vector3 PlayerPosition = Vector3.Zero;
    private float PlayerSpeed = 4f;
    private Matrix PlayerWorld;
    private CubePrimitive Box; 
    private Elevator elevator; 
    private TeapotPrimitive teapot;
    private CylinderPrimitive cylinder;

    private TrianglePrimitive triangle;

    private int PlayerRadius = 1;
    private int FloorUnitHeight = 6;

    private Vector3 CameraPosition = new Vector3(0, 3, -15);

    private float Yaw { get; set; }
    private float Pitch { get; set; }
    private float Roll { get; set; }

  protected override void Initialize() {
    var rasterizerState = new RasterizerState();
    rasterizerState.CullMode = CullMode.None;
    GraphicsDevice.RasterizerState = rasterizerState;
    PlayerWorld = Matrix.Identity;
    View = Matrix.CreateLookAt(CameraPosition + PlayerPosition, PlayerPosition,
                               Vector3.Up);
    Projection = Matrix.CreatePerspectiveFieldOfView(
        MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

    base.Initialize();
  }

  protected override void LoadContent() {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        PlayerSphere = new SpherePrimitive(GraphicsDevice, PlayerRadius, 16);
        Box = new CubePrimitive(GraphicsDevice, 1, Color.Red);
        elevator = new Elevator(GraphicsDevice,-Vector3.UnitY,1,2,Color.Green,5);
        triangle = new TrianglePrimitive(GraphicsDevice,  new Vector3(-1f, 1f, 1f), new Vector3(0f, 2f, 1f), new Vector3(1f, 1f, 1f), Color.Black, Color.Cyan, Color.Magenta);
        teapot = new TeapotPrimitive(GraphicsDevice, 1);
        cylinder = new CylinderPrimitive(GraphicsDevice, 10, 7, 8);



     Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

    base.LoadContent();
  }

  protected override void Update(GameTime gameTime) {

    float dt = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
    var keyboardState = Keyboard.GetState();

    if (keyboardState.IsKeyDown(Keys.Escape))
      Exit();

    if (keyboardState.IsKeyDown(Keys.W))
      PlayerPosition.Z += PlayerSpeed * dt;

    if (keyboardState.IsKeyDown(Keys.S))
      PlayerPosition.Z -= PlayerSpeed * dt;

    if (keyboardState.IsKeyDown(Keys.D))
      PlayerPosition.X -= PlayerSpeed * dt;

    if (keyboardState.IsKeyDown(Keys.A))
      PlayerPosition.X += PlayerSpeed * dt;

       // Movimiento de la cámara con las flechas para facilidad de ver las cosas
    float cameraSpeed = 5f;  // Velocidad de la cámara
    if (keyboardState.IsKeyDown(Keys.Up))
        CameraPosition.Z += cameraSpeed * dt;  // Mover la cámara hacia adelante

    if (keyboardState.IsKeyDown(Keys.Down))
        CameraPosition.Z -= cameraSpeed * dt;  // Mover la cámara hacia atrás

    if (keyboardState.IsKeyDown(Keys.Left))
        CameraPosition.X -= cameraSpeed * dt;  // Mover la cámara hacia la izquierda

    if (keyboardState.IsKeyDown(Keys.Right))
        CameraPosition.X += cameraSpeed * dt;  // Mover la cámara hacia la derecha

    base.Update(gameTime);

    PlayerWorld = Matrix.CreateTranslation(PlayerPosition);

    elevator.Update(dt);
    View = Matrix.CreateLookAt(CameraPosition + PlayerPosition, PlayerPosition,
                               Vector3.Up);

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime) {
    GraphicsDevice.Clear(Color.Black);
    DrawGeometry(teapot, new Vector3(2, 0, 10), Yaw, -Pitch, Roll);

    Effect.Parameters["World"].SetValue(PlayerWorld);
    Effect.Parameters["View"].SetValue(View);
    Effect.Parameters["Projection"].SetValue(Projection);
    Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());
    PlayerSphere.Draw(Effect);

    Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
    Matrix initial_floor = Matrix.Identity;
    for (int i = 0; i < 10; i++) {
      Matrix floor_world =
          Matrix.CreateScale(FloorUnitHeight) *
          Matrix.CreateTranslation(new Vector3(
              0, -PlayerRadius - (FloorUnitHeight) / 2, i * FloorUnitHeight)) *
          initial_floor;
      Effect.Parameters["World"].SetValue(floor_world);
      Box.Draw(Effect);
    }

    elevator.Draw(Effect);
    var triangleEffect = triangle.Effect;
    triangleEffect.World = Matrix.Identity;
    triangleEffect.View = View;
    triangleEffect.Projection = Projection;
    triangleEffect.LightingEnabled = false;
    triangle.Draw(triangleEffect);

     var cylinderEffect = cylinder.Effect;

    Matrix rotation = Matrix.CreateRotationX(MathHelper.PiOver2); 

    cylinderEffect.World = rotation * Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll) * Matrix.CreateTranslation(new Vector3(0, 0, 30));
    cylinderEffect.View = View;
    cylinderEffect.Projection = Projection;

    cylinder.Draw(cylinderEffect);

  }

   private void DrawGeometry(GeometricPrimitive geometry, Vector3 position, float yaw, float pitch, float roll)
        {
            var effect = geometry.Effect;

            effect.World = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) * Matrix.CreateTranslation(position);
            effect.View = View;
            effect.Projection = Projection;

            geometry.Draw(effect);
        }

  protected override void UnloadContent() {
    Content.Unload();
    PlayerSphere.Dispose();
    base.UnloadContent();
  }
}
}

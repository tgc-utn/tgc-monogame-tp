using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

  private Tp.SpherePrimitive PlayerSphere;
  private Vector3 PlayerPosition = Vector3.Zero;
  private float PlayerSpeed = 4f;
  private Matrix PlayerWorld;

  private Tp.CubePrimitive Box; 
  private Tp.Elevator elevator; 

  private int PlayerRadius = 1;
  private int FloorUnitHeight = 6;

  private Vector3 CameraPosition = new Vector3(0, 3, -7);

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
    PlayerSphere = new Tp.SpherePrimitive(GraphicsDevice, PlayerRadius, 16);
    Box = new Tp.CubePrimitive(GraphicsDevice, 1, Color.Red);
    elevator = new Tp.Elevator(GraphicsDevice,-Vector3.UnitY,1,2,Color.Green,5);


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

    PlayerWorld = Matrix.CreateTranslation(PlayerPosition);

   elevator.Update(dt);
    View = Matrix.CreateLookAt(CameraPosition + PlayerPosition, PlayerPosition,
                               Vector3.Up);

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime) {
    GraphicsDevice.Clear(Color.Black);

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

  }

  protected override void UnloadContent() {
    Content.Unload();
    PlayerSphere.Dispose();
    base.UnloadContent();
  }
}
}

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

  private GraphicsDeviceManager Graphics { get; }
  private SpriteBatch SpriteBatch { get; set; }
  private Effect Effect { get; set; }
  private Matrix World { get; set; }
  private Matrix View { get; set; }
  private Matrix Projection { get; set; }

  private Tp.SpherePrimitive PlayerSphere { get; set; }
  private Vector3 PlayerPosition = Vector3.Zero;
  private float PlayerSpeed = 2f;

  protected override void Initialize() {
    var rasterizerState = new RasterizerState();
    rasterizerState.CullMode = CullMode.None;
    GraphicsDevice.RasterizerState = rasterizerState;
    World = Matrix.Identity;
    View = Matrix.CreateLookAt(Vector3.UnitZ * 3, Vector3.Zero, Vector3.Up);
    Projection = Matrix.CreatePerspectiveFieldOfView(
        MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

    base.Initialize();
  }

  protected override void LoadContent() {
    SpriteBatch = new SpriteBatch(GraphicsDevice);
    PlayerSphere = new Tp.SpherePrimitive(GraphicsDevice, 1, 16);

    Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

    base.LoadContent();
  }

  protected override void Update(GameTime gameTime) {

    float dt = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
    var keyboardState = Keyboard.GetState();

    if (keyboardState.IsKeyDown(Keys.Escape))
      Exit();

    if (keyboardState.IsKeyDown(Keys.W))
      PlayerPosition.Z -= PlayerSpeed * dt;

    if (keyboardState.IsKeyDown(Keys.S))
      PlayerPosition.Z += PlayerSpeed * dt;

    if (keyboardState.IsKeyDown(Keys.D))
      PlayerPosition.X += PlayerSpeed * dt;

    if (keyboardState.IsKeyDown(Keys.A))
      PlayerPosition.X -= PlayerSpeed * dt;

    World = Matrix.CreateTranslation(PlayerPosition);

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime) {
    GraphicsDevice.Clear(Color.Black);

    Effect.Parameters["World"].SetValue(World);
    Effect.Parameters["View"].SetValue(View);
    Effect.Parameters["Projection"].SetValue(Projection);
    Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());

    PlayerSphere.Draw(Effect);
  }

  protected override void UnloadContent() {
    Content.Unload();
    base.UnloadContent();
  }
}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using ThunderingTanks.Objects.Tanks;

namespace ThunderingTanks.Objects
{
    public class HUD
    {

        public const string ContentFolderModels = "Models/";
        public const string ContentFolderTextures = "Textures/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderFonts = "Fonts/";

        private float ScreenWidth { get; set; }
        private float ScreenHeight { get; set; }

        private Rectangle lifeBar;
        private Rectangle e_lifeBar;

        private Texture2D lifeBar_t;
        private Texture2D e_lifeBar_t;
        private Texture2D CrossHairTexture;


        public float Convergence { get; set; } = 1000f;
        private Vector2 CrossHairPosition;

        private SpriteFont FontArial;

        public int _maxLifeBarWidth;

        #region Debug
        public Vector3 TankPosition { get; set; }
        public int BulletCount { get; set; }
        public bool TankIsColliding { get; set; }
        public float FPS { get; set; }
        public float TimeSinceLastShot { get; set; }
        #endregion

        public HUD(float screenWidth, float screenHeight)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;

            float barWidth = screenWidth / 10;
            float barHeight = screenHeight / 20;
            float barY = 10;
            float barX = screenWidth - 2 * barWidth + 30;

            lifeBar = new Rectangle((int)barX, (int)barY, (int)barWidth, (int)barHeight);
            e_lifeBar = new Rectangle();
        }

        public void LoadContent(ContentManager Content)
        {
            CrossHairTexture = Content.Load<Texture2D>(ContentFolderTextures + "/punto-de-mira");
            lifeBar_t = Content.Load<Texture2D>(ContentFolderTextures + "HUD/lifebar");
            e_lifeBar_t = Content.Load<Texture2D>(ContentFolderTextures + "HUD/lifebar_empty");
            FontArial = Content.Load<SpriteFont>(ContentFolderFonts + "arial");

            _maxLifeBarWidth = lifeBar.Width;
        }

        public void Update(Tank Panzer, ref Viewport viewport)
        {
            CrossHairPosition = new Vector2(ScreenWidth / 2 - 25, (float)((Math.Tan(Panzer.GunElevation) * Convergence) + (ScreenHeight / 2 + 25)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                    CrossHairTexture,
                    CrossHairPosition,
                    null, Color.Black, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0.8f
              );
            spriteBatch.Draw(
                lifeBar_t,
                lifeBar,
                Color.Yellow
             );
            spriteBatch.Draw(
                e_lifeBar_t,
                e_lifeBar,
                Color.Yellow
             );

            #region Debug
            spriteBatch.DrawString(FontArial, "Debug", new Vector2(20, 20), Color.Red);
            spriteBatch.DrawString(FontArial, "Cantidad De Balas: " + BulletCount.ToString(), new Vector2(20, 40), Color.Red);
            spriteBatch.DrawString(FontArial, "Posicion Del Tanque: " + "X:" + TankPosition.X.ToString() + "Z:" + TankPosition.Z.ToString(), new Vector2(20, 60), Color.Red);
            spriteBatch.DrawString(FontArial, (TankIsColliding) ? "Coliciona" : "No Coliciona", new Vector2(20, 80), Color.Red);
            spriteBatch.DrawString(FontArial, "Reloading Time: " + TimeSinceLastShot, new Vector2(20, 100), Color.Red);
            spriteBatch.DrawString(FontArial, "Distancia De Apuntado " + Convergence, new Vector2(20, 120), Color.Red);
            spriteBatch.DrawString(FontArial, "FPS " + FPS, new Vector2(20, 140), Color.Red);
            #endregion

        }
    }
}

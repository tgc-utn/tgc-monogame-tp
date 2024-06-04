using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThunderingTanks.Cameras;

namespace ThunderingTanks.Objects
{
    public class HUD
    {
        private Rectangle lifeBar;
        private Rectangle e_lifeBar;

        private Texture2D lifeBar_t;
        private Texture2D e_lifeBar_t;
        private Texture2D CrossHairTexture;

        private Vector2 CrossHairPosition;
        private Vector3 CrossHairAux;

        public const string ContentFolderModels = "Models/";
        public const string ContentFolderTextures = "Textures/";
        public const string ContentFolderEffects = "Effects/";

        public int _maxLifeBarWidth;


        public HUD() {
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int barWidth = screenWidth / 10;
            int barHeight = screenHeight / 20;
            int barY = 10;
            int barX = screenWidth - 2 * barWidth + 30;
            lifeBar = new Rectangle(barX, barY, barWidth, barHeight);
            e_lifeBar = new Rectangle();
        }

        public void loadContent(ContentManager Content)
        {
            CrossHairTexture = Content.Load<Texture2D>(ContentFolderTextures + "/punto-de-mira");
            lifeBar_t = Content.Load<Texture2D>(ContentFolderTextures + "HUD/lifebar");
            e_lifeBar_t = Content.Load<Texture2D>(ContentFolderTextures + "HUD/lifebar_empty");
            _maxLifeBarWidth = lifeBar.Width;
        }

        public void Update(Tank Panzer, TargetCamera _targetCamera, ref Viewport viewport, float screenWidth)
        {
            CrossHairAux = viewport.Project(
                Panzer.Direction,
                _targetCamera.Projection,
                _targetCamera.View,
                Matrix.CreateRotationX(Panzer.GunElevation) * Panzer.TurretMatrix
                );

            CrossHairPosition = new Vector2(screenWidth / 2 - 25, CrossHairAux.Y);
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
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace TGC.MonoGame.TP
{
    class HUD
    {
        TGCGame Game;

        public HUD(TGCGame instance)
        {
            Game = instance;
        }
        public void Draw()
        {
            String topMessage;
            Point size = Game.GraphicsDevice.Viewport.Bounds.Size;
            Vector2 center = new Vector2(size.X / 2, size.Y / 2);
            Vector2 btnCenter = new Vector2(size.X / 2 - 180, size.Y / 2 - 90);
            int btnDelta = 150;
            Vector2 btnScale = new Vector2(0.6f,0.6f);
            switch (Game.GameState)
            {
                case TGCGame.GmState.StartScreen:
                    #region startScreen

                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.Draw(Game.BtnPlay, btnCenter - new Vector2(0, btnDelta), null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();

                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.Draw(Game.BtnOptions, btnCenter, null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();

                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.Draw(Game.BtnExit, btnCenter + new Vector2(0, btnDelta), null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();
                    #endregion
                    break;
                case TGCGame.GmState.Running:
                    #region running
                    //topleft
                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.Draw(Game.TopLeftBar, new Vector2(0, 20f), null, Color.White, 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();



                    topMessage = "FPS " + Game.FPS + " HP " + Game.Xwing.HP + " " + Game.GameState.ToString();

                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.DrawString(Game.SpriteFont, topMessage, new Vector2(80, 45), Color.White);
                    Game.SpriteBatch.End();

                   
                    var scale = 0.1f;
                    var sz = 512 * scale;

                    //energy
                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.Draw(Game.HudEnergy[Game.Xwing.Energy], new Vector2(size.X - 300f, 20f), null, Color.White, 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();

                    //Crosshair
                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.Draw(Game.Crosshairs[0], new Vector2(center.X - sz / 2, center.Y - sz / 2), null, Color.White, 0f, Vector2.Zero, new Vector2(scale, scale), SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();
                    #endregion
                    break;

                case TGCGame.GmState.Paused:
                    #region paused
                    //Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    //Game.SpriteBatch.Draw(Game.TopLeftBar, new Vector2(0, 20f), null, Color.White, 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
                    //Game.SpriteBatch.End();

                    topMessage = Game.Vector2ToStr(Game.MouseXY);
                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.DrawString(Game.SpriteFont, topMessage, new Vector2(80, 45), Color.White);
                    Game.SpriteBatch.End();

                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.Draw(Game.BtnContinue, btnCenter - new Vector2(0, btnDelta), null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();

                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.Draw(Game.BtnMenu, btnCenter, null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();

                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    Game.SpriteBatch.Draw(Game.BtnExit, btnCenter + new Vector2(0, btnDelta), null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();

                    #endregion
                    break;
                case TGCGame.GmState.Victory:
                    break;
                case TGCGame.GmState.Defeat:
                    break;
            }

            

        }
    }
}

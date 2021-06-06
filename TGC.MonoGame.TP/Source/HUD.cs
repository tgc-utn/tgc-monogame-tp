using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace TGC.MonoGame.TP
{
    public class HUD
    {
        public List<Button> startScreenBtns= new List<Button>();
        public List<Button> pauseBtns = new List<Button>();
        public List<Button> endBtns = new List<Button>();

        TGCGame Game;
        Point size;
        Vector2 center;
        Point btnCenter;
        int btnDelta;
        Vector2 btnScale;
        public HUD(TGCGame instance)
        {
            Game = instance;
           
        }
        public void Init()
        {
            size = Game.GraphicsDevice.Viewport.Bounds.Size;
            center = new Vector2(size.X / 2, size.Y / 2);
            btnCenter = new Point(size.X / 2 + 200, size.Y / 2 - 90);
            btnDelta = 150;
            btnScale = new Vector2(0.6f, 0.6f);

            startScreenBtns.Clear();
            pauseBtns.Clear();
            endBtns.Clear();

            startScreenBtns.Add(
                new Button(BtnType.Play, btnCenter - new Point(0, btnDelta), Game.BtnPlay, btnScale));
            startScreenBtns.Add(
                new Button(BtnType.Options, btnCenter, Game.BtnOptions, btnScale));
            startScreenBtns.Add(
                new Button(BtnType.Exit, btnCenter + new Point(0, btnDelta), Game.BtnExit, btnScale));
            
            pauseBtns.Add(
                new Button(BtnType.Continue, btnCenter - new Point(0, btnDelta), Game.BtnContinue, btnScale));
            pauseBtns.Add(
                new Button(BtnType.Menu, btnCenter, Game.BtnMenu, btnScale));
            pauseBtns.Add(
                new Button(BtnType.Exit, btnCenter + new Point(0, btnDelta), Game.BtnExit, btnScale));

            endBtns.Add(
                new Button(BtnType.Menu, btnCenter, Game.BtnMenu, btnScale));
            endBtns.Add(
                new Button(BtnType.Exit, btnCenter + new Point(0, btnDelta), Game.BtnExit, btnScale));

        }
        public void Draw()
        {
            String topMessage;
           
            switch (Game.GameState)
            {
                case TGCGame.GmState.StartScreen:
                    #region startScreen

                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    foreach(Button btn in startScreenBtns)
                        Game.SpriteBatch.Draw(btn.Image, btn.Position, null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();

                    
                    #endregion
                    break;
                case TGCGame.GmState.Running:
                    #region running
                    //topleft
                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    
                    Game.SpriteBatch.Draw(Game.HPBar[Game.Xwing.GetHPIndex()], new Vector2(0, 20f), null, Color.White, 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);

                    topMessage = "FPS " + Game.FPS + " HP " + Game.Xwing.HP;
                    Game.SpriteBatch.DrawString(Game.SpriteFont, topMessage, new Vector2(80, 45), Color.White);
                    
                   
                    var scale = 0.1f;
                    var sz = 512 * scale;

                    //energy
                    Game.SpriteBatch.Draw(Game.HudEnergy[Game.Xwing.Energy], new Vector2(size.X - 300f, 20f), null, Color.White, 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
                    
                    //Crosshair
                    Game.SpriteBatch.Draw(Game.Crosshairs[0], new Vector2(center.X - sz / 2, center.Y - sz / 2), null, Color.White, 0f, Vector2.Zero, new Vector2(scale, scale), SpriteEffects.None, 0f);
                    Game.SpriteBatch.End();
                    #endregion
                    break;

                case TGCGame.GmState.Paused:
                    #region paused

                    if (!Game.Camera.Resuming)
                    {
                        Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                        foreach (Button btn in pauseBtns)
                            Game.SpriteBatch.Draw(btn.Image, btn.Position, null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                        Game.SpriteBatch.End();
                    }
                    #endregion
                    break;
                case TGCGame.GmState.Victory:
                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    foreach (Button btn in startScreenBtns)
                        Game.SpriteBatch.Draw(btn.Image, btn.Position, null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);

                    Game.SpriteBatch.DrawString(Game.SpriteFont, "victoria", center - new Vector2(size.X / 3, 0f), new Color(255f, 50f, 50f));

                    Game.SpriteBatch.End();

                    break;
                case TGCGame.GmState.Defeat:
                    Game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    foreach (Button btn in startScreenBtns)
                        Game.SpriteBatch.Draw(btn.Image, btn.Position, null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                    Game.SpriteBatch.DrawString(Game.SpriteFont, "derrota", center - new Vector2(size.X / 3, 0f), new Color(255f, 50f, 50f));

                    Game.SpriteBatch.End();

                    break;
            }

        }
        public void VerifyBtnClick(MouseState mState)
        {
            if (!mState.LeftButton.Equals(ButtonState.Pressed))
                return;
            
            List<Button> clicked;
            Rectangle mouse = new Rectangle(mState.Position, new Point(5,5));
            switch (Game.GameState)
            {
                case TGCGame.GmState.StartScreen:
                    clicked = startScreenBtns.FindAll(btn => btn.Box.Intersects(mouse));
                    ChangeGameState(clicked);
                    break;
                
                case TGCGame.GmState.Paused:
                    clicked = pauseBtns.FindAll(btn => btn.Box.Intersects(mouse));
                    ChangeGameState(clicked);
                    
                    break;
                case TGCGame.GmState.Victory:
                    clicked = endBtns.FindAll(btn => btn.Box.Intersects(mouse));
                    ChangeGameState(clicked);

                    break;
                case TGCGame.GmState.Defeat:
                    clicked = endBtns.FindAll(btn => btn.Box.Intersects(mouse));
                    ChangeGameState(clicked);

                    break;
            }
        }
        void ChangeGameState(List<Button> clicked)
        {
            if (clicked.Count == 0)
                return;

            switch(clicked[0].Type)
            {
                case BtnType.Play:
                    Game.Camera.Reset();
                    SoundManager.StopMusic();
                    Game.GameState = TGCGame.GmState.Running;
                    Game.IsMouseVisible = false;
                    break;
                case BtnType.Continue:
                    Game.Camera.SoftReset();
                    Game.IsMouseVisible = false;
                    break;
                case BtnType.Menu:
                    Game.GameState = TGCGame.GmState.StartScreen;
                    break;
                case BtnType.Exit:
                    Game.Exit();
                    break;
            }
        }
    }
    
    public class Button
    {
        public BtnType Type;
        public Texture2D Image;
        public Vector2 Position;
        public Rectangle Box;
        public Button(BtnType t, Point Pos, Texture2D img, Vector2 scale)
        {
            Type = t;
            Position = Pos.ToVector2();
            Image = img;
            Box = new Rectangle(Pos, (new Vector2(470,188)*scale).ToPoint());
        }
    }
    public enum BtnType
    {
        Play,
        Continue,
        Pause,
        Menu,
        Options,
        Exit
    }
}

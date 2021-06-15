using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
namespace TGC.MonoGame.TP
{
    
    public class HUD
    {
        Texture2D[] Crosshairs;
        Texture2D[] HudEnergy;
        Texture2D[] HPBar;
        Texture2D[] mmStraight;
        Texture2D[] mmElbow;
        Texture2D[] mmT;
        Texture2D mmIntersection, mmPlatform;

        Texture2D BtnPlay, BtnContinue, BtnMenu, BtnExit, BtnOptions;
        Texture2D Xwing;

        public SpriteFont SpriteFont;
        public SpriteFont BigFont;
        
        public List<Button> startScreenBtns= new List<Button>();
        public List<Button> optionsBtns = new List<Button>();
        public List<Button> pauseBtns = new List<Button>();
        public List<Button> endBtns = new List<Button>();

        public bool ShowFullMap = false;
        SpriteBatch SpriteBatch;

        TGCGame Game;
        Point size;
        Vector2 center;
        Point btnCenter;
        int btnDelta;
        Vector2 btnScale;

        ContentManager Content;
        String ContentFolderTextures, ContentFolderSpriteFonts;
        Texture2D MiniMap;
        public HUD(TGCGame instance)
        {
            Game = instance;
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

        }
        Texture2D[] loadNumberedTextures(String source, int start, int end, int inc)
        {
            List<Texture2D> textures = new List<Texture2D>();
            for (int n = start; n <= end; n += inc)
                textures.Add(Content.Load<Texture2D>(ContentFolderTextures + source + n));

            return textures.ToArray();
        }
        public void LoadContent()
        {
            Content = Game.Content;
            ContentFolderTextures = TGCGame.ContentFolderTextures;
            ContentFolderSpriteFonts = TGCGame.ContentFolderSpriteFonts;

            Crosshairs = new Texture2D[] {  Content.Load<Texture2D>(ContentFolderTextures + "Crosshair/crosshair"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "Crosshair/crosshair-red"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "Crosshair/crosshairAlpha")};

            HudEnergy = loadNumberedTextures("HUD/Energy/", 0, 10, 1);
            HPBar = loadNumberedTextures("HUD/TopLeft/", 0, 100, 10);
            
            BtnPlay = Content.Load<Texture2D>(ContentFolderTextures + "HUD/Buttons/Jugar");
            BtnContinue = Content.Load<Texture2D>(ContentFolderTextures + "HUD/Buttons/Continuar");
            BtnMenu = Content.Load<Texture2D>(ContentFolderTextures + "HUD/Buttons/Menu");
            BtnExit = Content.Load<Texture2D>(ContentFolderTextures + "HUD/Buttons/Salir");
            BtnOptions = Content.Load<Texture2D>(ContentFolderTextures + "HUD/Buttons/Opciones");

            mmStraight = new Texture2D[] {  Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/straight"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/straight-90")};
            mmElbow = new Texture2D[] {     Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/elbow-0"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/elbow-90"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/elbow-180"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/elbow-270")};
            mmT = new Texture2D[] {         Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/T-0"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/T-90"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/T-180"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/T-270")};

            mmIntersection = Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/intersection");
            mmPlatform = Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/platform");
            Xwing = Content.Load<Texture2D>(ContentFolderTextures + "HUD/MiniMap/xwing");
            SpriteFont = Content.Load<SpriteFont>(ContentFolderSpriteFonts + "Starjedi");
            GenerateMiniMap();
            Init();
        }
        Texture2D getTextureFromBlock (Trench block)
        {
            switch(block.Type)
            {
                case TrenchType.Straight:
                    switch(block.Rotation)
                    {
                        case 0: return mmStraight[0];
                        case 90: return mmStraight[1];
                        case 180: return mmStraight[0];
                        case 270: return mmStraight[1];
                    }
                    break;
                case TrenchType.Elbow:
                    return mmElbow[(int)(block.Rotation / 90)]; 
                    
                case TrenchType.T:
                    return mmT[(int)(block.Rotation / 90)];
                case TrenchType.Intersection:
                    return mmIntersection;
                case TrenchType.Platform:
                    return mmPlatform;
            }
            return mmPlatform;
        }
        void GenerateMiniMap()
        {
            var mapSize = TGCGame.MapSize;
            RenderTarget2D MiniMapTarget = new RenderTarget2D(Game.GraphicsDevice, 50 * mapSize, 50 * mapSize);
            Game.Graphics.GraphicsDevice.SetRenderTarget(MiniMapTarget);
            SpriteBatch.Begin();
            var map = Game.Map;

            Vector2 pos;
            for(int x = 0; x < mapSize; x++)
            {
                pos.X = x * 50;
                for (int y = 0; y < mapSize; y++)
                {
                    pos.Y = y * 50;
                    Texture2D selected = getTextureFromBlock(map[x,y]);
                    SpriteBatch.Draw(selected, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }
            }
            
            SpriteBatch.End();

            Game.Graphics.GraphicsDevice.SetRenderTarget(null);

            MiniMap = (Texture2D) MiniMapTarget;
            //SpriteBatch.Draw(mmStraight[0], pos, null, Color.White, rotation, rotationCenter, 1f, SpriteEffects.None, 0);
        }
        Vector2 CalculateMiniMapXwingPos()
        {
            var mapSize = TGCGame.MapSize;
            var mapLimit = Game.MapLimit;
            var blockSize = mapLimit / mapSize;
            var xwing = Game.Xwing;

            var CurrentBlock = new Vector2(
                xwing.Position.X / blockSize + 0.5f, xwing.Position.Z / blockSize + 0.5f);

            CurrentBlock.X = MathHelper.Clamp(CurrentBlock.X, 0, mapSize - 1);
            CurrentBlock.Y = MathHelper.Clamp(CurrentBlock.Y, 0, mapSize - 1);
            Vector2 recCenter;
            recCenter.X = (int)MathHelper.Lerp(0, 50 * mapSize, CurrentBlock.X / mapSize);
            recCenter.Y = (int)MathHelper.Lerp(0, 50 * mapSize, CurrentBlock.Y / mapSize);

            return recCenter;
        }
        Rectangle CalculateMiniMapRec()
        {
            var recCenter = CalculateMiniMapXwingPos();

            //Debug.WriteLine(Game.Vector2ToStr(CurrentBlock) + " x " + recCenter.X + " y " + recCenter.Y);

            var viewSize = 200;
            var x = (int)(recCenter.X - viewSize / 2);
            var y = (int)(recCenter.Y - viewSize / 2);
            var len = viewSize;

            return new Rectangle(x, y, len, len);
            
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
            optionsBtns.Clear();


            startScreenBtns.Add(
                new Button(BtnType.Play, btnCenter - new Point(0, btnDelta), BtnPlay, btnScale));
            startScreenBtns.Add(
                new Button(BtnType.Options, btnCenter, BtnOptions, btnScale));
            startScreenBtns.Add(
                new Button(BtnType.Exit, btnCenter + new Point(0, btnDelta), BtnExit, btnScale));
            
            pauseBtns.Add(
                new Button(BtnType.Continue, btnCenter - new Point(0, btnDelta), BtnContinue, btnScale));
            pauseBtns.Add(
                new Button(BtnType.Menu, btnCenter, BtnMenu, btnScale));
            pauseBtns.Add(
                new Button(BtnType.Exit, btnCenter + new Point(0, btnDelta), BtnExit, btnScale));

            endBtns.Add(
                new Button(BtnType.Menu, btnCenter, BtnMenu, btnScale));
            endBtns.Add(
                new Button(BtnType.Exit, btnCenter + new Point(0, btnDelta), BtnExit, btnScale));

            // + volume sliders
            optionsBtns.Add(
                new Button(BtnType.Menu, btnCenter, BtnMenu, btnScale));
            optionsBtns.Add(
                new Button(BtnType.Exit, btnCenter + new Point(0, btnDelta), BtnExit, btnScale));


        }

        public void Draw()
        {
            String topMessage;


            //SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            //SpriteBatch.DrawString(Game.SpriteFont, "" + Game.GameState, new Vector2(size.X / 2 - 100f, 45), Color.White);
            //SpriteBatch.End();
            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            switch (Game.GameState)
            {
                case TGCGame.GmState.StartScreen:
                    #region startScreen

                    foreach (Button btn in startScreenBtns)
                        SpriteBatch.Draw(btn.Image, btn.Position, null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);

                    #endregion
                    break;
                case TGCGame.GmState.Options:

                    SpriteBatch.DrawString(SpriteFont, "proximamente", center - new Vector2(size.X / 3, 0f), new Color(255f, 50f, 50f));

                    break;
                case TGCGame.GmState.Running:
                    #region running
                    //topleft
                    
                    SpriteBatch.Draw(HPBar[Game.Xwing.GetHPIndex()], new Vector2(0, 20f), null, Color.White, 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);

                    //topMessage = "FPS " + Game.FPS + " HP " + Game.Xwing.HP;
                    topMessage = "E "+Game.elementsDrawn + "/" + Game.totalElements + " Pos "+Game.IntVector3ToStr(Game.Xwing.Position);
                    SpriteBatch.DrawString(SpriteFont, topMessage, new Vector2(80, 45), Color.White);

                    //energy
                    SpriteBatch.Draw(HudEnergy[Game.Xwing.Energy], new Vector2(size.X - 300f, 20f), null, Color.White, 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);

                    if (ShowFullMap)
                    {
                        var pos = new Vector2(size.X / 2, size.Y / 2);
                        var fullMapScale = 0.5f;
                        
                        SpriteBatch.Draw(MiniMap,
                            pos, null, Color.White, MathHelper.Pi, new Vector2(525, 525), fullMapScale, SpriteEffects.None, 0);

                        Vector2 xwingPos = CalculateMiniMapXwingPos();
                        xwingPos.X = (int)xwingPos.X * fullMapScale;
                        xwingPos.Y = (int)xwingPos.Y * fullMapScale;

                        var topleft = pos - new Vector2(262, 262);

                        var finalPos = topleft + new Vector2(525 - xwingPos.X, 525 - xwingPos.Y);
                        var rotation = MathHelper.ToRadians(Game.Xwing.Yaw) - MathHelper.PiOver2;

                        SpriteBatch.Draw(Xwing,
                            finalPos, null, Color.White, rotation, new Vector2(226, 226), 0.1f, SpriteEffects.None, 0);

                    }
                    else
                    {
                        var rotation = -MathHelper.ToRadians(Game.Xwing.Yaw) - MathHelper.PiOver2;


                        var pos = new Vector2(150, size.Y - 150);
                        var ViewRec = CalculateMiniMapRec();

                        var offset = new Vector2(ViewRec.Width * 0.5f, ViewRec.Height * 0.5f);
                        SpriteBatch.Draw(MiniMap,
                            pos, ViewRec, Color.White, rotation, offset, 1f, SpriteEffects.None, 0);
                        
                        var xwingpos = new Vector2(126, size.Y - 181);
                        SpriteBatch.Draw(Xwing,
                            xwingpos, null, Color.White, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0);

                        //Crosshair
                        if (Game.SelectedCamera.Equals(Game.Camera))
                        {
                            //var scale = 0.1f; 
                            //var sz = 512 * scale;
                            var scale = 0.2f;
                            var sz = 300 * scale;
                            SpriteBatch.Draw(Crosshairs[2], new Vector2(center.X - sz / 2, center.Y - sz / 2), null, Color.White, 0f, Vector2.Zero, new Vector2(scale, scale), SpriteEffects.None, 0f);
                        }
                    }
                    

                    
                    
                    #endregion
                    break;

                case TGCGame.GmState.Paused:
                    #region paused

                    if (!Game.Camera.Resuming)
                    {
                        foreach (Button btn in pauseBtns)
                            SpriteBatch.Draw(btn.Image, btn.Position, null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                    }
                    #endregion
                    break;
                case TGCGame.GmState.Victory:
                    foreach (Button btn in endBtns)
                        SpriteBatch.Draw(btn.Image, btn.Position, null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);

                    SpriteBatch.DrawString(SpriteFont, "victoria", center - new Vector2(size.X / 3, 0f), new Color(255f, 50f, 50f));

            
                    break;
                case TGCGame.GmState.Defeat:
                    foreach (Button btn in endBtns)
                        SpriteBatch.Draw(btn.Image, btn.Position, null, Color.White, 0f, Vector2.Zero, btnScale, SpriteEffects.None, 0f);
                    SpriteBatch.DrawString(SpriteFont, "derrota", center - new Vector2(size.X / 3, 0f), new Color(255f, 50f, 50f));


                    break;
            }
            SpriteBatch.End();

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
                    ProcessClick(clicked);
                    break;
                
                case TGCGame.GmState.Options:
                    clicked = optionsBtns.FindAll(btn => btn.Box.Intersects(mouse));
                    ProcessClick(clicked, mouse);
                    break;

                case TGCGame.GmState.Paused:
                    clicked = pauseBtns.FindAll(btn => btn.Box.Intersects(mouse));
                    ProcessClick(clicked);
                    
                    break;
                case TGCGame.GmState.Victory:
                    clicked = endBtns.FindAll(btn => btn.Box.Intersects(mouse));
                    ProcessClick(clicked);

                    break;
                case TGCGame.GmState.Defeat:
                    clicked = endBtns.FindAll(btn => btn.Box.Intersects(mouse));
                    ProcessClick(clicked);

                    break;
            }
        }
        void ProcessClick(List<Button> clicked)
        {
            ProcessClick(clicked, Rectangle.Empty);
        }
        void ProcessClick(List<Button> clicked, Rectangle mousePos)
        {
            if (clicked.Count == 0)
                return;

            var btn = clicked[0];
            if (btn.Type.Equals(BtnType.VolMaster) ||
                btn.Type.Equals(BtnType.VolFX) ||
                btn.Type.Equals(BtnType.VolMusic))
            {
                ProcessVolClick(btn, mousePos);
                return;
            }

            switch (btn.Type)
            {
                case BtnType.Play:
                    Game.ChangeGameStateTo(TGCGame.GmState.Running);
                    break;
                case BtnType.Continue:
                    Game.ChangeGameStateTo(TGCGame.GmState.Running);
                    break;
                case BtnType.Menu:
                    Game.ChangeGameStateTo(TGCGame.GmState.StartScreen);
                    break;
                case BtnType.Options:
                    Game.ChangeGameStateTo(TGCGame.GmState.Options);
                    break;
                case BtnType.Exit:
                    Game.Exit();
                    break;
            }
        }
        void ProcessVolClick(Button btn, Rectangle mouse)
        {
            //verify where the click was, update vol, ui element
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
        VolMaster,
        VolFX,
        VolMusic,
        Exit
    }
}

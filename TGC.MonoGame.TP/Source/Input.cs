using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
namespace TGC.MonoGame.TP
{
    public class Input
    {
        TGCGame Game;
        List<Keys> ignoredKeys = new List<Keys>();
        public Input(TGCGame instance)
        {
            Game = instance;
        }



        public void ProcessInput()
        {
            var kState = Keyboard.GetState();
            var mState = Mouse.GetState();

            Game.HUD.VerifyBtnClick(mState);
            switch (Game.GameState)
            {
                case TGCGame.GmState.StartScreen:
                    #region start
                    if (kState.IsKeyDown(Keys.Enter))
                    {
                        Game.GameState = TGCGame.GmState.Running;
                        Game.Camera.Reset();
                    }
                    #endregion
                    break;
                case TGCGame.GmState.Running:
                    #region runningInput

                    if (Game.Camera.MouseLookEnabled && Game.IsActive)
                        Game.Camera.ProcessMouse(Game.Xwing);
                    Game.Camera.ProcessKeyboard(Game.Xwing);

                    if (Game.Camera.MouseLookEnabled)
                    {
                        if (mState.LeftButton.Equals(ButtonState.Pressed))
                        {
                            Game.Xwing.fireLaser();
                        }
                    }
                    if (kState.IsKeyDown(Keys.Escape))
                    {
                        if (!ignoredKeys.Contains(Keys.Escape))
                        {
                            ignoredKeys.Add(Keys.Escape);
                            Game.GameState = TGCGame.GmState.Paused;
                            Game.Camera.SaveCurrentState();
                            Game.IsMouseVisible = true;
                        }
                    }
                    if (kState.IsKeyDown(Keys.F))
                    {
                        Game.Xwing.fireLaser();
                    }
                    if (kState.IsKeyDown(Keys.V))
                    {
                        if (!ignoredKeys.Contains(Keys.V))
                        {
                            ignoredKeys.Add(Keys.V);
                            Game.IsFixedTimeStep = !Game.IsFixedTimeStep;
                        }
                    }
                    if (kState.IsKeyDown(Keys.F11))
                    {
                        if (!ignoredKeys.Contains(Keys.F11))
                        {
                            ignoredKeys.Add(Keys.F11);
                            if (Game.Graphics.IsFullScreen) //720 windowed
                            {
                                Game.Graphics.IsFullScreen = false;
                                Game.Graphics.PreferredBackBufferWidth = 1280;
                                Game.Graphics.PreferredBackBufferHeight = 720;
                            }
                            else //1080 fullscreen
                            {
                                Game.Graphics.IsFullScreen = true;
                                Game.Graphics.PreferredBackBufferWidth = 1920;
                                Game.Graphics.PreferredBackBufferHeight = 1080;
                            }
                            Game.Graphics.ApplyChanges();
                            Game.HUD.Init();
                        }
                    }

                    if (kState.IsKeyDown(Keys.M))
                    {
                        //evito que se cambie constantemente manteniendo apretada la tecla
                        if (!ignoredKeys.Contains(Keys.M))
                        {
                            ignoredKeys.Add(Keys.M);
                            if (!Game.Camera.MouseLookEnabled && Game.Camera.ArrowsLookEnabled)
                            {
                                Game.Camera.MouseLookEnabled = true;
                                //correccion inicial para que no salte a un punto cualquiera
                                Game.Camera.pastMousePosition = Mouse.GetState().Position.ToVector2();
                                Game.IsMouseVisible = false;
                            }
                            else if (Game.Camera.MouseLookEnabled && Game.Camera.ArrowsLookEnabled)
                            {
                                Game.Camera.ArrowsLookEnabled = false;
                            }
                            else if (Game.Camera.MouseLookEnabled && !Game.Camera.ArrowsLookEnabled)
                            {
                                Game.Camera.MouseLookEnabled = false;
                                Game.Camera.ArrowsLookEnabled = true;
                                Game.IsMouseVisible = true;
                            }
                        }
                    }
                    if (kState.IsKeyDown(Keys.R))
                    {
                        if (!ignoredKeys.Contains(Keys.R))
                        {
                            ignoredKeys.Add(Keys.R);
                            float roll = Game.Xwing.Roll;
                            Game.Xwing.clockwise = roll < 0;
                            Game.Xwing.PrevRoll = roll;
                            Game.Xwing.startRolling = true;
                            Game.Xwing.barrelRolling = true;
                            
                        }
                    }
                   
                    //remuevo de la lista aquellas teclas que solte

                    #endregion
                    break;

                case TGCGame.GmState.Paused:
                    if (kState.IsKeyDown(Keys.Escape))
                    {
                        if (!ignoredKeys.Contains(Keys.Escape))
                        {
                            ignoredKeys.Add(Keys.Escape);
                            Game.GameState = TGCGame.GmState.Running;
                            Game.Camera.SoftReset();
                            Game.IsMouseVisible = false;
                        }
                    }
                    
                    break;
                case TGCGame.GmState.Victory:
                    break;
                case TGCGame.GmState.Defeat:
                    break;
            }
            ignoredKeys.RemoveAll(kState.IsKeyUp);

        }
    }
}

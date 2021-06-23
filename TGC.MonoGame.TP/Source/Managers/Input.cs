﻿using System;
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


        //float deltaX = 0f;
        //float deltaZ = 0f;
        bool selectedFirst = true;
        public void ProcessInput()
        {
            var kState = Keyboard.GetState();
            var mState = Mouse.GetState();

            Game.HUD.VerifyBtnClick(mState);

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
            if (kState.IsKeyDown(Keys.V))
            {
                if (!ignoredKeys.Contains(Keys.V))
                {
                    ignoredKeys.Add(Keys.V);
                    Game.IsFixedTimeStep = !Game.IsFixedTimeStep;
                }
            }
            if (kState.IsKeyDown(Keys.G))
            {
                if (!ignoredKeys.Contains(Keys.G))
                {
                    ignoredKeys.Add(Keys.G);
                    Game.ShowGizmos= !Game.ShowGizmos;
                }
            }
            if (kState.IsKeyDown(Keys.P))
            {
                if (!ignoredKeys.Contains(Keys.P))
                {
                    ignoredKeys.Add(Keys.P);
                    Game.ApplyBloom = !Game.ApplyBloom;
                    //Game.ApplyShadowMap = !Game.ApplyShadowMap;

                    //Game.Drawer.saveToFile = true;
                }
            }
            //Game.ShowShadowMap = kState.IsKeyDown(Keys.O);
            Game.Drawer.ShowTarget1 = kState.IsKeyDown(Keys.D1);
            Game.Drawer.ShowTarget2 = kState.IsKeyDown(Keys.D2);
            Game.Drawer.ShowTarget3 = kState.IsKeyDown(Keys.D3);
            Game.Drawer.ShowTarget4 = kState.IsKeyDown(Keys.D4);


            switch (Game.GameState)
            {
                case TGCGame.GmState.StartScreen:
                    #region start
                    if (kState.IsKeyDown(Keys.Enter))
                    {
                        Game.ChangeGameStateTo(TGCGame.GmState.Running);
                        //Game.GameState = TGCGame.GmState.Running;
                        //Game.Camera.Reset();
                        //SoundManager.StopMusic();
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

                    Game.SelectedCamera = kState.IsKeyDown(Keys.B) ? Game.LookBack : Game.Camera;

                    Game.HUD.ShowFullMap = kState.IsKeyDown(Keys.CapsLock);
                    //if (kState.IsKeyDown(Keys.B))
                    //    Game.SelectedCamera = Game.LookBack;
                    //else
                    //    Game.SelectedCamera = Game.Camera;
                    if (kState.IsKeyDown(Keys.Escape))
                    {
                        if (!ignoredKeys.Contains(Keys.Escape))
                        {
                            ignoredKeys.Add(Keys.Escape);

                            Game.ChangeGameStateTo(TGCGame.GmState.Paused);

                            //Game.GameState = TGCGame.GmState.Paused;
                            //Game.Camera.SaveCurrentState();
                            ////Game.Camera.= MathHelper.ToRadians(Game.Camera.Yaw) + MathHelper.Pi;
                            //Game.IsMouseVisible = true;
                        }
                    }
                    if (kState.IsKeyDown(Keys.F))
                    {
                        Game.Xwing.fireLaser();
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
                    //parameter debug (i.e. moving models)
                    #region parameterDebug
                    float inputDelta;
                    
                    var deltaX = 0f;
                    var deltaY = 0f;
                    var deltaZ = 0f;
                    var update = false;
                    if (kState.IsKeyDown(Keys.N))
                        inputDelta = 0.02f;
                    else
                        inputDelta = 0.01f;
                    if (kState.IsKeyDown(Keys.I))
                    {
                        deltaZ = inputDelta;
                        update = true;
                    }
                    if (kState.IsKeyDown(Keys.K))
                    {
                        deltaZ = -inputDelta;
                        update = true;
                    }
                    if (kState.IsKeyDown(Keys.J))
                    {
                        deltaX = inputDelta;
                        update = true;
                    }
                    if (kState.IsKeyDown(Keys.L))
                    {
                        deltaX = -inputDelta;
                        update = true;
                    }
                    if (kState.IsKeyDown(Keys.Y))
                    {
                        deltaY = inputDelta;
                        update = true;
                    }
                    if (kState.IsKeyDown(Keys.H))
                    {
                        deltaY = -inputDelta;
                        update = true;
                    }


                    //if (kState.IsKeyDown(Keys.U))
                    //{
                    //    if(!ignoredKeys.Contains(Keys.U))
                    //    {
                    //        ignoredKeys.Add(Keys.U);
                    //        selectedFirst = !selectedFirst;
                    //        Debug.WriteLine("SelectedFirst " + selectedFirst);
                    //    }
                    //}
                    if (update)
                    {
                        Game.Drawer.zMul += deltaX;
                        Game.Drawer.wMul += deltaY;
                        Game.Drawer.depthMul += deltaZ;

                        Debug.WriteLine("zMul " + Game.Drawer.zMul + "wMul " + Game.Drawer.wMul + " DM " + Game.Drawer.depthMul);
                    }
                    #endregion
                    #endregion
                    break;

                case TGCGame.GmState.Paused:
                    if (kState.IsKeyDown(Keys.Escape))
                    {
                        if (!ignoredKeys.Contains(Keys.Escape))
                        {
                            ignoredKeys.Add(Keys.Escape);
                            Game.ChangeGameStateTo(TGCGame.GmState.Running);
                            ////Game.GameState = TGCGame.GmState.Running;
                            //Game.Camera.SoftReset();
                            //Game.IsMouseVisible = false;
                        }
                    }
                    
                    break;
                case TGCGame.GmState.Victory:
                    if (kState.IsKeyDown(Keys.Escape))
                        Game.Exit();
                    
                        break;
                case TGCGame.GmState.Defeat:
                    if (kState.IsKeyDown(Keys.Escape))
                        Game.Exit();

                    break;
            }
            ignoredKeys.RemoveAll(kState.IsKeyUp);

        }
    }
}
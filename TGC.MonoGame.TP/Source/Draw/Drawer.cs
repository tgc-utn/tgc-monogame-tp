using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP
{
    public class Drawer
    {
        TGCGame Game;
        GraphicsDevice GraphicsDevice;
        ContentManager Content;
        String ContentFolder3D;
        String ContentFolderEffects;
        String ContentFolderTextures;
        //TGCGame.GmState GameState;
        #region models effects textures targets
        public Model TieModel;
        public Model XwingModel;
        private Model XwingEnginesModel;

        public static Model TrenchPlatform;
        public static Model TrenchStraight;
        public static Model TrenchT;
        public static Model TrenchIntersection;
        public static Model TrenchElbow;
        public static Model TrenchTurret;
        private Model Trench2;
        SkyBox SkyBox;
        Model skyboxModel;
        public Model LaserModel;
        private Effect EffectTexture;
        private Effect Effect;
        public Effect EffectLight;
        public Effect EffectBloom;
        public Effect EffectBlur;
        public Effect MasterEffect;
        public Effect MasterMRT;

        private Texture TieTexture;
        private Texture TrenchTexture;
        private Texture[] XwingTextures;
        
        private FullScreenQuad FullScreenQuad;
        private RenderTarget2D MainSceneRenderTarget;
        private RenderTarget2D BlurHRenderTarget;
        private RenderTarget2D BlurVRenderTarget;
        private RenderTarget2D ShadowMapRenderTarget;

        private RenderTarget2D ColorTarget;
        private RenderTarget2D NormalTarget;
        private RenderTarget2D DepthTarget;
        private RenderTarget2D BloomFilterTarget;

        #endregion

        int BloomPassCount = 2;
        int ShadowMapSize = 4000;

        public bool saveToFile = false;
        public bool modelTechnique = true;
        public bool ShowBloomFilter = false;
        public bool ShowShadowMap = true;
        public float wMul = 0f;

        public float zMul = 1f;
        public float depthMul = 1f;

        public List<Trench> trenchesToDraw = new List<Trench>();
        public List<TieFighter> tiesToDraw = new List<TieFighter>();
        public List<Laser> lasersToDraw = new List<Laser>();
        public List<Ship> shipsToDraw = new List<Ship>();
        public bool showXwing;

        SpriteBatch SpriteBatch;
        enum DrawType
        {
            Regular,
            BloomFilter,
            DepthMap,
            Shadowed,
            ShadowedBloom
        }
        public Drawer()
        {
           
        }
        
        public void Init()
        {
            Game = TGCGame.Instance;
            GraphicsDevice = Game.GraphicsDevice;
            Content = Game.Content;

            ContentFolder3D = TGCGame.ContentFolder3D;
            ContentFolderEffects = TGCGame.ContentFolderEffects;
            ContentFolderTextures = TGCGame.ContentFolderTextures;

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            LoadContent();
        }
        void LoadContent()
        {
            TieModel = Content.Load<Model>(ContentFolder3D + "TIE/TIE");
            XwingModel = Content.Load<Model>(ContentFolder3D + "XWing/model");
            XwingEnginesModel = Content.Load<Model>(ContentFolder3D + "XWing/xwing-engines");

            TrenchPlatform = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Platform-Block");
            TrenchStraight = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Straight-Block");
            TrenchT = Content.Load<Model>(ContentFolder3D + "Trench/Trench-T-Block");
            TrenchElbow = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Elbow-Block");
            TrenchIntersection = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Intersection");
            TrenchTurret = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Turret");
            Trench2 = Content.Load<Model>(ContentFolder3D + "Trench2/Trench");
            LaserModel = Content.Load<Model>(ContentFolder3D + "Laser/Laser");

            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            EffectTexture = Content.Load<Effect>(ContentFolderEffects + "BasicTexture");
            EffectLight = Content.Load<Effect>(ContentFolderEffects + "BlinnPhong");
            EffectBloom = Content.Load<Effect>(ContentFolderEffects + "Bloom");
            EffectBlur = Content.Load<Effect>(ContentFolderEffects + "GaussianBlur");


            MasterEffect = Content.Load<Effect>(ContentFolderEffects + "MasterEffect");
            MasterMRT = Content.Load<Effect>(ContentFolderEffects + "MasterMRT");
            XwingTextures = new Texture[] { Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert6_Base_Color"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert5_Base_Color") };

            TieTexture = Content.Load<Texture2D>(ContentFolderTextures + "TIE/TIE_IN_Diff");

            TrenchTexture = Content.Load<Texture2D>(ContentFolderTextures + "Trench/MetalSurface");


            assignEffectToModels(new Model[] { XwingModel, TieModel }, EffectTexture);
            assignEffectToModels(new Model[] { TrenchPlatform, TrenchStraight, TrenchElbow, TrenchT, TrenchIntersection, TrenchTurret }, EffectLight);
            assignEffectToModels(new Model[] { Trench2, LaserModel, XwingEnginesModel }, Effect);

            assignEffectToModels(new Model[] { LaserModel, XwingEnginesModel }, MasterEffect);

            manageEffectParameters();

            skyboxModel = Content.Load<Model>(ContentFolder3D + "skybox/cube");
            //boxModel = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Turret");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skybox/space_earth_small_skybox");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");
            SkyBox = new SkyBox(skyboxModel, skyBoxTexture, skyBoxEffect);

            var width = GraphicsDevice.Viewport.Width;
            var height = GraphicsDevice.Viewport.Height;

            FullScreenQuad = new FullScreenQuad(GraphicsDevice);
            MainSceneRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
               GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
               RenderTargetUsage.DiscardContents);


            BlurHRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);
            BlurVRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);


            //ShadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, ShadowMapSize, ShadowMapSize, false,
            //    SurfaceFormat.Single, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);

            ColorTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            NormalTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            //try
            //{
            //    ShadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, width * 5, height * 5, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            //    Debug.WriteLine("5x shadows");
            //}
            //catch
            //{
                ShadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, width * 2, height * 2, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
                Debug.WriteLine("2x shadows");
            //}
            BloomFilterTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            RenderTargetBinding[0] = ColorTarget;
            RenderTargetBinding[1] = NormalTarget;
            //RenderTargetBinding[2] = ShadowMapRenderTarget;
            RenderTargetBinding[2] = BloomFilterTarget;

        }
        RenderTargetBinding[] RenderTargetBinding = new RenderTargetBinding[3];
        public void Unload()
        {
            FullScreenQuad.Dispose();
            BlurHRenderTarget.Dispose();
            MainSceneRenderTarget.Dispose();
            BlurVRenderTarget.Dispose();
        }


        void DrawScene(DrawType drawType)
        {
            var CameraView = Game.SelectedCamera.View;
            var CameraProjection = Game.SelectedCamera.Projection;
            var CameraPosition = Game.SelectedCamera.Position;

            EPbasicView.SetValue(CameraView);
            EPtextureView.SetValue(CameraView);
            EPbasicProjection.SetValue(CameraProjection);
            EPtextureProjection.SetValue(CameraProjection);


            if (drawType == DrawType.Regular || drawType == DrawType.Shadowed || drawType == DrawType.ShadowedBloom)
                SkyBox.Draw(CameraView, CameraProjection, CameraPosition);
            
            if (Game.GameState.Equals(TGCGame.GmState.Running) ||
                Game.GameState.Equals(TGCGame.GmState.Paused) ||
                Game.GameState.Equals(TGCGame.GmState.Defeat))
            {
                if(showXwing)
                    DrawXWing(Game.Xwing, drawType);
                foreach (var enemy in tiesToDraw)
                    DrawTie(enemy, drawType);
                foreach (var ship in shipsToDraw)
                {
                    if (ship.Allied)
                        DrawXwing(ship, drawType);
                    else
                        DrawTie(ship, drawType);
                }
                   

                if (drawType != DrawType.DepthMap)
                    foreach (var laser in lasersToDraw)
                        DrawModel(LaserModel, laser.SRT, laser.Color, drawType);
                DrawMap(drawType);

                if (Game.ShowGizmos)
                {
                    Matrix SRT;
                    foreach (var laser in Laser.AlliedLasers)
                    {
                        var OBB = laser.BoundingBox;
                        SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                        Game.Gizmos.DrawCube(SRT, Color.White);
                    }
                    foreach (var laser in Laser.EnemyLasers)
                    {
                        var OBB = laser.BoundingBox;
                        SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                        Game.Gizmos.DrawCube(SRT, Color.White);
                    }
                }
            }
        }

        #region drawers
        #region nonMRT
        public void DrawRegular()
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            DrawScene(DrawType.Regular);
        }
        
        #endregion

        public bool ShowTarget1;
        public bool ShowTarget2;
        public bool ShowTarget3;
        public bool ShowTarget4;

        void DrawSceneMRT(bool shadowMapping)
        {
            if (!shadowMapping)
            {
                var CameraView = Game.SelectedCamera.View;
                var CameraProjection = Game.SelectedCamera.Projection;
                var CameraPosition = Game.SelectedCamera.Position;

                //SkyBox.Draw(CameraView, CameraProjection, CameraPosition);
                SkyBox.Effect = MasterMRT;
                MasterMRT.CurrentTechnique = ETMRTskybox;
                SkyBox.Draw(CameraView, CameraProjection, CameraPosition);
            }
            else
                MasterMRT.CurrentTechnique = ETMRTshadowmap;

            if (Game.GameState.Equals(TGCGame.GmState.Running) ||
                Game.GameState.Equals(TGCGame.GmState.Paused) ||
                Game.GameState.Equals(TGCGame.GmState.Defeat))
            {
                
                
                if (showXwing)
                    DrawXWingMRT(Game.Xwing, shadowMapping);
                foreach (var enemy in tiesToDraw)
                    DrawTieMRT(enemy);
                foreach (var ship in shipsToDraw)
                {
                    if (ship.Allied)
                        DrawXwingMRT(ship);
                    else
                        DrawTieMRT(ship);
                }
                if (!shadowMapping)
                {
                    MasterMRT.CurrentTechnique = ETMRTbasicColor;
                    EPMRTaddToBloomFilter.SetValue(1f);
                    foreach (var laser in lasersToDraw)
                        DrawModelMRT(LaserModel, laser.SRT, laser.Color);

                    EPMRTaddToBloomFilter.SetValue(0f);
                    EPMRTcolor.SetValue(new Vector3(0.5f, 0.5f, 0.5f));
                }
                DrawMapMRT();
            }
            
            
        }
        public void DrawMRT()
        {
            assignEffectToModels(new Model[] { TieModel, XwingModel, XwingEnginesModel, TrenchElbow, 
                TrenchIntersection, TrenchPlatform, TrenchStraight, TrenchT, TrenchTurret, LaserModel }, MasterMRT);

            GraphicsDevice.SetRenderTarget(ShadowMapRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            DrawSceneMRT(true);



            GraphicsDevice.SetRenderTargets(RenderTargetBinding);

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            
            DrawSceneMRT(false);


            if (!ShowTarget1)
            {

                GraphicsDevice.SetRenderTarget(null);

                SpriteBatch.Begin();


                if (ShowTarget2)
                    SpriteBatch.Draw(NormalTarget, Vector2.Zero, Color.White);
                else if (ShowTarget3)
                    SpriteBatch.Draw(ShadowMapRenderTarget,
                           Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                else if (ShowTarget4)
                    SpriteBatch.Draw(BloomFilterTarget, Vector2.Zero, Color.White);
                else
                    SpriteBatch.Draw(ColorTarget, Vector2.Zero, Color.White);

                SpriteBatch.End();
            }
            else
            {

                EffectBlur.CurrentTechnique = EffectBlur.Techniques["MRTtech"];
                EPblurScreenSize.SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
                //EffectBlur.Parameters["screenSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
                var bloomTexture = BloomFilterTarget;
                

                GraphicsDevice.SetRenderTargets(BlurHRenderTarget, BlurVRenderTarget);
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

                EPblurTexture.SetValue(bloomTexture);

                FullScreenQuad.Draw(EffectBlur);

                
                GraphicsDevice.DepthStencilState = DepthStencilState.None;
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(Color.Black);

                EffectBloom.CurrentTechnique = EffectBloom.Techniques["Integrate"];
                EPbloomTexture.SetValue(ColorTarget);
                EPbloomBlurHTexture.SetValue(BlurHRenderTarget);
                EPbloomBlurVTexture.SetValue(BlurVRenderTarget);

                FullScreenQuad.Draw(EffectBloom);
            }

            //DrawScene(DrawType.Regular);

        }
        public void DrawShadowed()
        {
            Stream stream;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            // Set the render target as our shadow map, we are drawing the depth into this texture

            if (ShowShadowMap)
                GraphicsDevice.SetRenderTarget(null);
            else
                GraphicsDevice.SetRenderTarget(ShadowMapRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

            MasterEffect.CurrentTechnique = MasterEffect.Techniques["DepthPass"];
            MasterEffect.Parameters["zMul"].SetValue(zMul);
            MasterEffect.Parameters["wMul"].SetValue(wMul);
            MasterEffect.Parameters["depthMul"].SetValue(depthMul);

            DrawScene(DrawType.DepthMap);

            if (ShowShadowMap)
                return;

            //if (saveToFile)
            //{
            //    stream = File.OpenWrite("shadowmap.png");
            //    ShadowMapRenderTarget.SaveAsPng(stream, ShadowMapSize, ShadowMapSize);
            //    stream.Dispose();
            //}

            saveToFile = false;

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);

            MasterEffect.CurrentTechnique = ETmasterShadowed;
            EPmasterShadowMap.SetValue(ShadowMapRenderTarget);
            EPmasterLightPosition.SetValue(Game.LightCamera.Position);
            EPmasterLightViewProjection.SetValue(Game.LightCamera.View * Game.LightCamera.Projection);
            EPmasterShadowMapSize.SetValue(new Vector2(ShadowMapSize, ShadowMapSize));

            DrawScene(DrawType.Shadowed);
        }

        #region regular elements
        void DrawMap(DrawType drawType)
        {
            foreach (var t in trenchesToDraw)
                DrawTrench(t, drawType);
        }
        void DrawTrench(Trench t, DrawType drawType)
        {
            EPlightKD.SetValue(0.8f);
            EPlightKS.SetValue(0.4f);
            EPlightSh.SetValue(3f);

            if (drawType == DrawType.BloomFilter || drawType == DrawType.DepthMap)
            {
                DrawModel(t.Model, t.SRT, Vector3.Zero, drawType);
                foreach (var turret in t.Turrets)
                    DrawModel(TrenchTurret, turret.SRT, Vector3.Zero, drawType);
            }
            else if (drawType == DrawType.Shadowed)
            {
                assignEffectToModel(t.Model, MasterEffect);
                assignEffectToModel(TrenchTurret, MasterEffect);
                MasterEffect.CurrentTechnique = ETmasterShadowed;
                Matrix world;
                var meshCount = 0;

                foreach (var mesh in t.Model.Meshes)
                {
                    world = mesh.ParentBone.Transform * t.SRT;

                    EPmasterWorld.SetValue(world);
                    EPlightInverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                    EPmasterWorldViewProjection.SetValue(world * Game.SelectedCamera.View * Game.SelectedCamera.Projection);
                    mesh.Draw();

                    meshCount++;
                }
                foreach (var turret in t.Turrets)
                {
                    foreach (var mesh in TrenchTurret.Meshes)
                    {
                        world = mesh.ParentBone.Transform * turret.SRT;

                        EPmasterWorld.SetValue(world);
                        EPlightInverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                        EPmasterWorldViewProjection.SetValue(world * Game.SelectedCamera.View * Game.SelectedCamera.Projection);


                        mesh.Draw();
                    }
                    if (Game.ShowGizmos)
                    {
                        var BB = turret.BoundingBox;
                        Game.Gizmos.DrawCube(BoundingVolumesExtensions.GetCenter(BB), BoundingVolumesExtensions.GetExtents(BB) * 2f, Color.Magenta);

                    }
                }


            }
            else if (drawType == DrawType.Regular)
            {
                
                EPlightTexture.SetValue(TrenchTexture);
                //EffectLight.Parameters["baseTexture"].SetValue(TrenchTexture);
                //EffectBloom.Parameters["baseTexture"].SetValue(TrenchTexture);

                Matrix world;
                var meshCount = 0;

                foreach (var mesh in t.Model.Meshes)
                {

                    world = mesh.ParentBone.Transform * t.SRT;

                    EPlightWorld.SetValue(world);
                    EPlightInverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));

                    //EffectLight.Parameters["World"].SetValue(world);
                    //EffectLight.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(world)));

                    var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                    //EffectLight.Parameters["WorldViewProjection"].SetValue(wvp);
                    //EffectBloom.Parameters["WorldViewProjection"].SetValue(wvp); 
                    EPlightWorldViewProjection.SetValue(wvp);
                    //EPbloomWorldViewProjection.SetValue(wvp);

                    mesh.Draw();

                    meshCount++;
                }
                foreach (var turret in t.Turrets)
                {
                    foreach (var mesh in TrenchTurret.Meshes)
                    {
                        world = mesh.ParentBone.Transform * turret.SRT;

                        //EffectLight.Parameters["World"].SetValue(world);
                        //EffectLight.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(world)));
                        EPlightWorld.SetValue(world);
                        EPlightWorldViewProjection.SetValue(Matrix.Transpose(Matrix.Invert(world)));

                        var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                        //EffectLight.Parameters["WorldViewProjection"].SetValue(wvp);
                        //EffectBloom.Parameters["WorldViewProjection"].SetValue(wvp);
                        EPlightWorldViewProjection.SetValue(wvp);
                        //EPbloomWorldViewProjection.SetValue(wvp);
                        mesh.Draw();
                    }
                    if (Game.ShowGizmos)
                    {
                        var BB = turret.BoundingBox;
                        Game.Gizmos.DrawCube(BoundingVolumesExtensions.GetCenter(BB), BoundingVolumesExtensions.GetExtents(BB) * 2f, Color.Magenta);

                    }
                }

                if (Game.ShowGizmos)
                {
                    var index = 0;
                    Color[] colors = { Color.White, Color.Yellow, Color.Blue, Color.Magenta };
                    foreach (var BB in t.boundingBoxes)
                    {
                        var color = colors[index];

                        if (t.IsCurrent)
                            color = Color.Cyan;

                        Game.Gizmos.DrawCube(BoundingVolumesExtensions.GetCenter(BB), BoundingVolumesExtensions.GetExtents(BB) * 2f, Game.Xwing.OBB.Intersects(BB) ? Color.Red : color);
                        index++;
                    }
                    //Matrix SRT;
                    //foreach (var OBB in t.boundingBoxes)
                    //{
                    //    var color = colors[index];
                    //    SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    //    Gizmos.DrawCube(SRT, Xwing.OBB.Intersects(OBB) ? Color.Red : color);
                    //    index++;
                    //}

                }
            }
        }
  
        void DrawXwing(Ship xwing, DrawType drawType)
        {
            int meshCount = 0;

            EPlightKD.SetValue(1f);
            EPlightKS.SetValue(1f);
            EPlightSh.SetValue(20f);

            if (drawType == DrawType.BloomFilter || drawType == DrawType.DepthMap)
            {
                DrawModel(XwingModel, xwing.SRT, Vector3.Zero, drawType);
            }
            else if (drawType == DrawType.Regular || drawType == DrawType.Shadowed)
            {
                assignEffectToModel(XwingModel, EffectLight);

                foreach (var mesh in XwingModel.Meshes)
                {
                    var world = mesh.ParentBone.Transform * xwing.SRT;

                    var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                    var itw = Matrix.Transpose(Matrix.Invert(world));

                    EPlightWorld.SetValue(world);
                    EPlightInverseTransposeWorld.SetValue(itw);
                    EPlightTexture.SetValue(XwingTextures[meshCount]);
                    EPlightWorldViewProjection.SetValue(wvp);

                    //EPmasterWorld.SetValue(xwing.World);
                    //EPmasterWorldViewProjection.SetValue(wvp);
                    //EPmasterInverseTransposeWorld.SetValue(itw);

                    meshCount++;

                    mesh.Draw();
                }
            }
        }
        void DrawXWing(Xwing xwing, DrawType drawType)
        {
            int meshCount = 0; //Como el xwing tiene 2 texturas, tengo que dibujarlo de esta manera

            //efecto para verificar colisiones, se pone rojo

            EPlightKD.SetValue(1f);
            EPlightKS.SetValue(1f);
            EPlightSh.SetValue(20f);


            DrawModel(XwingEnginesModel, xwing.EnginesSRT, xwing.EnginesColor, drawType);

            if (drawType == DrawType.BloomFilter || drawType == DrawType.DepthMap)
            {
                DrawModel(XwingModel, xwing.SRT, Vector3.Zero, drawType);
            }
            //else if (drawType == DrawType.Shadowed)
            //{

            //}
            else if (drawType == DrawType.Regular || drawType == DrawType.Shadowed)
            {
                assignEffectToModel(XwingModel, EffectLight);

                foreach (var mesh in XwingModel.Meshes)
                {
                    xwing.World = mesh.ParentBone.Transform * xwing.SRT;

                    var wvp = xwing.World * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                    var itw = Matrix.Transpose(Matrix.Invert(xwing.World));

                    EPlightWorld.SetValue(xwing.World);
                    EPlightInverseTransposeWorld.SetValue(itw);
                    EPlightTexture.SetValue(XwingTextures[meshCount]);
                    EPlightWorldViewProjection.SetValue(wvp);

                    //EPmasterWorld.SetValue(xwing.World);
                    //EPmasterWorldViewProjection.SetValue(wvp);
                    //EPmasterInverseTransposeWorld.SetValue(itw);

                    meshCount++;

                    mesh.Draw();
                }
                if (Game.ShowGizmos)
                {
                    
                    var OBB = xwing.OBB;
                    Matrix SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Game.Gizmos.DrawCube(SRT, xwing.hit ? Color.Red : Color.White);
                    
                    OBB = xwing.OBBL;
                    SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Game.Gizmos.DrawCube(SRT, xwing.hit ? Color.Red : Color.Yellow);

                    OBB = xwing.OBBR;
                    SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Game.Gizmos.DrawCube(SRT, xwing.hit ? Color.Red : Color.Cyan);

                    OBB = xwing.OBBU;
                    SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Game.Gizmos.DrawCube(SRT, xwing.hit ? Color.Red : Color.Magenta);

                    OBB = xwing.OBBD;
                    SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Game.Gizmos.DrawCube(SRT, xwing.hit ? Color.Red : Color.Green);
                
                }
            }

        }
        void DrawTie(Ship tie, DrawType drawType)
        {
            EPlightKD.SetValue(0.2f);
            EPlightKS.SetValue(0.2f);
            EPlightSh.SetValue(0.5f);
            if (drawType == DrawType.BloomFilter || drawType == DrawType.DepthMap)
            {
                DrawModel(TieModel, tie.SRT, Vector3.Zero, drawType);
            }
            else if (drawType == DrawType.Regular || drawType == DrawType.Shadowed)
            {
                assignEffectToModel(TieModel, EffectLight);
                Matrix world;
                foreach (var mesh in TieModel.Meshes)
                {
                    world = mesh.ParentBone.Transform * tie.SRT;

                    var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                    EPlightWorld.SetValue(world);
                    EPlightTexture.SetValue(TieTexture);
                    EPlightWorldViewProjection.SetValue(wvp);
                    EPlightInverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));

                    mesh.Draw();
                }
                
            }
        }
        void DrawTie(TieFighter tie, DrawType drawType)
        {
            EPlightKD.SetValue(0.2f);
            EPlightKS.SetValue(0.2f);
            EPlightSh.SetValue(0.5f);

            if (drawType == DrawType.BloomFilter || drawType == DrawType.DepthMap)
            {
                DrawModel(TieModel, tie.SRT, Vector3.Zero, drawType);
            }
            //else if ()
            //{

            //}
            else if (drawType == DrawType.Regular || drawType == DrawType.Shadowed)
            {
                assignEffectToModel(TieModel, EffectLight);
                Matrix world;
                foreach (var mesh in TieModel.Meshes)
                {
                    world = mesh.ParentBone.Transform * tie.SRT;

                    //EPtextureWorld.SetValue(world);
                    //EPtextureBaseTexture.SetValue(TieTexture);
                    //EPtextureColor.SetValue(Vector3.Zero);

                    var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                    EPlightWorld.SetValue(world);
                    EPlightTexture.SetValue(TieTexture);
                    EPlightWorldViewProjection.SetValue(wvp);
                    EPlightInverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                    //EffectTexture.Parameters["World"].SetValue(world);
                    //EffectTexture.Parameters["ModelTexture"].SetValue(TieTexture);
                    //EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.One);
                    mesh.Draw();
                }
                if (Game.ShowGizmos)
                {
                    Game.Gizmos.DrawSphere(tie.boundingSphere.Center, tie.boundingSphere.Radius * Vector3.One, Color.White);
                }
            }

        }
        void DrawModel(Model model, Matrix SRT, Vector3 color, DrawType drawType)
        {
            assignEffectToModel(model, MasterEffect);

            if (drawType == DrawType.Regular || drawType == DrawType.BloomFilter || drawType == DrawType.Shadowed)
                MasterEffect.CurrentTechnique = ETmasterBasicColor;
            if (drawType == DrawType.DepthMap)
                MasterEffect.CurrentTechnique = ETmasterDepth;

            //EPbasicColor.SetValue(color);
            EPmasterColor.SetValue(color);
            foreach (var mesh in model.Meshes)
            {
                var world = mesh.ParentBone.Transform * SRT;
                Matrix wvp = Matrix.Identity;
                if (drawType == DrawType.Regular || drawType == DrawType.Shadowed || drawType == DrawType.BloomFilter)
                    wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                else if (drawType == DrawType.DepthMap)
                    wvp = world * Game.LightCamera.View * Game.LightCamera.Projection;

                //Effect.Parameters["World"].SetValue(world);
                //EPbasicWorld.SetValue(world);
                EPmasterWorldViewProjection.SetValue(wvp);
                //EffectBloom.Parameters["WorldViewProjection"].SetValue(wvp);
                mesh.Draw();
            }
            
        }
        #endregion
        #region MRTelements
        void DrawMapMRT()
        {
            foreach (var t in trenchesToDraw)
                DrawTrenchMRT(t);
        }
        void DrawTrenchMRT(Trench t)
        {
            
            Matrix world;
            
            foreach (var mesh in t.Model.Meshes)
            {
                world = mesh.ParentBone.Transform * t.SRT;
                EPMRTcolor.SetValue(new Vector3(0.5f, 0.5f, 0.5f));
                EPMRTworld.SetValue(world);
                EPMRTworldViewProjection.SetValue(world * Game.SelectedCamera.View * Game.SelectedCamera.Projection);
                EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                mesh.Draw();

            }
            foreach (var turret in t.Turrets)
            {
                foreach (var mesh in TrenchTurret.Meshes)
                {
                    world = mesh.ParentBone.Transform * turret.SRT;
                    EPMRTcolor.SetValue(new Vector3(0.5f, 0.5f, 0.5f));
                    EPMRTworld.SetValue(world);
                    EPMRTworldViewProjection.SetValue(world * Game.SelectedCamera.View * Game.SelectedCamera.Projection);
                    EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                    mesh.Draw();
                }
            }

        }
        
        void DrawXwingMRT(Ship xwing)
        {
            Matrix world;
            int meshCount = 0;
            foreach (var mesh in XwingModel.Meshes)
            {
                world = mesh.ParentBone.Transform * xwing.SRT;

                var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                //var itw = Matrix.Transpose(Matrix.Invert(xwing.World));

                EPMRTtexture.SetValue(XwingTextures[meshCount]);
                EPMRTworld.SetValue(world);
                EPMRTworldViewProjection.SetValue(wvp);
                EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                meshCount++;

                mesh.Draw();
            }
        }

        void DrawXWingMRT(Xwing xwing, bool shadowMapping)
        {
            int meshCount = 0;
            if (!shadowMapping)
                MasterMRT.CurrentTechnique = ETMRTbasicColor;
            
            EPMRTaddToBloomFilter.SetValue(1f);
            DrawModelMRT(XwingEnginesModel, xwing.EnginesSRT, xwing.EnginesColor);
            EPMRTaddToBloomFilter.SetValue(0f);
            
            if (!shadowMapping)
                MasterMRT.CurrentTechnique = ETMRTtextured;

            foreach (var mesh in XwingModel.Meshes)
            {
                xwing.World = mesh.ParentBone.Transform * xwing.SRT;

                var wvp = xwing.World * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                //var itw = Matrix.Transpose(Matrix.Invert(xwing.World));

                EPMRTtexture.SetValue(XwingTextures[meshCount]);
                EPMRTworld.SetValue(xwing.World);
                EPMRTworldViewProjection.SetValue(wvp);
                EPMRTlightViewProjection.SetValue(xwing.World * Game.LightCamera.View * Game.LightCamera.Projection);
                meshCount++;

                mesh.Draw();
            }
        }

        void DrawTieMRT(Ship tie)
        {
            Matrix world;
            foreach (var mesh in TieModel.Meshes)
            {
                world = mesh.ParentBone.Transform * tie.SRT;

                var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                EPMRTworld.SetValue(world);
                EPMRTtexture.SetValue(TieTexture);
                EPMRTworldViewProjection.SetValue(wvp);
                EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                mesh.Draw();
            }
        }
        void DrawTieMRT(TieFighter tie)
        {
            Matrix world;
            foreach (var mesh in TieModel.Meshes)
            {
                world = mesh.ParentBone.Transform * tie.SRT;

                var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                EPMRTworld.SetValue(world);
                EPMRTtexture.SetValue(TieTexture);
                EPMRTworldViewProjection.SetValue(wvp);
                EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                //EPlightInverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));
                mesh.Draw();
            }
        }
        void DrawModelMRT(Model model, Matrix SRT, Vector3 color)
        {
            
            foreach (var mesh in model.Meshes)
            {
                var world = mesh.ParentBone.Transform * SRT;
                var wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;

                EPMRTworld.SetValue(world);
                EPMRTcolor.SetValue(color);
                //MasterMRT.Parameters["World"].SetValue(world);
                EPMRTlightViewProjection.SetValue(world * Game.LightCamera.View * Game.LightCamera.Projection);
                EPMRTworldViewProjection.SetValue(wvp);
                mesh.Draw();
            }

        }

        #endregion
        #endregion

        void assignEffectToModel(Model model, Effect effect)
        {
            foreach (var mesh in model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
        }
        void assignEffectToModels(Model[] models, Effect effect)
        {
            foreach (Model model in models)
                assignEffectToModel(model, effect);
        }

        #region effectParameters
        EffectParameter EPlightWorldViewProjection;
        EffectParameter EPlightView;
        EffectParameter EPlightProjection;
        EffectParameter EPlightWorld;
        EffectParameter EPlightInverseTransposeWorld;
        EffectParameter EPlightLightPosition;
        EffectParameter EPlightEyePosition;
        EffectParameter EPlightTexture;
        EffectParameter EPlightKD;
        EffectParameter EPlightKS;
        EffectParameter EPlightSh;

        EffectParameter EPbloomWorldViewProjection;
        EffectParameter EPbloomTexture;
        EffectParameter EPbloomBlurHTexture;
        EffectParameter EPbloomBlurVTexture;

        EffectParameter EPblurScreenSize;
        EffectParameter EPblurTexture;

        EffectParameter EPbasicView;
        EffectParameter EPbasicProjection;
        EffectParameter EPbasicWorld;
        EffectParameter EPbasicColor;

        EffectParameter EPtextureView;
        EffectParameter EPtextureProjection;
        EffectParameter EPtextureWorld;
        EffectParameter EPtextureColor;
        EffectParameter EPtextureBaseTexture;

        EffectParameter EPmasterColor;
        EffectParameter EPmasterTexture;
        EffectParameter EPmasterShadowMap;
        EffectParameter EPmasterLightPosition;
        EffectParameter EPmasterShadowMapSize;
        EffectParameter EPmasterLightViewProjection;
        EffectParameter EPmasterWorld;
        EffectParameter EPmasterWorldViewProjection;
        EffectParameter EPmasterInverseTransposeWorld;

        EffectParameter EPMRTworld;
        EffectParameter EPMRTworldViewProjection;
        EffectParameter EPMRTaddToBloomFilter;
        EffectParameter EPMRTcolor;
        EffectParameter EPMRTtexture;
        EffectParameter EPMRTlightViewProjection;

        EffectTechnique ETmasterBasicColor;
        EffectTechnique ETmasterDepth;
        EffectTechnique ETmasterShadowed;

        EffectTechnique ETMRTtextured;
        EffectTechnique ETMRTbasicColor;
        EffectTechnique ETMRTskybox;
        EffectTechnique ETMRTshadowmap;
        void manageEffectParameters()
        {
            EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.Zero);
            EffectTexture.Parameters["TextureMultiplier"].SetValue(1f);


            EffectLight.Parameters["ambientColor"].SetValue(new Vector3(1f, 1f, 1f));
            EffectLight.Parameters["diffuseColor"].SetValue(new Vector3(1f, 1f, 1f));
            EffectLight.Parameters["specularColor"].SetValue(new Vector3(1f, 1f, 1f));

            EffectLight.Parameters["KAmbient"].SetValue(0.4f);
            EffectLight.Parameters["KDiffuse"].SetValue(0.8f);
            EffectLight.Parameters["KSpecular"].SetValue(0.4f);
            EffectLight.Parameters["shininess"].SetValue(3f);

            EPlightKD = EffectLight.Parameters["KDiffuse"];
            EPlightKS = EffectLight.Parameters["KSpecular"];
            EPlightSh = EffectLight.Parameters["shininess"];

            
            EPlightWorldViewProjection = EffectLight.Parameters["WorldViewProjection"];
            EPlightWorld = EffectLight.Parameters["World"];
            EPlightInverseTransposeWorld = EffectLight.Parameters["InverseTransposeWorld"];
            EPlightLightPosition = EffectLight.Parameters["lightPosition"];
            EPlightEyePosition = EffectLight.Parameters["eyePosition"];
            EPlightTexture = EffectLight.Parameters["baseTexture"];

            EPbloomWorldViewProjection = EffectBloom.Parameters["WorldViewProjection"];
            EPbloomTexture = EffectBloom.Parameters["baseTexture"];
            EPbloomBlurHTexture = EffectBloom.Parameters["blurHTexture"];
            EPbloomBlurVTexture = EffectBloom.Parameters["blurVTexture"];

            EPblurTexture = EffectBlur.Parameters["baseTexture"];
            EPblurScreenSize = EffectBlur.Parameters["screenSize"];
            EPbasicView = Effect.Parameters["View"];
            EPbasicProjection = Effect.Parameters["Projection"];
            EPbasicWorld = Effect.Parameters["World"];
            EPbasicColor = Effect.Parameters["DiffuseColor"];

            EPtextureView = EffectTexture.Parameters["View"];
            EPtextureProjection = EffectTexture.Parameters["Projection"];
            EPtextureWorld = EffectTexture.Parameters["World"];
            EPtextureColor = EffectTexture.Parameters["ModifierColor"];
            EPtextureBaseTexture = EffectTexture.Parameters["ModelTexture"];

            EPmasterColor = MasterEffect.Parameters["Color"];
            EPmasterTexture = MasterEffect.Parameters["baseTexture"];
            EPmasterShadowMap = MasterEffect.Parameters["shadowMap"];
            EPmasterLightPosition = MasterEffect.Parameters["lightPosition"];
            EPmasterShadowMapSize = MasterEffect.Parameters["shadowMapSize"];
            EPmasterLightViewProjection = MasterEffect.Parameters["LightViewProjection"];
            EPmasterWorld = MasterEffect.Parameters["World"];
            EPmasterWorldViewProjection = MasterEffect.Parameters["WorldViewProjection"];
            EPmasterInverseTransposeWorld = MasterEffect.Parameters["InverseTransposeWorld"];

            EPMRTworld = MasterMRT.Parameters["World"];
            EPMRTworldViewProjection = MasterMRT.Parameters["WorldViewProjection"];
            EPMRTtexture = MasterMRT.Parameters["Texture"];
            EPMRTaddToBloomFilter = MasterMRT.Parameters["AddToFilter"];
            EPMRTcolor = MasterMRT.Parameters["Color"];
            EPMRTlightViewProjection = MasterMRT.Parameters["LightViewProjection"];

            ETMRTbasicColor = MasterMRT.Techniques["BasicColorDraw"];
            ETMRTtextured = MasterMRT.Techniques["TexturedDraw"];
            ETMRTskybox = MasterMRT.Techniques["Skybox"];
            ETMRTshadowmap = MasterMRT.Techniques["DepthPass"];

            ETmasterDepth = MasterEffect.Techniques["DepthPass"];
            ETmasterShadowed = MasterEffect.Techniques["DrawShadowedPCF"];
            ETmasterBasicColor = MasterEffect.Techniques["BasicColor"];
            
        }
        #endregion
    }
}
using System;
using System.IO;
using System.Collections.Generic;
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
        private Model Tie;
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

        private Texture TieTexture;
        private Texture TrenchTexture;

        
        private RenderTarget2D FirstPassBloomRenderTarget;
        private FullScreenQuad FullScreenQuad;
        private RenderTarget2D MainSceneRenderTarget;
        private RenderTarget2D SecondPassBloomRenderTarget;
        private RenderTarget2D ShadowMapRenderTarget;
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
        public List<Xwing> xwingsToDraw = new List<Xwing>();
        public List<Laser> lasersToDraw = new List<Laser>();


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

            LoadContent();
        }
        void LoadContent()
        {
            Tie = Content.Load<Model>(ContentFolder3D + "TIE/TIE");
            Game.Xwing.Model = Content.Load<Model>(ContentFolder3D + "XWing/model");
            Game.Xwing.EnginesModel = Content.Load<Model>(ContentFolder3D + "XWing/xwing-engines");

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

            Game.Xwing.Textures = new Texture[] { Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert6_Base_Color"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert5_Base_Color") };

            TieTexture = Content.Load<Texture2D>(ContentFolderTextures + "TIE/TIE_IN_Diff");

            TrenchTexture = Content.Load<Texture2D>(ContentFolderTextures + "Trench/MetalSurface");


            assignEffectToModels(new Model[] { Game.Xwing.Model, Tie }, EffectTexture);
            assignEffectToModels(new Model[] { TrenchPlatform, TrenchStraight, TrenchElbow, TrenchT, TrenchIntersection, TrenchTurret }, EffectLight);
            assignEffectToModels(new Model[] { Trench2, LaserModel, Game.Xwing.EnginesModel }, Effect);

            assignEffectToModels(new Model[] { LaserModel, Game.Xwing.EnginesModel }, MasterEffect);

            manageEffectParameters();

            skyboxModel = Content.Load<Model>(ContentFolder3D + "skybox/cube");
            //boxModel = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Turret");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skybox/space_earth_small_skybox");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");
            SkyBox = new SkyBox(skyboxModel, skyBoxTexture, skyBoxEffect);


            FullScreenQuad = new FullScreenQuad(GraphicsDevice);
            MainSceneRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
               GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
               RenderTargetUsage.DiscardContents);
            FirstPassBloomRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
                RenderTargetUsage.DiscardContents);
            SecondPassBloomRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);


            ShadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, ShadowMapSize, ShadowMapSize, false,
                SurfaceFormat.Single, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);

        }
        public void Unload()
        {
            FullScreenQuad.Dispose();
            FirstPassBloomRenderTarget.Dispose();
            MainSceneRenderTarget.Dispose();
            SecondPassBloomRenderTarget.Dispose();
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
                foreach (var xwing in xwingsToDraw)
                    DrawXWing(xwing, drawType);
                foreach (var enemy in tiesToDraw)
                    DrawTie(enemy, drawType);
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
        public void DrawRegular()
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            DrawScene(DrawType.Regular);
        }
        public void DrawBloom()
        {
            Stream stream;
            GraphicsDevice.SetRenderTarget(MainSceneRenderTarget);

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            DrawScene(DrawType.Regular);

           
            // Set the render target as our bloomRenderTarget, we are drawing the bloom color into this texture
            if (!ShowBloomFilter)
            {
                GraphicsDevice.SetRenderTarget(FirstPassBloomRenderTarget);
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
            }
            else
            {
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
            }

            //Debug.WriteLine("drawing bloom filter");
            DrawScene(DrawType.BloomFilter);

            if (ShowBloomFilter)
                return;


            if (saveToFile)
            {
                stream = File.OpenWrite("bloomFilter.png");
                FirstPassBloomRenderTarget.SaveAsPng(stream, 1280, 720);
                stream.Dispose();
            }
            
            
            EffectBlur.CurrentTechnique = EffectBlur.Techniques["Blur"];
            EPblurScreenSize.SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            //EffectBlur.Parameters["screenSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            var bloomTexture = FirstPassBloomRenderTarget;
            var finalBloomRenderTarget = SecondPassBloomRenderTarget;

            for (var index = 0; index < BloomPassCount; index++)
            {
                // Set the render target as null, we are drawing into the screen now!
                GraphicsDevice.SetRenderTarget(finalBloomRenderTarget);
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

                //EffectBlur.Parameters["baseTexture"].SetValue(bloomTexture);
                EPblurTexture.SetValue(bloomTexture);

                FullScreenQuad.Draw(EffectBlur);

                if (index != BloomPassCount - 1)
                {
                    var auxiliar = bloomTexture;
                    bloomTexture = finalBloomRenderTarget;
                    finalBloomRenderTarget = auxiliar;
                }
            }
            if (saveToFile)
            {
                stream = File.OpenWrite("blurred.png");
                finalBloomRenderTarget.SaveAsPng(stream, 1280, 720);
                stream.Dispose();
            }
            saveToFile = false;
            
            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            EffectBloom.CurrentTechnique = EffectBloom.Techniques["Integrate"];
            EPbloomTexture.SetValue(MainSceneRenderTarget);
            EPbloomFilteredTexture.SetValue(finalBloomRenderTarget);
            //EffectBloom.Parameters["baseTexture"].SetValue(MainSceneRenderTarget);
            //EffectBloom.Parameters["bloomTexture"].SetValue(finalBloomRenderTarget);
            FullScreenQuad.Draw(EffectBloom);
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

                //EffectBloom.Parameters["ApplyBloom"].SetValue(0f);
                //EPbloomTexture.SetValue(TrenchTexture);
                //EffectBloom.CurrentTechnique = EffectBloom.Techniques["BloomPass"];
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
                assignEffectToModel(t.Model, EffectLight);
                assignEffectToModel(TrenchTurret, EffectLight);

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
                    EPbloomWorldViewProjection.SetValue(wvp);

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
                        EPbloomWorldViewProjection.SetValue(wvp);
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

        void DrawXWing(Xwing xwing, DrawType drawType)
        {
            int meshCount = 0; //Como el xwing tiene 2 texturas, tengo que dibujarlo de esta manera

            //efecto para verificar colisiones, se pone rojo

            EPlightKD.SetValue(1f);
            EPlightKS.SetValue(1f);
            EPlightSh.SetValue(20f);


            DrawModel(xwing.EnginesModel, xwing.EnginesSRT, xwing.EnginesColor, drawType);

            if (drawType == DrawType.BloomFilter || drawType == DrawType.DepthMap)
            {
                DrawModel(xwing.Model, xwing.SRT, Vector3.Zero, drawType);
            }
            //else if (drawType == DrawType.Shadowed)
            //{

            //}
            else if (drawType == DrawType.Regular || drawType == DrawType.Shadowed)
            {
                assignEffectToModel(xwing.Model, EffectLight);

                foreach (var mesh in xwing.Model.Meshes)
                {
                    xwing.World = mesh.ParentBone.Transform * xwing.SRT;

                    //EPtextureWorld.SetValue(Xwing.World);
                    //EPtextureBaseTexture.SetValue(Xwing.Textures[meshCount]);
                    var wvp = xwing.World * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                    var itw = Matrix.Transpose(Matrix.Invert(xwing.World));

                    EPlightWorld.SetValue(xwing.World);
                    EPlightInverseTransposeWorld.SetValue(itw);
                    EPlightTexture.SetValue(xwing.Textures[meshCount]);
                    EPlightWorldViewProjection.SetValue(wvp);

                    //EPmasterWorld.SetValue(xwing.World);
                    //EPmasterWorldViewProjection.SetValue(wvp);
                    //EPmasterInverseTransposeWorld.SetValue(itw);

                    //EffectTexture.Parameters["World"].SetValue(Xwing.World);
                    //EffectTexture.Parameters["ModelTexture"].SetValue(Xwing.Textures[meshCount]);
                    meshCount++;

                    mesh.Draw();
                }
                if (Game.ShowGizmos)
                {
                    //var BB = xwing.BB;
                    //Gizmos.DrawSphere(Xwing.boundingSphere.Center, Xwing.boundingSphere.Radius * Vector3.One, Color.White);

                    //Gizmos.DrawCube(BoundingVolumesExtensions.GetCenter(BB), 
                    //BoundingVolumesExtensions.GetExtents(BB) * 2f, Xwing.hit ? Color.Red : Color.White);


                    var OBB = xwing.OBB;
                    Matrix SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Game.Gizmos.DrawCube(SRT, xwing.hit ? Color.Red : Color.White);
                    /*
                    OBB = Xwing.OBBL;
                    SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Gizmos.DrawCube(SRT, Xwing.hit ? Color.Red : Color.Yellow);

                    OBB = Xwing.OBBR;
                    SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Gizmos.DrawCube(SRT, Xwing.hit ? Color.Red : Color.Cyan);

                    OBB = Xwing.OBBU;
                    SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Gizmos.DrawCube(SRT, Xwing.hit ? Color.Red : Color.Magenta);

                    OBB = Xwing.OBBD;
                    SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Gizmos.DrawCube(SRT, Xwing.hit ? Color.Red : Color.Green);
                */
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
                DrawModel(Tie, tie.SRT, Vector3.Zero, drawType);
            }
            //else if ()
            //{

            //}
            else if (drawType == DrawType.Regular || drawType == DrawType.Shadowed)
            {
                assignEffectToModel(Tie, EffectLight);
                Matrix world;
                foreach (var mesh in Tie.Meshes)
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
                if (drawType == DrawType.Regular || drawType == DrawType.Shadowed)
                    wvp = world * Game.SelectedCamera.View * Game.SelectedCamera.Projection;
                else if (drawType == DrawType.DepthMap)
                    wvp = world * Game.LightCamera.View * Game.LightCamera.Projection;

                //Effect.Parameters["World"].SetValue(world);
                //EPbasicWorld.SetValue(world);
                EPmasterWorldViewProjection.SetValue(wvp);
                //EffectBloom.Parameters["WorldViewProjection"].SetValue(wvp);
                mesh.Draw();
            }

            //if (isBloomPass)
            //    assignEffectToModel(model, Effect);

        }

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
        EffectParameter EPbloomFilteredTexture;

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

        EffectTechnique ETmasterBasicColor;
        EffectTechnique ETmasterDepth;
        EffectTechnique ETmasterShadowed;

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

            EffectBloom.Parameters["enginesColor1"].SetValue(new Vector3(0f, 0.6f, 0.8f));
            EffectBloom.Parameters["enginesColor2"].SetValue(new Vector3(0.7f, 0.15f, 0f));
            EffectBloom.Parameters["laserColor1"].SetValue(new Vector3(0.8f, 0f, 0f));
            EffectBloom.Parameters["laserColor2"].SetValue(new Vector3(0f, 0.8f, 0f));
            EffectBloom.Parameters["laserColor3"].SetValue(new Vector3(0.8f, 0f, 0.8f));

            EPlightWorldViewProjection = EffectLight.Parameters["WorldViewProjection"];
            EPlightWorld = EffectLight.Parameters["World"];
            EPlightInverseTransposeWorld = EffectLight.Parameters["InverseTransposeWorld"];
            EPlightLightPosition = EffectLight.Parameters["lightPosition"];
            EPlightEyePosition = EffectLight.Parameters["eyePosition"];
            EPlightTexture = EffectLight.Parameters["baseTexture"];

            EPbloomWorldViewProjection = EffectBloom.Parameters["WorldViewProjection"];
            EPbloomTexture = EffectBloom.Parameters["baseTexture"];
            EPbloomFilteredTexture = EffectBloom.Parameters["bloomTexture"];

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



            ETmasterDepth = MasterEffect.Techniques["DepthPass"];
            ETmasterShadowed = MasterEffect.Techniques["DrawShadowedPCF"];
            ETmasterBasicColor = MasterEffect.Techniques["BasicColor"];

        }
        #endregion
    }
}